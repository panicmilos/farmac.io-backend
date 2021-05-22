using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class PharmacyReportsService : IPharmacyReportsService
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IReservationService _reservationService;

        public PharmacyReportsService(IAppointmentService appointmentService, IReservationService reservationService)
        {
            _appointmentService = appointmentService;
            _reservationService = reservationService;
        }

        public IEnumerable<PharmacyReportRecordDTO> GenerateExaminationsReportFor(Guid pharmacyId, TimePeriodDTO timePeriod)
        {
            var isPeriodLongerThanAMonth = timePeriod.From.Month != timePeriod.To.Month;
            var appointmentsForPharmacyInTimePeriod = AppointmentsForPharmacyInTimePeriod(pharmacyId, timePeriod, true);

            return appointmentsForPharmacyInTimePeriod.OrderBy(appointment => appointment.DateTime)
                .GroupBy(appointment => isPeriodLongerThanAMonth ? appointment.DateTime.Month : appointment.DateTime.Day)
                .Select(group => new PharmacyReportRecordDTO
                {
                    Group = group.Key.ToString(),
                    Value = group.Count(appointment => appointment.IsReserved)
                })
                .ToList();
        }

        public IEnumerable<PharmacyReportRecordDTO> GenerateMedicineConsumptionReportFor(Guid pharmacyId, TimePeriodDTO timePeriod)
        {
            var isPeriodLongerThanAMonth = timePeriod.From.Month != timePeriod.To.Month;
            var doneReservationsForPharmacyInTimePeriod = DoneReservationsForPharmacyInTimePeriod(pharmacyId, timePeriod);

            return doneReservationsForPharmacyInTimePeriod.OrderBy(reservation => reservation.CreatedAt)
                .GroupBy(reservation => isPeriodLongerThanAMonth ? reservation.CreatedAt.Month : reservation.CreatedAt.Day)
                .Select(group => new PharmacyReportRecordDTO
                {
                    Group = group.Key.ToString(),
                    Value = group.Sum(reservation => reservation.State == ReservationState.Done ?
                        reservation.Medicines.Sum(orderedMedicine => orderedMedicine.Quantity) : 0)
                })
                .ToList();
        }

        public IEnumerable<PharmacyReportRecordDTO> GeneratePharmacyIncomeReportFor(Guid pharmacyId, TimePeriodDTO timePeriod)
        {
            var isPeriodLongerThanAMonth = timePeriod.From.Month != timePeriod.To.Month;
            var appointmentsForPharmacyInTimePeriod = AppointmentsForPharmacyInTimePeriod(pharmacyId, timePeriod, false);
            var doneReservationsForPharmacyInTimePeriod = DoneReservationsForPharmacyInTimePeriod(pharmacyId, timePeriod);

            var groupedAppointmentsByDate = appointmentsForPharmacyInTimePeriod.OrderBy(appointment => appointment.DateTime)
                .GroupBy(appointment => isPeriodLongerThanAMonth ? appointment.DateTime.Month : appointment.DateTime.Day)
                .Select(group => new PharmacyReportRecordDTO
                {
                    Group = group.Key.ToString(),
                    Value = group.Sum(appointment => appointment.Price)
                })
                .ToList();

            var groupedReservationsByDate = doneReservationsForPharmacyInTimePeriod
                .OrderBy(reservation => reservation.CreatedAt)
                .GroupBy(reservation =>
                    isPeriodLongerThanAMonth ? reservation.CreatedAt.Month : reservation.CreatedAt.Day)
                .Select(group => new PharmacyReportRecordDTO
                {
                    Group = group.Key.ToString(),
                    Value = group.Sum(reservation =>
                        reservation.State == ReservationState.Done
                            ? reservation.Medicines.Sum(orderedMedicine =>
                                orderedMedicine.Price * orderedMedicine.Quantity)
                            : 0)
                })
                .ToList();
            return groupedAppointmentsByDate.Concat(groupedReservationsByDate)
                .GroupBy(pharmacyReportRecord => pharmacyReportRecord.Group)
                .Select(group => new PharmacyReportRecordDTO
                {
                    Group = group.Key,
                    Value = group.Sum(pharmacyReportRecord => pharmacyReportRecord.Value)
                })
                .ToList();
        }

        private IEnumerable<Appointment> AppointmentsForPharmacyInTimePeriod(Guid pharmacyId, TimePeriodDTO timePeriod
            , bool dermatologistsOnly)
        {
            var isPeriodLongerThanAMonth = timePeriod.From.Month != timePeriod.To.Month;
            var appointmentsForPharmacyInTimePeriod =
                (dermatologistsOnly ? _appointmentService.ReadForDermatologistsInPharmacy(pharmacyId)
                    : _appointmentService.ReadForPharmacy(pharmacyId))
                .Where(appointment => appointment.DateTime >= timePeriod.From && appointment.DateTime <= timePeriod.To && appointment?.Report?.Notes != "Patient did not show up.")
                .ToList();

            // Fill empty days or months
            (isPeriodLongerThanAMonth ? EachMonth(timePeriod) : EachDay(timePeriod)).ToList()
                .ForEach(dateTime =>
                {
                    if (appointmentsForPharmacyInTimePeriod.FirstOrDefault(appointment =>
                        isPeriodLongerThanAMonth
                            ? appointment.DateTime.Month == dateTime.Month
                            : appointment.DateTime.Day == dateTime.Day) == null)
                        appointmentsForPharmacyInTimePeriod.Add(new Appointment
                        {
                            IsReserved = false,
                            DateTime = dateTime
                        });
                });
            return appointmentsForPharmacyInTimePeriod;
        }

        private IEnumerable<Reservation> DoneReservationsForPharmacyInTimePeriod(Guid pharmacyId, TimePeriodDTO timePeriod)
        {
            var isPeriodLongerThanAMonth = timePeriod.From.Month != timePeriod.To.Month;
            var doneReservationsForPharmacyInTimePeriod = _reservationService.ReadFrom(pharmacyId)
                .Where(reservation =>
                    reservation.CreatedAt >= timePeriod.From && reservation.CreatedAt <= timePeriod.To)
                .ToList();

            // Fill empty days or months
            (isPeriodLongerThanAMonth ? EachMonth(timePeriod) : EachDay(timePeriod)).ToList()
                .ForEach(dateTime =>
                {
                    if (doneReservationsForPharmacyInTimePeriod.FirstOrDefault(reservation =>
                        isPeriodLongerThanAMonth
                            ? reservation.CreatedAt.Month == dateTime.Month
                            : reservation.CreatedAt.Day == dateTime.Day) == null)
                        doneReservationsForPharmacyInTimePeriod.Add(new Reservation
                        {
                            State = ReservationState.Reserved,
                            CreatedAt = dateTime
                        });
                });
            return doneReservationsForPharmacyInTimePeriod;
        }

        private static IEnumerable<DateTime> EachDay(TimePeriodDTO timePeriod)
        {
            for (var day = timePeriod.From.Date; day.Date <= timePeriod.To.Date; day = day.AddDays(1))
                yield return day;
        }

        private static IEnumerable<DateTime> EachMonth(TimePeriodDTO timePeriod)
        {
            for (var month = timePeriod.From.Date; month.Date <= timePeriod.To.Date; month = month.AddMonths(1))
                yield return month;
        }
    }
}