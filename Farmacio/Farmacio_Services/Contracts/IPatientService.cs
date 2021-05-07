using Farmacio_Models.Domain;
using System;

namespace Farmacio_Services.Contracts
{
    public interface IPatientService : IAccountService
    {
        bool HasExceededLimitOfNegativePoints(Guid patientId);

        Account UpdateLoyaltyProgram(Guid patientAccountId, Guid? loyaltyProgramId);
    }
}