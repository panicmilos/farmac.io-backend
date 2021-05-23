using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.DTO;
using Farmacio_Services.Implementation.Validation;

namespace Farmacio_Services.Implementation
{
    public class DermatologistService : MedicalStaffService, IDermatologistService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;

        public DermatologistService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService, IAppointmentService appointmentService
            , IAccountRepository repository)
            : base(emailVerificationService, appointmentService, repository)
        {
            _pharmacyService = pharmacyService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
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

        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if (existingAccount == null)
                throw new MissingEntityException("Dermatologist account not found.");
            return existingAccount;
        }

        public IEnumerable<Account> ReadForPharmacy(Guid pharmacyId)
        {
            return FilterByPharmacyId(Read(), pharmacyId);
        }

        public Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistAccountId)
        {
            var dermatologistAccount = Read(dermatologistAccountId);
            return _dermatologistWorkPlaceService
                .GetWorkPlaceInPharmacyFor(dermatologistAccount.UserId, pharmacyId) != null
                ? dermatologistAccount
                : null;
        }

        public IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name)
        {
            return FilterByPharmacyId(SearchByName(name), pharmacyId);
        }

        public override IEnumerable<Account> ReadBy(MedicalStaffFilterParamsDTO filterParams)
        {
            var pharmacyId = filterParams.PharmacyId;
            return pharmacyId != null
                ? FilterByPharmacyId(base.ReadBy(filterParams), new Guid(pharmacyId))
                : base.ReadBy(filterParams);
        }

        public Account AddToPharmacy(Guid pharmacyId, Guid dermatologistAccountId, WorkTime workTime)
        {
            var dermatologistAccount = TryToRead(dermatologistAccountId);
            var pharmacy = _pharmacyService.TryToRead(pharmacyId);

            var workPlace =
                _dermatologistWorkPlaceService.GetWorkPlaceInPharmacyFor(dermatologistAccount.UserId, pharmacyId);

            if (workPlace != null)
                throw new AlreadyEmployedInPharmacyException("Dermatologist already employed in pharmacy.");

            WorkTimeValidation.ValidateWorkHours(workTime);

            if (!IsWorkTimeForDermatologistValid(workTime, dermatologistAccount.UserId))
                throw new WorkTimesOverlapException("Work time overlaps with another.");

            var newWorkPlace = new DermatologistWorkPlace
            {
                DermatologistId = dermatologistAccount.UserId,
                Pharmacy = pharmacy,
                WorkTime = workTime
            };

            _dermatologistWorkPlaceService.Create(newWorkPlace);

            return Update(dermatologistAccount);
        }

        public Account RemoveFromPharmacy(Guid pharmacyId, Guid dermatologistAccountId)
        {
            var dermatologistAccount = TryToRead(dermatologistAccountId);

            var workPlace =
                _dermatologistWorkPlaceService.GetWorkPlaceInPharmacyFor(dermatologistAccount.UserId, pharmacyId);
            if (workPlace == null)
                throw new NotEmployedInPharmacyException("Dermatologist is not employed in the given pharmacy.");

            var hasFutureAppointments = _appointmentService.ReadForMedicalStaffInPharmacy(dermatologistAccount.UserId, pharmacyId)
                .Where(appointment => appointment.DateTime > DateTime.Now)
                .Any();

            if (hasFutureAppointments)
            {
                throw new BadLogicException("You cannot remove the dermatologist from the pharmacy because he has appointments in the future.");
            }

            _dermatologistWorkPlaceService.Delete(workPlace.Id);
            return dermatologistAccount;
        }

        private IEnumerable<Account> FilterByPharmacyId(IEnumerable<Account> accounts, Guid pharmacyId)
        {
            return accounts.Where(a =>
                _dermatologistWorkPlaceService.GetWorkPlaceInPharmacyFor(a.UserId, pharmacyId) != null);
        }

        private bool IsWorkTimeForDermatologistValid(WorkTime workTime, Guid dermatologistId)
        {
            var overlap = _dermatologistWorkPlaceService
                .GetWorkPlacesFor(dermatologistId)
                .FirstOrDefault(wp => TimeIntervalUtils
                    .TimeIntervalTimesOverlap(wp.WorkTime.From, wp.WorkTime.To, workTime.From, workTime.To));
            return overlap == null;
        }
    }
}