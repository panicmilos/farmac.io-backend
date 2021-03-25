using AutoMapper;
using Farmacio_API.Contracts.Requests.Pharmacies;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.Pharmacies
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreatePharmacyRequest, Pharmacy>();
        }
    }
}