using System;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class DiscountService : IDiscountService
    {
        private readonly ILoyaltyProgramService _loyaltyProgramService;
        private readonly IPromotionService _promotionService;
        
        public DiscountService(ILoyaltyProgramService loyaltyProgramService, IPromotionService promotionService)
        {
            _loyaltyProgramService = loyaltyProgramService;
            _promotionService = promotionService;
        }
        
        public int ReadDiscountFor(Guid pharmacyId, Guid patientId)
        {
            return _loyaltyProgramService.ReadDiscountFor(patientId) + _promotionService.ReadDiscountFor(pharmacyId);
        }
    }
}