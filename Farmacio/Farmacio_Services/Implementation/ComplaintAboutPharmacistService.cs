using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ComplaintAboutPharmacistService : ComplaintService<ComplaintAboutPharmacist>, IComplaintAboutPharmacistService
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPharmacistService _pharmacistService;

        public ComplaintAboutPharmacistService(
            IAppointmentService appointmentService,
            IPharmacistService pharmacistService,
            IRepository<ComplaintAboutPharmacist> repository
        ) :
            base(repository)
        {
            _appointmentService = appointmentService;
            _pharmacistService = pharmacistService;
        }

        public IEnumerable<Pharmacist> ReadThatPatientCanComplaintAbout(Guid patientId)
        {
            var pharmacistsIds = _pharmacistService.Read().Select(pharmacist => pharmacist.UserId).ToHashSet();

            return _appointmentService.ReadForPatient(patientId)
                .Where(appointment => appointment.DateTime < DateTime.Now &&
                                      appointment.IsReserved &&
                                      pharmacistsIds.Contains(appointment.MedicalStaffId) && _appointmentService.DidPatientShowUpForAppointment(appointment.Id))
                .Select(appointment => appointment.MedicalStaff as Pharmacist).ToHashSet();
        }

        public override ComplaintAboutPharmacist Create(ComplaintAboutPharmacist complaint)
        {
            if (!HadAppointmentWithPharmacistInPast(complaint.WriterId, complaint.PharmacistId))
            {
                throw new BadLogicException("Patient didn't have appointment with given pharmacist in the past.");
            }

            return base.Create(complaint);
        }

        private bool HadAppointmentWithPharmacistInPast(Guid patientId, Guid pharmacistId)
        {
            var pharmacistsIds = _pharmacistService.Read().Select(pharmacist => pharmacist.UserId).ToHashSet();

            return _appointmentService.ReadForPatient(patientId)
                .FirstOrDefault(appointment => appointment.DateTime < DateTime.Now &&
                                               appointment.IsReserved &&
                                               appointment.MedicalStaffId == pharmacistId &&
                                               pharmacistsIds.Contains(appointment.MedicalStaffId) && 
                                               _appointmentService.DidPatientShowUpForAppointment(appointment.Id)) != null;
        }
    }
}