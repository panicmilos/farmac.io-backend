using AutoMapper;
using Farmacio_API.Contracts.Requests;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.SupplierOffers
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateSupplierOfferRequest, SupplierOffer>()
                .ForMember(dst => dst.Status, opts => opts.MapFrom(src => OfferStatus.WaitingForAnswer));
        }
    }
}