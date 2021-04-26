﻿using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface IDermatologistService : IMedicalStaffService
    {
        IEnumerable<Account> ReadForPharmacy(Guid pharmacyId);

        Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistId);

        IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name);

        Account AddToPharmacy(Guid pharmacyId, Guid dermatologistId, WorkTime workTime);

        Account RemoveFromPharmacy(Guid pharmacyId, Guid dermatologistId);
        Grade GradeDermatologist(MedicalStaffGrade grade);
        IEnumerable<Account> ReadThatPatientCanRate(Guid patientId);
        IEnumerable<Account> ReadThatPatientRated(Guid patientId);
    }
}