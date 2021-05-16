using System;
using System.Collections.Generic;
using System.Linq;
using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class PromotionService : CrudService<Promotion>, IPromotionService
    {
        private readonly IPatientFollowingsService _patientFollowingsService;
        private readonly IPharmacyService _pharmacyService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;
        
        public PromotionService(IPharmacyService pharmacyService, IEmailDispatcher emailDispatcher,
            ITemplatesProvider templatesProvider, IRepository<Promotion> repository
            , IPatientFollowingsService patientFollowingsService) : base(repository)
        {
            _pharmacyService = pharmacyService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
            _patientFollowingsService = patientFollowingsService;
        }

        public override Promotion Create(Promotion promotion)
        {
            ValidatePromotion(promotion);

            var createdPromotion = base.Create(promotion);
            
            _patientFollowingsService
                .ReadFollowersFor(promotion.PharmacyId)
                .ToList().ForEach(account =>
                {
                    var offerAcceptedEmail = _templatesProvider.FromTemplate<Email>("PromotionCreated",
                        new {To = account.Email, Name = account.User.FirstName});
                    _emailDispatcher.Dispatch(offerAcceptedEmail);
                });
            
            return createdPromotion;
        }

        public override Promotion Update(Promotion promotion)
        {
            ValidatePromotion(promotion, false);
            return base.Update(promotion);
        }

        public IEnumerable<Promotion> ReadFor(Guid pharmacyId)
        {
            return Read().Where(promotion => promotion.PharmacyId == pharmacyId).ToList();
        }

        public IEnumerable<Promotion> ReadPageFor(Guid pharmacyId, PageDTO pageDto)
        {
            return PaginationUtils<Promotion>.Page(ReadFor(pharmacyId), pageDto);
        }

        public IEnumerable<Promotion> ReadActiveFor(Guid pharmacyId)
        {
            return ReadFor(pharmacyId)
                .Where(promotion => DateTime.Now >= promotion.From && DateTime.Now <= promotion.To).ToList();
        }

        public int ReadDiscountFor(Guid pharmacyId)
        {
            return ReadActiveFor(pharmacyId).Sum(promotion => promotion.Discount);
        }

        private void ValidatePromotion(Promotion promotion, bool isCreate = true)
        {
            if (isCreate)
                _pharmacyService.TryToRead(promotion.PharmacyId);
            if (promotion.To <= promotion.From)
                throw new BadLogicException("Please provide valid time period.");
            if (DateTime.Now > promotion.From)
                throw new BadLogicException("Promotion must be in the future.");
            if (promotion.Discount <= 0 || promotion.Discount > 100)
                throw new BadLogicException("Discount is not valid.");
        }
    }
}