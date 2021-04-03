using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IMedicineReplacementService : ICrudService<MedicineReplacement>
    {
        IEnumerable<MedicineReplacement> GetReplacementsFor(Guid medicineId);

        void UpdateReplacementsFor(Guid medicineId, IEnumerable<MedicineReplacement> replacements);

        void DeleteReplacementsFor(Guid medicineId);
    }
}