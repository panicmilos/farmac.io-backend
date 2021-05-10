using AutoMapper;
using Farmacio_API.Contracts.Requests.AbsenceRequests;
using Farmacio_Models.DTO;

namespace Farmacio_API.Mappings.AbsenceRequests
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateAbsenceRequestRequest, AbsenceRequestDTO>();
        }
    }
}
