using AutoMapper;
using Farmacio_API.Contracts.Requests.LoyaltyPoints;
using Domain = Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.LoyaltyPoints
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<UpdateLoyaltyPointsRequest, Domain.LoyaltyPoints>();
            CreateMap<UpdateMedicinePointsRequest, Domain.MedicinePoints>();
        }
    }
}