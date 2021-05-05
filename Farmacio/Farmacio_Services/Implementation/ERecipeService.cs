using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ERecipeService : CrudService<ERecipe>, IERecipeService
    {
        private readonly IPatientService _patientService;
        public ERecipeService(IRepository<ERecipe> repository, IPatientService patientService) : base(repository)
        {
            _patientService = patientService;
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

        public IEnumerable<ERecipeDTO> SortFor(Guid patientUserId, ERecipesSortFilterParams sortFilterParams)
        {
            var patient = _patientService.ReadByUserId(patientUserId);

            if (patient == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }

            (string sortCriteria, bool isAsc, bool isUsed) = sortFilterParams;

            var eRecipes = Read().ToList().Where(eRecipe => eRecipe.PatientId == patientUserId && eRecipe.IsUsed == isUsed);

            var sortingCriteria = new Dictionary<string, Func<ERecipe, object>>()
            {
                { "issuingDate", e => e.IssuingDate }
            };

            if (sortingCriteria.TryGetValue(sortCriteria ?? "", out var sortingCriterion))
            {
                eRecipes = isAsc ? eRecipes.OrderBy(sortingCriterion) : eRecipes.OrderByDescending(sortingCriterion);
            }

            return eRecipes.Select(eRecipe => new ERecipeDTO
            {
                Id = eRecipe.Id,
                IssuingDate = eRecipe.IssuingDate,
                IsUsed = eRecipe.IsUsed,
                UniqueId = eRecipe.UniqueId
            });
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
