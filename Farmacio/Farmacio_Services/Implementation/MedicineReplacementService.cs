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

        public IEnumerable<Medicine> GetReplacementsFor(Guid medicineId)
        {
            return base.Read()
                .ToList()
                .Where(replacement => replacement.MedicineId == medicineId)
                .Select(replacement => replacement.ReplacementMedicine);
        }

        public void DeleteReplacementsFor(Guid medicineId)
        {
            var replacementsFor = Read()
                .Where(replacement => replacement.MedicineId == medicineId)
                .ToList();

            replacementsFor.ForEach(replacement => base.Delete(replacement.Id));
        }
    }
}