using AutoMapper;
using Farmacio_API.Contracts.Requests.PharmacyMedicines;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.PharmacyMedicines
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreatePharmacyMedicineRequest, PharmacyMedicine>();

        }
    }
}