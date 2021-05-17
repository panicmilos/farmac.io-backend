using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IMedicalStaffService : IAccountService
    {
        IEnumerable<PatientDTO> ReadPatientsForMedicalStaff(Guid medicalAccountId);
        IEnumerable<PatientDTO> ReadPatientsForMedicalStaffBy(Guid medicalAccountId, PatientSearchParams searchParams);
        IEnumerable<PatientDTO> ReadPageOfPatientsForMedicalStaffBy(Guid medicalAccountId, PatientSearchParams searchParams, PageDTO pageDTO);
        public IEnumerable<Account> ReadBy(MedicalStaffFilterParamsDTO filterParams);
        IEnumerable<Account> ReadPageBy(MedicalStaffFilterParamsDTO filterParams, PageDTO pageDto);
        public Account UpdateGrade(MedicalStaff medicalStaff);
    }
}
