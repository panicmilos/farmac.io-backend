﻿using System;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

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

        public override PharmacyPriceList Create(PharmacyPriceList pharmacyPriceList)
        {
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