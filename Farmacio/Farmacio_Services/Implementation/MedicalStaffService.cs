using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class MedicalStaffService : AccountService, IMedicalStaffService
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMedicalStaffGradeService _medicalStaffGradeService;

        public MedicalStaffService(IEmailVerificationService emailVerificationService, IAppointmentService appointmentService, IMedicalStaffGradeService medicalStaffGradeService,
            IRepository<Account> repository)
            : base(emailVerificationService, repository)
        {
            _appointmentService = appointmentService;
            _medicalStaffGradeService = medicalStaffGradeService;
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

        public IEnumerable<PatientDTO> ReadSortedPatientsForMedicalStaff(Guid medicalAccountId, string crit, bool isAsc)
        {
            var sortingCriteria = new Dictionary<string, Func<PatientDTO, object>>()
            {
                { "firstName", p => p.FirstName },
                { "lastName", p => p.LastName },
                { "appointmentDate", p => p.AppointmentDate }
            };
            var patients = ReadPatientsForMedicalStaff(medicalAccountId);
            if (sortingCriteria.TryGetValue(crit ?? "", out var sortCrit))
                patients = isAsc ? patients.OrderBy(sortCrit) : patients.OrderByDescending(sortCrit);
            return patients;
        }
        
        public virtual IEnumerable<Account> ReadBy(MedicalStaffFilterParamsDTO filterParams)
        {
            var (name, pharmacyId, gradeFrom, gradeTo) = filterParams;
            return SearchByName(name)
                .Where(acc =>
                {
                    var dermatologist = (MedicalStaff) acc.User;
                    return (gradeFrom == 0 || dermatologist.AverageGrade >= gradeFrom) 
                           && (gradeTo == 0 || dermatologist.AverageGrade <= gradeTo);
                });
        }

        public IEnumerable<PatientDTO> SearchPatientsForMedicalStaff(Guid medicalAccountId, string name)
        {
            return ReadPatientsForMedicalStaff(medicalAccountId).Where(p =>
                name == null ||
                $"{p.FirstName.ToLower()} {p.LastName.ToLower()}".Contains(name.ToLower()));
        }

        public Account UpdateGrade(MedicalStaff medicalStaff)
        {
            var staffAccount = ReadByUserId(medicalStaff.Id);
            var staffUser = staffAccount.User as MedicalStaff;
            staffUser.NumberOfGrades = medicalStaff.NumberOfGrades;
            staffUser.AverageGrade = medicalStaff.AverageGrade;
            staffAccount.User = staffUser;
            return base.Update(staffAccount);
        }

        public Grade GradeMedicalStaff(MedicalStaffGrade grade)
        {
            var medicalStaff = ReadByUserId(grade.MedicalStaffId);
            if (medicalStaff == null)
            {
                throw new MissingEntityException("The given medical staff does not exist.");
            }

            if (!_appointmentService.DidPatientHaveAppointmentWithDermatologist(grade.PatientId, grade.MedicalStaffId))
            {
                throw new BadLogicException("The patient cannot rate the medical staff because he did not have an appointment with him.");
            }

            if (_medicalStaffGradeService.DidPatientGradeMedicalStaff(grade.PatientId, grade.MedicalStaffId))
            {
                throw new BadLogicException("The patient has already been rate a medical staff.");
            }

            grade = _medicalStaffGradeService.Create(grade) as MedicalStaffGrade;
            var medicalStaffUser = medicalStaff.User as MedicalStaff;
            medicalStaffUser.AverageGrade = (medicalStaffUser.NumberOfGrades * medicalStaffUser.AverageGrade + grade.Value) / ++medicalStaffUser.NumberOfGrades;
            medicalStaff.User = medicalStaffUser;
            UpdateGrade(medicalStaffUser);

            return grade;
        }
    }
}
