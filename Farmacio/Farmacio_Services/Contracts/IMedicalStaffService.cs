using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IMedicalStaffService : IAccountService
    {
        IEnumerable<PatientDTO> ReadPatientsForMedicalStaff(Guid medicalId);
        IEnumerable<PatientDTO> ReadSortedPatientsForMedicalStaff(Guid medicalId, string crit, bool isAsc);
        IEnumerable<PatientDTO> SearchPatientsForMedicalStaff(Guid medicalId, string name);
    }
}
