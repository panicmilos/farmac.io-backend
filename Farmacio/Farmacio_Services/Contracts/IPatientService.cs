using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IPatientService : IAccountService
    {
        bool HasExceededLimitOfNegativePoints(Guid patientId);
        IEnumerable<PatientDTO> ReadPatientsForMedicalStaff(Guid medicalId);
        IEnumerable<PatientDTO> ReadSortedPatientsForMedicalStaff(Guid medicalId, string crit, bool isAsc);
        IEnumerable<PatientDTO> SearchPatientsForMedicalStaff(Guid medicalId, string name);
    }
}