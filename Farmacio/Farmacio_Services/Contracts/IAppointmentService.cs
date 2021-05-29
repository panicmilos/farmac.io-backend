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
        IEnumerable<Appointment> ReadPageForDermatologistsInPharmacy(Guid pharmacyId, PageDTO pageDTO);

        IEnumerable<Appointment> ReadForMedicalStaff(Guid medicalStaffId);
        IEnumerable<Appointment> ReadForMedicalStaffForUpdate(Guid medicalStaffId);
        IEnumerable<Appointment> ReadForMedicalStaffInPharmacy(Guid medicalStaffId, Guid pharmacyId);
        IEnumerable<Appointment> ReadReservedButUnreportedForMedicalStaff(Guid medicalStaffId);
        IEnumerable<Appointment> ReadForReportPage(Guid medicalStaffId);

        IEnumerable<Appointment> ReadForPatient(Guid patientId);
        IEnumerable<Appointment> ReadForPatientForUpdate(Guid patientId);

        IEnumerable<AppointmentAsEvent> ReadAppointmentsForCalendar(Guid medicalStaffId);

        Appointment CreateDermatologistAppointment(CreateAppointmentDTO appointmentDTO);
        Appointment CreatePharmacistAppointment(CreateAppointmentDTO appointmentDTO);
        Appointment CreateAnotherAppointmentByMedicalStaff(CreateAppointmentDTO appointmentDTO);
        Appointment MakeAppointmentWithDermatologist(MakeAppointmentWithDermatologistDTO appointmentRequest);

        Appointment CancelAppointmentWithDermatologist(Guid appointmentId);
        Appointment CancelAppointmentWithPharmacist(Guid appointmentId);

        IEnumerable<Appointment> ReadFutureExaminationAppointmentsFor(Guid patientId);
        IEnumerable<Appointment> ReadFutureConsultationAppointmentsFor(Guid patientId);
        IEnumerable<Appointment> ReadPatientsHistoryOfVisitingDermatologists(Guid patientId);
        IEnumerable<Appointment> ReadPatientsHistoryOfVisitingPharmacists(Guid patientId);
        IEnumerable<Appointment> ReadPagesOfPatientHistoryVisitingPharmacists(Guid patientId, PageDTO pageDTO);
        IEnumerable<Appointment> ReadPageOfPatientHistoryVisitingDermatologists(Guid patientId, PageDTO pageDTO);

        Report CreateReport(CreateReportDTO reportDTO);
        Report NotePatientDidNotShowUp(CreateReportDTO reportDTO);

        IEnumerable<Account> ReadPharmacistsForAppointment(IEnumerable<Account> pharmacists, SearhSortParamsForAppointments searchParams);

        bool DidPatientHaveAppointmentWithMedicalStaff(Guid patientId, Guid medicalStaffId);
        bool DidPatientShowUpForAppointment(Guid appointmentId);

        IEnumerable<Appointment> SortAppointments(IEnumerable<Appointment> appointments, string criteria, bool isAsc);
        IEnumerable<Appointment> SortAppointmentsPageTo(IEnumerable<Appointment> appointments, string criteria, bool isAsc, PageDTO pageDTO);

    }
}