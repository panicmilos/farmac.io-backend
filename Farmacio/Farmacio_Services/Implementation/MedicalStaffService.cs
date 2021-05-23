using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class MedicalStaffService : AccountService, IMedicalStaffService
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;
        private readonly ICrudService<WorkTime> _workTimeService;

        public MedicalStaffService(
            IEmailVerificationService emailVerificationService 
            , IAppointmentService appointmentService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService
            , ICrudService<WorkTime> workTimeService
            , IAccountRepository repository)
            : base(emailVerificationService, repository)
        {
            _appointmentService = appointmentService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _workTimeService = workTimeService;
        }

        public IEnumerable<PatientDTO> ReadPatientsForMedicalStaff(Guid medicalAccountId)
        {
            var medicalAccount = TryToRead(medicalAccountId);

            return _appointmentService
                .ReadForMedicalStaff(medicalAccount.UserId)
                .Where(appointment => appointment.IsReserved && appointment.PatientId != null)
                .GroupBy(appointment => appointment.PatientId)
                .Select(group => group.Where(appointment => appointment.DateTime == group.Max(appointment => appointment.DateTime)).First())
                .Select(appointment => new PatientDTO
                {
                    PatientId = appointment.PatientId.Value,
                    FirstName = appointment.Patient.FirstName,
                    LastName = appointment.Patient.LastName,
                    DateOfBirth = appointment.Patient.DateOfBirth,
                    Address = appointment.Patient.Address,
                    PhoneNumber = appointment.Patient.PhoneNumber,
                    AppointmentDate = appointment.DateTime
                });
        }

        private IEnumerable<PatientDTO> SortPatientDTOs(IEnumerable<PatientDTO> patients, string sortCriteria, bool isAscending)
        {
            var sortingCriteria = new Dictionary<string, Func<PatientDTO, object>>()
            {
                { "firstName", p => p.FirstName },
                { "lastName", p => p.LastName },
                { "appointmentDate", p => p.AppointmentDate }
            };
            if (sortingCriteria.TryGetValue(sortCriteria ?? "", out var sortCrit))
                patients = isAscending ? patients.OrderBy(sortCrit) : patients.OrderByDescending(sortCrit);
            return patients;
        }

        public virtual IEnumerable<Account> ReadBy(MedicalStaffFilterParamsDTO filterParams)
        {
            var (name, pharmacyId, gradeFrom, gradeTo) = filterParams;
            return SearchByName(name)
                .Where(acc =>
                {
                    var dermatologist = (MedicalStaff)acc.User;
                    return (gradeFrom == 0 || dermatologist.AverageGrade >= gradeFrom)
                           && (gradeTo == 0 || dermatologist.AverageGrade <= gradeTo);
                });
        }

        public IEnumerable<Account> ReadPageBy(MedicalStaffFilterParamsDTO filterParams, PageDTO pageDto)
        {
            return PaginationUtils<Account>.Page(ReadBy(filterParams), pageDto);
        }

        public Account UpdateGrade(MedicalStaff medicalStaff)
        {
            var staffAccount = ReadByUserId(medicalStaff.Id);
            var staffUser = staffAccount.User as MedicalStaff;
            if (staffUser == null) return base.Update(staffAccount);
            staffUser.NumberOfGrades = medicalStaff.NumberOfGrades;
            staffUser.AverageGrade = medicalStaff.AverageGrade;
            staffAccount.User = staffUser;

            return base.Update(staffAccount);
        }

        public override Account Delete(Guid id)
        {
            var existingAccount = TryToRead(id);
            if(_appointmentService.ReadForMedicalStaff(existingAccount.UserId)
                .Any(appointment => appointment.IsReserved))
                throw new BadLogicException("The medical staff can't be deleted because it has upcoming appointments.");
            if (existingAccount.Role == Role.Dermatologist)
            {
                _dermatologistWorkPlaceService
                    .GetWorkPlacesFor(existingAccount.UserId)
                    .ToList().ForEach(dermatologistWorkPlace =>
                    {
                        _dermatologistWorkPlaceService.Delete(dermatologistWorkPlace.Id);
                        _workTimeService.Delete(dermatologistWorkPlace.WorkTimeId);
                    });
            } else if (existingAccount.Role == Role.Pharmacist)
            {
                _workTimeService.Delete(((Pharmacist) existingAccount.User).WorkTimeId);
            }
            return base.Delete(id);
        }

        public IEnumerable<PatientDTO> ReadPatientsForMedicalStaffBy(Guid medicalAccountId, PatientSearchParams searchParams)
        {
            var (name, sortCriteria, isAscending) = searchParams;
            var patients = ReadPatientsForMedicalStaff(medicalAccountId).Where(p =>
                string.IsNullOrEmpty(name) ||  $"{p.FirstName.ToLower()} {p.LastName.ToLower()}".Contains(name.ToLower()));
            return SortPatientDTOs(patients, sortCriteria, isAscending);
        }

        public IEnumerable<PatientDTO> ReadPageOfPatientsForMedicalStaffBy(Guid medicalAccountId, PatientSearchParams searchParams, PageDTO pageDTO)
        {
            return PaginationUtils<PatientDTO>.Page(ReadPatientsForMedicalStaffBy(medicalAccountId, searchParams), pageDTO);
        }
    }
}