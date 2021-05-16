using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;

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

        public IEnumerable<NotInStock> ReadPageFor(Guid pharmacyId, bool? isSeen, PageDTO pageDto)
        {
            return PaginationUtils<NotInStock>.Page(
                ReadFor(pharmacyId).Where(notInStock => isSeen == null || notInStock.IsSeen == isSeen.Value), pageDto);
        }

        public NotInStock MarkSeen(Guid notInStockId)
        {
            var notInStock = TryToRead(notInStockId);
            notInStock.IsSeen = true;
            return base.Update(notInStock);
        }
    }
}