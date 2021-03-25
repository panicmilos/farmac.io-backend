using AutoMapper;
using Farmacio_API.Contracts.Requests.Addresses;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.Addresses
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateAddressRequest, Address>();
            CreateMap<UpdateAddressRequest, Address>();
        }
    }
}