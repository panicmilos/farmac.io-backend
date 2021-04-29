using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IPromotionService : ICrudService<Promotion>
    {
        IEnumerable<Promotion> ReadFor(Guid pharmacyId);
        IEnumerable<Promotion> ReadActiveFor(Guid pharmacyId);
    }
}