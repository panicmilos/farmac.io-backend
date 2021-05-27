using AutoMapper;
using Farmacio_API.Contracts.Requests.PharmacyPriceLists;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.PharmacyPriceLists
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreatePharmacyPriceListRequest, PharmacyPriceList>();
            CreateMap<UpdatePharmacyPriceListRequest, PharmacyPriceList>();
            CreateMap<CreateMedicinePriceRequest, MedicinePrice>();
        }
    }
}