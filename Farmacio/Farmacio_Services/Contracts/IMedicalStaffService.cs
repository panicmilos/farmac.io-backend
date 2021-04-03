using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IMedicalStaffService : IAccountService
    {
        IEnumerable<PatientDTO> GetPatients(Guid medicalAccountId);
    }
}
