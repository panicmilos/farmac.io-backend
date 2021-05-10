using System;

namespace Farmacio_Services.Contracts
{
    public interface IDiscountService
    {
        int ReadDiscountFor(Guid pharmacyId, Guid patientId);
    }
}