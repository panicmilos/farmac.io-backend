using AutoMapper;
using Farmacio_API.Contracts.Requests.PharmacyReports;
using Farmacio_Models.DTO;

namespace Farmacio_API.Mappings.PharmacyReports
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<TimePeriodRequest, TimePeriodDTO>();
        }
    }
}