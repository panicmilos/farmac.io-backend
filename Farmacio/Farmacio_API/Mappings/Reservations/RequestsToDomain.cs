using AutoMapper;
using Farmacio_API.Contracts.Requests.Reservations;
using Farmacio_Models.Domain;

namespace Farmacio_API.Mappings.Reservations
{
    public class RequestsToDomain : Profile
    {
        public RequestsToDomain()
        {
            CreateMap<CreateReservedMedicineRequest, ReservedMedicine>();
            CreateMap<CreateReservationRequest, Reservation>();
        }
    }
}