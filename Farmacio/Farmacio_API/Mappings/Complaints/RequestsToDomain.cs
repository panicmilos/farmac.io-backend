using AutoMapper;
using Farmacio_API.Contracts.Requests.Complaints;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.Complaints
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateComplaintAboutDermatologistRequest, ComplaintAboutDermatologist>();
            CreateMap<CreateComplaintAboutPharmacistRequest, ComplaintAboutPharmacist>();
        }
    }
}