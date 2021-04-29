using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ComplaintAboutPharmacyService : ComplaintService<ComplaintAboutPharmacy>, IComplaintAboutPharmacyService
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IReservationService _reservationService;

        public ComplaintAboutPharmacyService(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IReservationService reservationService,
            IRepository<ComplaintAboutPharmacy> repository
        ) :
            base(repository)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _reservationService = reservationService;
        }

        public IEnumerable<Pharmacy> ReadThatPatientCanComplaintAbout(Guid patientId)
        {
            var pharmaciesThatPatientComplaintAbout = new List<Pharmacy>();

            pharmaciesThatPatientComplaintAbout.AddRange(
                _reservationService.ReadFor(patientId)
                    .Where(reservation => reservation.State == ReservationState.Done)
                    .Select(reservation => reservation.Pharmacy));

            pharmaciesThatPatientComplaintAbout.AddRange(
                _appointmentService.ReadFor(patientId)
                    .Where(appointment => appointment.DateTime < DateTime.Now &&
                                          appointment.IsReserved)
                    .Select(appointment => appointment.Pharmacy));

            return pharmaciesThatPatientComplaintAbout.ToHashSet();
        }

        public override ComplaintAboutPharmacy Create(ComplaintAboutPharmacy complaint)
        {
            if (_patientService.ReadByUserId(complaint.WriterId) == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }
            if (!HasConsumedServicesOfPharmacyInPast(complaint.WriterId, complaint.PharmacyId))
            {
                throw new BadLogicException("Patient didn't consume services of given pharmacy in the past.");
            }

            return base.Create(complaint);
        }

        private bool HasConsumedServicesOfPharmacyInPast(Guid patientId, Guid pharmacyId)
        {
            var hasDoneReservation = _reservationService.ReadFor(patientId)
                .FirstOrDefault(reservation => reservation.State == ReservationState.Done &&
                                               reservation.PharmacyId == pharmacyId) != null;

            if (hasDoneReservation)
            {
                return true;
            }

            var hasConsultationOrExaminationInPast = _appointmentService.ReadFor(patientId)
                .FirstOrDefault(appointment => appointment.PharmacyId == pharmacyId &&
                                               appointment.DateTime < DateTime.Now &&
                                               appointment.IsReserved) != null;

            if (hasConsultationOrExaminationInPast)
            {
                return true;
            }

            return false;
        }
    }
}