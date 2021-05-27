using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IERecipeService : ICrudService<ERecipe>
    {
        bool WasMedicinePrescribedToPatient(Guid patientId, Guid medicineId);

        IEnumerable<ERecipe> ReadFor(Guid patientId);

        IEnumerable<PharmacyForERecipeDTO> FindPharmaciesWithMedicinesFrom(Guid eRecipeId);

        IEnumerable<PharmacyForERecipeDTO> SortPharmaciesWithMedicinesFrom(Guid eRecipeId, string sortCriteria, bool isAscending);

        Reservation CreateReservationFromERecipe(CreateReservationFromERecipeDTO createERecipeDTO);

        IEnumerable<ERecipeDTO> SortFor(Guid patientId, ERecipesSortFilterParams sortFilterParams);

        IEnumerable<ERecipeMedicine> ReadMedicinesFromERecipe(Guid eRecipeId);

        IEnumerable<ERecipeDTO> SortForPageTo(Guid patientId, ERecipesSortFilterParams sortFilterParams, PageDTO pageDTO);
    }
}