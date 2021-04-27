using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyService : ICrudService<Pharmacy>
    {
        IEnumerable<SmallPharmacyDTO> ReadForHomePage();

        IEnumerable<PharmaciesOfMedicineDTO> MedicineInPharmacies(Guid Id);

        MedicineInPharmacyDTO ReadMedicine(Guid pharmacyId, Guid medicineId);

        void ChangeStockFor(Guid pharmacyId, Guid medicineId, int changeFor);
        IEnumerable<SmallPharmacyDTO> ReadBy(PharmacySearchParams searchParams);
        IEnumerable<PharmacyDTO> GetPharmaciesOfPharmacists(IList<Account> pharmacists, SearhSortParamsForAppointments searchParams);
        float GetPriceOfPharmacistConsultation(Guid pharmacyId);
        float GetPriceOfDermatologistExamination(Guid pharmacyId);
    }
}