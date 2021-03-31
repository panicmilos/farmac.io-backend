using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class DermatologistService : AccountService, IDermatologistService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;
        private readonly IAppointmentService _appointmentService;

        public DermatologistService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService, IAppointmentService appointmentService
            , IRepository<Account> repository)
            : base(emailVerificationService, repository)
        {
            _pharmacyService = pharmacyService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _appointmentService = appointmentService;
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Dermatologist).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.Dermatologist ? account : null;
        }

        public override Account Update(Account account)
        {
            var dermatologistAccount = Read(account.Id);
            if (dermatologistAccount == null)
            {
                throw new MissingEntityException();
            }

            dermatologistAccount.User.FirstName = account.User.FirstName;
            dermatologistAccount.User.LastName = account.User.LastName;
            dermatologistAccount.User.PhoneNumber = account.User.PhoneNumber;
            dermatologistAccount.User.PID = account.User.PID;
            dermatologistAccount.User.DateOfBirth = account.User.DateOfBirth;

            dermatologistAccount.User.Address.State = account.User.Address.State;
            dermatologistAccount.User.Address.City = account.User.Address.City;
            dermatologistAccount.User.Address.StreetName = account.User.Address.StreetName;
            dermatologistAccount.User.Address.StreetNumber = account.User.Address.StreetNumber;
            dermatologistAccount.User.Address.Lat = account.User.Address.Lat;
            dermatologistAccount.User.Address.Lng = account.User.Address.Lng;

            return base.Update(dermatologistAccount);
        }

        public IEnumerable<Account> ReadForPharmacy(Guid pharmacyId)
        {
            return FilterByPharmacyId(Read(), pharmacyId);
        }

        public Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistAccountId)
        {
            var dermatologistAccount = Read(dermatologistAccountId);
            return _dermatologistWorkPlaceService
                .GetDermatologistWorkPlaceInPharmacy(dermatologistAccount.UserId, pharmacyId) != null
                ? dermatologistAccount
                : null;
        }

        public IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name)
        {
            return FilterByPharmacyId(SearchByName(name), pharmacyId);
        }

        public Account AddToPharmacy(Guid pharmacyId, Guid dermatologistAccountId, WorkTime workTime)
        {
            var dermatologistAccount = Read(dermatologistAccountId);
            if (dermatologistAccount == null)
                throw new MissingEntityException("Dermatologist with the given Id not found.");

            var pharmacy = _pharmacyService.Read(pharmacyId);
            if (pharmacy == null)
                throw new MissingEntityException("Pharmacy with the given Id not found.");

            var workPlace =
                _dermatologistWorkPlaceService.GetDermatologistWorkPlaceInPharmacy(dermatologistAccount.UserId,
                    pharmacyId);

            if (workPlace != null)
                throw new AlreadyEmployedInPharmacyException("Dermatologist already employed in pharmacy.");

            ValidateWorkTime(workTime);

            if (!IsWorkTimeForDermatologistValid(workTime, dermatologistAccount.UserId))
                throw new WorkTimesOverlapException("Work time overlaps with another.");

            var newWorkPlace = new DermatologistWorkPlace
            {
                Pharmacy = pharmacy,
                WorkTime = workTime
            };

            _dermatologistWorkPlaceService.Create(newWorkPlace);

            return Update(dermatologistAccount);
        }

        public Account RemoveFromPharmacy(Guid pharmacyId, Guid dermatologistAccountId)
        {
            var dermatologistAccount = Read(dermatologistAccountId);
            if (dermatologistAccount == null)
                throw new MissingEntityException("Dermatologist with the given Id not found.");

            var workPlace =
                _dermatologistWorkPlaceService.GetDermatologistWorkPlaceInPharmacy(dermatologistAccount.UserId, pharmacyId);
            if (workPlace == null)
                throw new NotEmployedInPharmacyException("Dermatologist is not employed in the given pharmacy.");

            _dermatologistWorkPlaceService.Delete(workPlace.Id);
            return dermatologistAccount;
        }

        private IEnumerable<Account> FilterByPharmacyId(IEnumerable<Account> accounts, Guid pharmacyId)
        {
            return accounts.Where(a =>
                _dermatologistWorkPlaceService.GetDermatologistWorkPlaceInPharmacy(a.UserId, pharmacyId) != null);
        }

        private static void ValidateWorkTime(WorkTime workTime)
        {
            var workTimeHourDiff = Math.Abs(workTime.From.Hour - workTime.To.Hour);
            var workTimeMinuteDiff = Math.Abs(workTime.From.Minute - workTime.To.Minute);
            if (workTimeHourDiff < 1 || workTimeHourDiff > 8 || (workTimeHourDiff == 8 && workTimeMinuteDiff != 0))
                throw new InvalidWorkTimeException("Work time must be minimum 1 hour and maximum 8 hours long.");
        }

        private bool IsWorkTimeForDermatologistValid(WorkTime workTime, Guid dermatologistId)
        {
            var overlap = _dermatologistWorkPlaceService
                .GetDermatologistWorkPlaces(dermatologistId)
                .FirstOrDefault(wp => TimeIntervalUtils
                    .TimeIntervalTimesOverlap(wp.WorkTime.From, wp.WorkTime.To, workTime.From, workTime.To));
            return overlap == null;
        }

        public IEnumerable<PatientDTO> GetPatients(Guid dermatologistAccountId)
        {
            var account = Read(dermatologistAccountId);
            if (account == null)
                throw new MissingEntityException("Dermatologist account not found.");

            return _appointmentService
                .ReadForMedicalStaff(account.UserId)
                .Where(ap => ap.IsReserved && ap.PatientId != null)
                .Select(ap => new PatientDTO
                {
                    Id = ap.PatientId.Value,
                    FirstName = ap.Patient.FirstName,
                    LastName = ap.Patient.LastName,
                    DateOfBirth = ap.Patient.DateOfBirth,
                    Address = ap.Patient.Address,
                    PhoneNumber = ap.Patient.PhoneNumber,
                    AppointmentDate = ap.DateTime
                });
        }
    }
}