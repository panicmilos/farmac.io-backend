using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IPatientAllergyService : ICrudService<PatientAllergy>
    {
        IEnumerable<PatientAllergy> Create(PatientAllergyDTO request);
        IEnumerable<SmallMedicineDTO> GetPatientsAllergies(Guid patientId);
    }
}
