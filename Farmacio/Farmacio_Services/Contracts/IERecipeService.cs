using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IERecipeService : ICrudService<ERecipe>
    {
        bool WasMedicinePrescribedToPatient(Guid patienrtId, Guid medicineId);
        IEnumerable<ERecipe> ReadFor(Guid patientId);
        IEnumerable<ERecipeDTO> SortFor(Guid patientId, ERecipesSortFilterParams sortFilterParams);
    }
}
