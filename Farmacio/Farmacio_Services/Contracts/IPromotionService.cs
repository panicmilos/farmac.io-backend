using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface IPromotionService : ICrudService<Promotion>
    {
        IEnumerable<Promotion> ReadFor(Guid pharmacyId);
        IEnumerable<Promotion> ReadPageFor(Guid pharmacyId, PageDTO pageDto);
        IEnumerable<Promotion> ReadActiveFor(Guid pharmacyId);
        int ReadDiscountFor(Guid pharmacyId);
    }
}