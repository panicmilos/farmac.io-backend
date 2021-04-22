using System;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class PharmacyPriceListService : CrudService<PharmacyPriceList>, IPharmacyPriceListService
    {
        public PharmacyPriceListService(IRepository<PharmacyPriceList> repository) 
            : base(repository)
        {
        }

        public PharmacyPriceList ReadForPharmacy(Guid pharmacyId)
        {
            return Read().ToList().FirstOrDefault(pl => pl.PharmacyId == pharmacyId);
        }
        
        public PharmacyPriceList ReadWithMostRecentPricesFor(Guid pharmacyId)
        {
            var priceList = Read().ToList().FirstOrDefault(pl => pl.PharmacyId == pharmacyId);
            if (priceList == null)
                return null;
            var allMedicinePrices = priceList.MedicinePriceList.ToList(); 
            priceList.MedicinePriceList = allMedicinePrices.Where(medicinePrice =>
            {
                return medicinePrice.ActiveFrom == allMedicinePrices
                    .Where(mp => mp.MedicineId == medicinePrice.MedicineId).Max(mp => mp.ActiveFrom);
            }).ToList();
            return priceList;
        }

        public PharmacyPriceList TryToReadFor(Guid pharmacyId)
        {
            var pharmacyPriceList = ReadForPharmacy(pharmacyId);
            if (pharmacyPriceList == null)
                throw new MissingEntityException("Pharmacy price list for pharmacy not found.");
            return pharmacyPriceList;
        }
        
        public PharmacyPriceList TryToReadWithMostRecentPricesFor(Guid pharmacyId)
        {
            var pharmacyPriceList = ReadWithMostRecentPricesFor(pharmacyId);
            if (pharmacyPriceList == null)
                throw new MissingEntityException("Pharmacy price list for pharmacy not found.");
            return pharmacyPriceList;
        }

        public override PharmacyPriceList Create(PharmacyPriceList pharmacyPriceList)
        {
            if(ReadForPharmacy(pharmacyPriceList.PharmacyId) != null)
                throw new BadLogicException("Pharmacy price list for the given pharmacy already exists.");
            pharmacyPriceList.MedicinePriceList.ForEach(medicinePrice =>
            {
                medicinePrice.ActiveFrom = DateTime.Now;
            });
            
            return base.Create(pharmacyPriceList);
        }

        public override PharmacyPriceList Update(PharmacyPriceList pharmacyPriceList)
        {
            var existingPharmacyPriceList = TryToRead(pharmacyPriceList.Id);
            pharmacyPriceList.MedicinePriceList.ForEach(medicinePrice =>
            {
                medicinePrice.ActiveFrom = DateTime.Now;
            });

            existingPharmacyPriceList.ConsultationPrice = pharmacyPriceList.ConsultationPrice;
            existingPharmacyPriceList.ExaminationPrice = pharmacyPriceList.ExaminationPrice;
            existingPharmacyPriceList.MedicinePriceList.AddRange(pharmacyPriceList.MedicinePriceList);
            
            return base.Update(existingPharmacyPriceList);
        }
    }
}