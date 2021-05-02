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
        private readonly IMedicalStaffService _medicalStaffService;
        private readonly IPatientService _patientService;
        public MedicalStaffGradeService(IRepository<Grade> repository, IMedicalStaffService medicalStaffService, IPatientService patientService):
            base(repository)
        {
            _medicalStaffService = medicalStaffService;
            _patientService = patientService;
        }

        public bool DidPatientGradeMedicalStaff(Guid patientId, Guid medicalStaffId)
        {
            var grades = Read().Where(grade => {
                var medicalStaffGrade = grade as MedicalStaffGrade;
                if(medicalStaffGrade != null)
                {
                    return medicalStaffGrade.PatientId == patientId && medicalStaffGrade.MedicalStaffId == medicalStaffId;
                }
                return false;
            });
            return grades.FirstOrDefault() != null;
        }

        public MedicalStaffGrade Read(Guid patientId, Guid medicalStaffId)
        {
            _patientService.TryToRead(patientId);

            var medicalStaff = _medicalStaffService.ReadByUserId(medicalStaffId);
            if(medicalStaff == null)
            {
                throw new MissingEntityException("The given dermatologist does not exist in the system.");
            }

            return Read().Where(grade =>
            {
                var medicalStafGrade = grade as MedicalStaffGrade;
                return medicalStafGrade.PatientId == patientId && medicalStafGrade.MedicalStaffId == medicalStaffId;
            }).FirstOrDefault() as MedicalStaffGrade;
        }
    }
}
