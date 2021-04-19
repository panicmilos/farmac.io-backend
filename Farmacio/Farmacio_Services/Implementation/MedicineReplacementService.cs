using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class MedicineReplacementService : CrudService<MedicineReplacement>, IMedicineReplacementService
    {
        public MedicineReplacementService(IRepository<MedicineReplacement> repository) :
            base(repository)
        {
        }

        public IEnumerable<MedicineReplacement> GetReplacementsFor(Guid medicineId)
        {
            return base.Read()
                .ToList()
                .Where(replacement => replacement.MedicineId == medicineId);
        }

        public void UpdateReplacementsFor(Guid medicineId, IEnumerable<MedicineReplacement> replacements)
        {
            var existingReplacementsFor = GetReplacementsFor(medicineId);

            foreach (var replacement in replacements)
            {
                var existingReplacement = existingReplacementsFor.FirstOrDefault(r => r.ReplacementMedicineId == replacement.ReplacementMedicineId);
                if (existingReplacement == null)
                {
                    replacement.MedicineId = medicineId;
                    Create(replacement);
                }
                else
                {
                    replacement.ReplacementMedicine = existingReplacement.ReplacementMedicine;
                }
            }
            foreach (var replacement in existingReplacementsFor)
            {
                if (replacements.FirstOrDefault(r => r.ReplacementMedicineId == replacement.ReplacementMedicineId) == null)
                {
                    Delete(replacement.Id);
                }
            }
        }

        public void DeleteReplacementsFor(Guid medicineId)
        {
            var replacementsFor = GetReplacementsFor(medicineId).ToList();

            replacementsFor.ForEach(replacement => base.Delete(replacement.Id));
        }
    }
}