using AutoMapper;
using Farmacio_API.Contracts.Requests.ERecipes;
using Farmacio_Models.DTO;

namespace Farmacio_API.Mappings.ERecipes
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateERecipeMedicineRequest, MedicineQuantityDTO>();

            CreateMap<CreateReservationFromERecipeRequest, CreateReservationFromERecipeDTO>();
        }
    }
}