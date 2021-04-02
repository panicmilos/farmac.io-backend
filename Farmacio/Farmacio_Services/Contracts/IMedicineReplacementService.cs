using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IMedicineReplacementService : ICrudService<MedicineReplacement>
    {
        IEnumerable<Medicine> GetReplacementsFor(Guid medicineId);

        void DeleteReplacementsFor(Guid medicineId);
    }
}