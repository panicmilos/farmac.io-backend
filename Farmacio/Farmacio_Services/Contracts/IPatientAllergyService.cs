using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IPatientAllergyService : ICrudService<PatientAllergy>
    {
        IEnumerable<PatientAllergy> CreateAllergies(PatientAllergyDTO request);
        IEnumerable<SmallMedicineDTO> GetPatientsAllergies(Guid patientId);
        IEnumerable<CheckMedicineDTO> ForEachMedicineInListCheckIfPatientHasAnAllergyToIt(IEnumerable<CheckMedicineDTO> medicineDTOs, Guid patientId);
        PatientAllergy Delete(Guid patientId, Guid medicineId);
    }
}
