using AutoMapper;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_API.Contracts.Requests.PharmacyOrders;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.PharmacyOrders
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<UpdatePharmacyOrderRequest, PharmacyOrder>();
            CreateMap<CreatePharmacyOrderRequest, PharmacyOrder>();
            CreateMap<CreatePharmacyMedicineRequest, OrderedMedicine>();
        }
    }
}