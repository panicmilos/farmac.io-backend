using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmacio_Services.Implementation
{
    public class MedicalStaffGradeService : GradeService, IMedicalStaffGradeService
    {
        private readonly IPatientService _patientService;
        private readonly IMedicalStaffService _medicalStaffService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDermatologistService _dermatologistService;
        private readonly IPharmacistService _pharmacistService;

        public MedicalStaffGradeService(IRepository<Grade> repository, IPatientService patientService
            , IMedicalStaffService medicalStaffService
            , IAppointmentService appointmentService
            , IDermatologistService dermatologistService
            , IPharmacistService pharmacistService):
            base(repository)
        {
            _patientService = patientService;
            _medicalStaffService = medicalStaffService;
            _appointmentService = appointmentService;
            _dermatologistService = dermatologistService;
            _pharmacistService = pharmacistService;
        }

        public bool DidPatientGradeMedicalStaff(Guid patientId, Guid medicalStaffId)
        {
            var grades = Read().Where(grade => {
                var medicalStaffGrade = grade as MedicalStaffGrade;
                return medicalStaffGrade?.PatientId == patientId && medicalStaffGrade?.MedicalStaffId == medicalStaffId;
            });
            return grades?.FirstOrDefault() != null;
        }

        public MedicalStaffGrade Read(Guid patientId, Guid medicalStaffId)
        {
            _patientService.TryToRead(patientId);

            return Read().Where(grade =>
            {
                var medicalStafGrade = grade as MedicalStaffGrade;
                return medicalStafGrade?.PatientId == patientId && medicalStafGrade?.MedicalStaffId == medicalStaffId;
            }).FirstOrDefault() as MedicalStaffGrade;
        }

        public Grade GradeMedicalStaff(MedicalStaffGrade grade)
        {
            var medicalStaff = _medicalStaffService.ReadByUserId(grade.MedicalStaffId);
            if (medicalStaff == null)
            {
                throw new MissingEntityException("The given medical staff does not exist.");
            }

            if (!_appointmentService.DidPatientHaveAppointmentWithMedicalStaff(grade.PatientId, grade.MedicalStaffId))
            {
                throw new BadLogicException("The patient cannot rate the medical staff because he did not have an appointment with him.");
            }

            if (DidPatientGradeMedicalStaff(grade.PatientId, grade.MedicalStaffId))
            {
                throw new BadLogicException("The patient has already rated a medical staff.");
            }

            grade = base.Create(grade) as MedicalStaffGrade;
            var medicalStaffUser = medicalStaff.User as MedicalStaff;
            medicalStaffUser.AverageGrade = (medicalStaffUser.NumberOfGrades * medicalStaffUser.AverageGrade + grade.Value) / ++medicalStaffUser.NumberOfGrades;
            medicalStaff.User = medicalStaffUser;
            _medicalStaffService.UpdateGrade(medicalStaffUser);

            return grade;
        }

        public IEnumerable<Account> ReadDermatologistThatPatientCanRate(Guid patientId)
        {
            return _dermatologistService.Read().Where(dermatologist => _appointmentService.DidPatientHaveAppointmentWithMedicalStaff(patientId, dermatologist.UserId) &&
            !DidPatientGradeMedicalStaff(patientId, dermatologist.UserId)).ToList();
        }

        public IEnumerable<Account> ReadDermatologistThatPatientRated(Guid patientId)
        {
            return _dermatologistService.Read().Where(dermatologist => DidPatientGradeMedicalStaff(patientId, dermatologist.UserId)).ToList();
        }
        public IEnumerable<Account> ReadPharmacistsThatPatientCanRate(Guid patientId)
        {
            return _pharmacistService.Read().Where(pharmacists => _appointmentService.DidPatientHaveAppointmentWithMedicalStaff(patientId, pharmacists.UserId) &&
            !DidPatientGradeMedicalStaff(patientId, pharmacists.UserId)).ToList();
        }

        public MedicalStaffGrade ChangeGrade(Guid gradeId, int value)
        {
            var grade = TryToRead(gradeId);

            var medicalStaffGrade = grade as MedicalStaffGrade;

            var medicalStaff = _medicalStaffService.ReadByUserId(medicalStaffGrade.MedicalStaffId);
            if (medicalStaff == null)
            {
                throw new MissingEntityException("The given medical staff does not exist.");
            }

            if (value < 1 || value > 5)
            {
                throw new BadLogicException("The grade can be between 0 and 5.");
            }

            if (grade.Value == value)
            {
                throw new BadLogicException("The given grade is same as previous.");
            }

            var medicalStaffUser = medicalStaff.User as MedicalStaff;

            medicalStaffUser.AverageGrade = (medicalStaffUser.AverageGrade * medicalStaffUser.NumberOfGrades - grade.Value + value) / medicalStaffUser.NumberOfGrades;
            _medicalStaffService.UpdateGrade(medicalStaffUser);

            medicalStaffGrade.Value = value;

            return base.Update(medicalStaffGrade) as MedicalStaffGrade;
        }

        public IEnumerable<Account> ReadPharmacistsThatPatientRated(Guid patientId)
        {
            return _pharmacistService.Read().Where(pharmacist => DidPatientGradeMedicalStaff(patientId, pharmacist.UserId)).ToList();
        }
    }
}
