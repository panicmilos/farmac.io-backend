﻿using Farmacio_Models.Domain;
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
        public MedicalStaffGradeService(IRepository<Grade> repository, IPatientService patientService):
            base(repository)
        {
            _patientService = patientService;
        }

        public bool DidPatientGradeMedicalStaff(Guid patientId, Guid medicalStaffId)
        {
            var grades = Read().Where(grade => {
                var medicalStaffGrade = grade as MedicalStaffGrade;
                return medicalStaffGrade?.PatientId == patientId && medicalStaffGrade?.MedicalStaffId == medicalStaffId;
            });
            return grades.FirstOrDefault() != null;
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
    }
}
