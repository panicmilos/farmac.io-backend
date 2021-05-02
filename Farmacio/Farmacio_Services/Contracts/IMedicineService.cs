using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IMedicineService : ICrudService<Medicine>
    {
        FullMedicineDTO ReadFullMedicine(Guid id);

        FullMedicineDTO Create(FullMedicineDTO fullMedicineDto);

        FullMedicineDTO Update(FullMedicineDTO fullMedicineDto);

        IEnumerable<SmallMedicineDTO> ReadForDisplay();

        IEnumerable<string> ReadMedicineNames(Guid pharmacyId);

        IEnumerable<CheckMedicineDTO> ReadMedicinesOrReplacementsByName(Guid pharmacyId, string name);

        IEnumerable<SmallMedicineDTO> ReadBy(MedicineSearchParams searchParams);

        IEnumerable<string> ReadMedicineTypes();

        Medicine UpdateGrade(Medicine medicine);
    }
}