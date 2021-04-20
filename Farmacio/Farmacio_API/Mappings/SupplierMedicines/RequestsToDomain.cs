using AutoMapper;
using Farmacio_API.Contracts.Requests.SupplierMedicines;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.SupplierMedicines
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateSupplierMedicineRequest, SupplierMedicine>();
            CreateMap<UpdateSupplierMedicineRequest, SupplierMedicine>();
        }
    }
}