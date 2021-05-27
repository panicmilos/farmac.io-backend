using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyService : ICrudService<Pharmacy>
    {
        IEnumerable<SmallPharmacyDTO> ReadForHomePage();

        IEnumerable<PharmaciesOfMedicineDTO> MedicineInPharmacies(Guid medicineId);

        MedicineInPharmacyDTO ReadMedicine(Guid pharmacyId, Guid medicineId);

        void ChangeStockFor(Guid pharmacyId, Guid medicineId, int changeFor);

        IEnumerable<SmallPharmacyDTO> ReadBy(PharmacySearchParams searchParams);

        IEnumerable<PharmacyDTO> GetPharmaciesOfPharmacists(IList<Account> pharmacists, SearhSortParamsForAppointments searchParams);

        float GetPriceOfPharmacistConsultation(Guid pharmacyId);

        float GetPriceOfDermatologistExamination(Guid pharmacyId);

        Pharmacy UpdateGrade(Pharmacy pharmacy);

        void ReturnMedicinesInStock(Guid pharmacyId, Guid medicineId, int quantity);

        IEnumerable<SmallPharmacyDTO> ReadPagesToBy(PharmacySearchParams searchParams, PageDTO pageDTO);

        IEnumerable<PharmacyDTO> GetPharmaciesOfPharmacistsPagesTo(IList<Account> pharmacists, SearhSortParamsForAppointments searchParams, PageDTO pageDTO);
    }
}