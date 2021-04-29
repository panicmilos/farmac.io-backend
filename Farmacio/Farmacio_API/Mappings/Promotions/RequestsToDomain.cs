using AutoMapper;
using Farmacio_API.Contracts.Requests.Promotions;
using Farmacio_Models.Domain;

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