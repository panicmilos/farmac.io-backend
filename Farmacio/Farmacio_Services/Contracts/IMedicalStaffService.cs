﻿using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IMedicalStaffService : IAccountService
    {
        IEnumerable<PatientDTO> ReadPatientsForMedicalStaff(Guid medicalId);
        IEnumerable<PatientDTO> ReadSortedPatientsForMedicalStaff(Guid medicalId, string crit, bool isAsc);
        IEnumerable<PatientDTO> SearchPatientsForMedicalStaff(Guid medicalId, string name);
        public IEnumerable<Account> ReadBy(MedicalStaffFilterParamsDTO filterParams);
        IEnumerable<Account> ReadPageBy(MedicalStaffFilterParamsDTO filterParams, PageDTO pageDto);
        public Account UpdateGrade(MedicalStaff medicalStaff);
    }
}
