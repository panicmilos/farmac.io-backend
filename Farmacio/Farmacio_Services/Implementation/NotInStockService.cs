using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class NotInStockService : CrudService<NotInStock>, INotInStockService
    {
        public NotInStockService(IRepository<NotInStock> repository) : base(repository)
        {
        }


        public IEnumerable<NotInStock> ReadFor(Guid pharmacyId)
        {
            return Read().Where(notInStock => notInStock.PharmacyId == pharmacyId).ToList();
        }

        public NotInStock MarkSeen(Guid notInStockId)
        {
            var notInStock = TryToRead(notInStockId);
            notInStock.IsSeen = true;
            return base.Update(notInStock);
        }
    }
}