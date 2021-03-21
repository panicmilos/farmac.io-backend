using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Supplier : User
    {
        public virtual List<SupplierOffer> Offers { get; set; }
    }
}
