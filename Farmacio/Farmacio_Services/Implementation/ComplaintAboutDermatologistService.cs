using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ComplaintAboutDermatologistService : ComplaintService<ComplaintAboutDermatologist>, IComplaintAboutDermatologistService
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDermatologistService _dermatologistService;

        public ComplaintAboutDermatologistService(
            IAppointmentService appointmentService,
            IDermatologistService dermatologistService,
            IRepository<ComplaintAboutDermatologist> repository
        ) :
            base(repository)
        {
            _appointmentService = appointmentService;
            _dermatologistService = dermatologistService;
        }

        public IEnumerable<Dermatologist> ReadThatPatientCanComplaintAbout(Guid patientId)
        {
            var dermatologistIds = _dermatologistService.Read().Select(dermatologist => dermatologist.UserId).ToHashSet();

            return _appointmentService.ReadFor(patientId)
                .Where(appointment => appointment.PatientId == patientId &&
                                      appointment.DateTime < DateTime.Now &&
                                      appointment.IsReserved &&
                                      dermatologistIds.Contains(appointment.MedicalStaffId))
                .Select(appointment => appointment.MedicalStaff as Dermatologist).ToHashSet();
        }

        public override ComplaintAboutDermatologist Create(ComplaintAboutDermatologist complaint)
        {
            if (!HadAppointmentWithDermatologistInPast(complaint.WriterId, complaint.DermatologistId))
            {
                throw new BadLogicException("Patient didn't have appointment with given dermatologist in the past.");
            }

            return base.Create(complaint);
        }

        private bool HadAppointmentWithDermatologistInPast(Guid patientId, Guid dermatologistId)
        {
            var dermatologistIds = _dermatologistService.Read().Select(dermatologist => dermatologist.UserId).ToHashSet();

            return _appointmentService.ReadFor(patientId)
                .FirstOrDefault(appointment => appointment.PatientId == patientId &&
                                               appointment.DateTime < DateTime.Now &&
                                               appointment.IsReserved &&
                                               appointment.MedicalStaffId == dermatologistId &&
                                               dermatologistIds.Contains(appointment.MedicalStaffId)) != null;
        }
    }
}