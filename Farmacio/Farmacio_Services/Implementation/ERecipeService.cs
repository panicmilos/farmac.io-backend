using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ERecipeService : CrudService<ERecipe>, IERecipeService
    {
        public ERecipeService(IRepository<ERecipe> repository) : base(repository)
        {

        }

        public override ERecipe Create(ERecipe eRecipe)
        {
            eRecipe.UniqueId = GetUniqueId();
            return base.Create(eRecipe);
        }

        public IEnumerable<ERecipe> ReadFor(Guid patientId)
        {
            return Read().Where(eRecipe => eRecipe.PatientId == patientId).ToList();
        }

        public bool WasMedicinePrescribedToPatient(Guid patientId, Guid medicineId)
        {
            var eRecipes = ReadFor(patientId);
            eRecipes = eRecipes.Where(eRecipe =>
            {
                var medicines = eRecipe.Medicines;
                return medicines.Where(medicine => medicine.MedicineId == medicineId).FirstOrDefault() != null;
            }).ToList();
            return eRecipes.Count() > 0;
        }

        private string GetUniqueId()
        {
            string uniqueId;
            do
            {
                uniqueId = StringUtils.RandomString(10);
            } while (!IsIdUnique(uniqueId));

            return uniqueId;
        }

        private bool IsIdUnique(string id)
        {
            return Read().FirstOrDefault(reservation => reservation.UniqueId == id) == default;
        }
    }
}
