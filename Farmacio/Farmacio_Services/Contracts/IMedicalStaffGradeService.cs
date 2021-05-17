using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IMedicalStaffGradeService : IGradeService
    {
        bool DidPatientGradeMedicalStaff(Guid patientId, Guid medicalStaffId);

        MedicalStaffGrade Read(Guid patientId, Guid medicalStaffId);

        Grade GradeMedicalStaff(MedicalStaffGrade grade);

        IEnumerable<Account> ReadDermatologistThatPatientCanRate(Guid patientId);

        IEnumerable<Account> ReadDermatologistThatPatientRated(Guid patientId);

        IEnumerable<Account> ReadPharmacistsThatPatientCanRate(Guid patientId);

        MedicalStaffGrade ChangeGrade(Guid gradeId, int value);
        IEnumerable<Account> ReadPharmacistsThatPatientRated(Guid patientId);

        IEnumerable<Account> ReadPharmacistsThatPatientRatedPageTo(Guid patientId, PageDTO pageDTO);

        IEnumerable<Account> ReadDermatologistThatPatientRatedPageTo(Guid patientId, PageDTO pageDTO);
    }
}
