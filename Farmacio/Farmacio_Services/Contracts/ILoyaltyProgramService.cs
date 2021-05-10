using Farmacio_Models.Domain;
using System;

namespace Farmacio_Services.Contracts
{
    public interface ILoyaltyProgramService : ICrudService<LoyaltyProgram>
    {
        int ReadDiscountFor(Guid patientId);
    }
}