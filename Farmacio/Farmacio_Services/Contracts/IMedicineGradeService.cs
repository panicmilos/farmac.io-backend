using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IMedicineGradeService: IGradeService
    {
        IEnumerable<Medicine> ReadThatPatientCanRate(Guid patientId);

        MedicineGrade ChangeGrade(Guid medicineGradeId, int value);

        IEnumerable<Medicine> ReadMedicinesThatPatientRated(Guid patientId);

        MedicineGrade Read(Guid patientId, Guid medicineId);

        IEnumerable<Medicine> ReadMedicinesThatPatientRatedPageTo(Guid patientId, PageDTO pageDTO);
    }
}
