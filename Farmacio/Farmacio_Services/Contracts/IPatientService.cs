﻿using System;

namespace Farmacio_Services.Contracts
{
    public interface IPatientService : IAccountService
    {
        bool HasExceededLimitOfNegativePoints(Guid patientId);
    }
}