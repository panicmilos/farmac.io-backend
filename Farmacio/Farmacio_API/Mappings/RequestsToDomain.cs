using AutoMapper;
using Farmacio_API.Contracts.Requests;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateTestEntityRequest, TestEntity>();
        }
    }
}