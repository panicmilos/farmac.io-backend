using AutoMapper;
using Farmacio_API.Contracts.Requests.Medicines;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyPriceLists;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;

namespace Farmacio_API.Mappings.PharmacyPriceLists
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreatePharmacyPriceListsRequest, PharmacyPriceList>();
            CreateMap<UpdatePharmacyPriceListRequest, PharmacyPriceList>();
            CreateMap<CreateMedicinePriceRequest, MedicinePrice>();
        }
    }
}