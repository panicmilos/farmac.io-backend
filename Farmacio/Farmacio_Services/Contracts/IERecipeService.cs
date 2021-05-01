using Farmacio_Models.Domain;
using System;

namespace Farmacio_Services.Contracts
{
    public interface IERecipeService : ICrudService<ERecipe>
    {
        bool DidPatientHasBeenPrescribedMedicine(Guid patienrtId, Guid medicineId);
    }
}
