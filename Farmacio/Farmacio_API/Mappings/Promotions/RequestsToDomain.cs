using AutoMapper;
using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_API.Contracts.Requests.Promotions;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;

namespace Farmacio_API.Mappings.Promotions
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<UpdatePromotionRequest, Promotion>();
            CreateMap<CreatePromotionRequest, Promotion>();
        }
    }
}