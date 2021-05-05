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

        IEnumerable<PharmacyForERecipeDTO> FindPharmaciesWithMedicinesFrom(Guid eRecipeId);

        IEnumerable<PharmacyForERecipeDTO> SortPharmaciesWithMedicinesFrom(Guid eRecipeId, string sortCriteria, bool isAscending);

        Reservation CreateReservationFromERecipe(CreateReservationFromERecipeDTO createERecipeDTO);
    }
}