using System;
using System.Collections.Generic;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface IAppointmentService : ICrudService<Appointment>
    {
        IEnumerable<Appointment> ReadForPharmacy(Guid pharmacyId);
        IEnumerable<Appointment> ReadForDermatologistsInPharmacy(Guid pharmacyId);

        IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId);

        IEnumerable<Appointment> ReadReservedButUnreportedForMedicalStaff(Guid medicalStaffId);

        Appointment CreateDermatologistAppointment(CreateAppointmentDTO appointment);

        Appointment CreatePharmacistAppointment(CreateAppointmentDTO appointment);

        Appointment MakeAppointmentWithDermatologist(MakeAppointmentWithDermatologistDTO appointment);

        IEnumerable<Appointment> SortAppointments(IEnumerable<Appointment> appointments, string criteria, bool isAsc);

        IEnumerable<Appointment> ReadForPatients(Guid patientId);

        Appointment CancelAppointmentWithDermatologist(Guid appointmentId);

        IEnumerable<Appointment> ReadPatientsHistoryOfVisitsToDermatologist(Guid patientId);

        Report CreateReport(CreateReportDTO reportDTO);

        Report NotePatientDidNotShowUp(CreateReportDTO reportDTO);

        IEnumerable<Appointment> ReadFuturePharmacistsAppointmentsFor(Guid patientId);

        IEnumerable<Account> ReadPharmacistsForAppointment(IEnumerable<Account> pharmacists, SearhSortParamsForAppointments searchParams);

        Appointment CancelAppointmentWithPharmacist(Guid appointmentId);

        bool DidPatientHaveAppointmentWithMedicalStaff(Guid patientId, Guid medicalStaffId);

        Appointment CreateAnotherAppointmentByMedicalStaff(CreateAppointmentDTO appointment);
        
        IEnumerable<AppointmentAsEvent> ReadAppointmentsForCalendar(Guid medicalStaffId);
        
        IEnumerable<Appointment> ReadFor(Guid patientId);
        IEnumerable<Appointment> ReadPatientsHistoryOfVisitingPharmacists(Guid patientId);
    }
}