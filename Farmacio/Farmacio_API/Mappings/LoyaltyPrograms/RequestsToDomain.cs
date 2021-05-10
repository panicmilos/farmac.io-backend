using AutoMapper;
using Farmacio_API.Contracts.Requests.LoyaltyPrograms;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.LoyaltyPrograms
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateLoyaltyProgramRequest, LoyaltyProgram>();
            CreateMap<UpdateLoyaltyProgramRequest, LoyaltyProgram>();
        }
    }
}