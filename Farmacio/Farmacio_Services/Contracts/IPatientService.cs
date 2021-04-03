using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IPatientService : IAccountService
    {
        bool ExceededLimitOfNegativePoints(Guid patientId);
    }
}