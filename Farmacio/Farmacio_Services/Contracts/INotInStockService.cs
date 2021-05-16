using System;
using Farmacio_Models.Domain;
using System.Collections.Generic;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface INotInStockService : ICrudService<NotInStock>
    {
        IEnumerable<NotInStock> ReadFor(Guid pharmacyId);
        IEnumerable<NotInStock> ReadPageFor(Guid pharmacyId, bool? isSeen, PageDTO pageDto);
        NotInStock MarkSeen(Guid notInStockId);
    }
}
