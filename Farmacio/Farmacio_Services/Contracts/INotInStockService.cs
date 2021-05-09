using System;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface INotInStockService : ICrudService<NotInStock>
    {
        IEnumerable<NotInStock> ReadFor(Guid pharmacyId);
        NotInStock MarkSeen(Guid notInStockId);
    }
}
