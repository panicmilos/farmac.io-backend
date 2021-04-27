using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IMedicalStaffGradeService : IGradeService
    {
        bool DidPatientGradeMedicalStaff(Guid patientId, Guid medicalStaffId);
        MedicalStaffGrade Read(Guid patientId, Guid medicalStaffId);
    }
}
