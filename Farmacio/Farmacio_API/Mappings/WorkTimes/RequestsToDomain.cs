using AutoMapper;
using Farmacio_API.Contracts.Requests.Addresses;
using Farmacio_API.Contracts.Requests.WorkTimes;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.WorkTimes
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<WorkTimeRequest, WorkTime>();
        }
    }
}