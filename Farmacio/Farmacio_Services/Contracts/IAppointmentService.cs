using Farmacio_Models.Domain;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface IAppointmentService : ICrudService<Appointment>
    {
        Appointment CreateDermatologistAppointment(CreateAppointmentDTO appointment);
    }
}