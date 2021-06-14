using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Services.Implementation.Utils;

namespace Farmacio_Services.Implementation
{
    public class AbsenceRequestService : CrudService<AbsenceRequest>, IAbsenceRequestService
    {
        private readonly IAccountService _accountService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;
        private readonly IPatientService _patientService;
        private readonly IMedicalStaffService _medicalStaffService;

        public AbsenceRequestService(IRepository<AbsenceRequest> repository
            , IAccountService accountService, IAppointmentService appointmentService
            , ITemplatesProvider templatesProvider, IEmailDispatcher emailDispatcher
            , IPatientService patientService, IMedicalStaffService medicalStaffService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService) : base(repository)
        {
            _accountService = accountService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _appointmentService = appointmentService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
            _patientService = patientService;
            _medicalStaffService = medicalStaffService;
        }

        public IEnumerable<AbsenceRequest> ReadFor(Guid pharmacyId)
        {
            return Read().Where(absenceRequest => absenceRequest.PharmacyId == pharmacyId).ToList();
        }

        public IEnumerable<AbsenceRequest> ReadPageFor(Guid pharmacyId, PageDTO pageDto)
        {
            return PaginationUtils<AbsenceRequest>.Page(ReadFor(pharmacyId), pageDto);
        }

        public bool IsMedicalStaffAbsent(Guid medicalStaffId, DateTime date)
        {
            return Read().Any(absenceRequest => absenceRequest.RequesterId == medicalStaffId
                                                && absenceRequest.Status == AbsenceRequestStatus.Accepted
                                                && date >= absenceRequest.FromDate
                                                && date <= absenceRequest.ToDate);
        }

        public AbsenceRequest AcceptAbsenceRequest(Guid absenceRequestId)
        {
            var absenceRequest = TryToRead(absenceRequestId);
            if (absenceRequest.Status != AbsenceRequestStatus.WaitingForAnswer)
                throw new BadLogicException("Absence request has already been handled.");

            _appointmentService.ReadForMedicalStaffInPharmacy(absenceRequest.RequesterId, absenceRequest.PharmacyId)
                .ToList()
                .ForEach(appointment =>
                {
                    if (appointment.DateTime.Date < absenceRequest.FromDate.Date || appointment.DateTime.Date > absenceRequest.ToDate.Date)
                        return;

                    _appointmentService.Delete(appointment.Id);

                    if (!appointment.IsReserved || appointment.PatientId == null) return;

                    var patientAccount = _patientService.ReadByUserId(appointment.PatientId.Value);
                    var appointmentCanceledEmail = _templatesProvider.FromTemplate<Email>("AppointmentCanceled",
                        new
                        {
                            To = patientAccount.Email,
                            Name = patientAccount.User.FirstName,
                            Date = appointment.DateTime
                        });
                    _emailDispatcher.Dispatch(appointmentCanceledEmail);
                });

            var medicalStaffAccount = _medicalStaffService.ReadByUserId(absenceRequest.Requester.Id);
            var absenceRequestAcceptedEmail = _templatesProvider.FromTemplate<Email>("AbsenceRequestAccepted",
                new
                {
                    To = medicalStaffAccount.Email,
                    Name = medicalStaffAccount.User.FirstName,
                    absenceRequest.FromDate,
                    absenceRequest.ToDate,
                    PharmacyName = absenceRequest.Pharmacy.Name
                });
            _emailDispatcher.Dispatch(absenceRequestAcceptedEmail);

            absenceRequest.Status = AbsenceRequestStatus.Accepted;
            return base.Update(absenceRequest);
        }

        public AbsenceRequest DeclineAbsenceRequest(Guid absenceRequestId, string reason)
        {
            var absenceRequest = TryToRead(absenceRequestId);
            if (absenceRequest.Status != AbsenceRequestStatus.WaitingForAnswer)
                throw new BadLogicException("Absence request has already been handled.");

            var medicalStaffAccount = _medicalStaffService.ReadByUserId(absenceRequest.Requester.Id);
            var absenceRequestDeclinedEmail = _templatesProvider.FromTemplate<Email>("AbsenceRequestDeclined",
                new
                {
                    To = medicalStaffAccount.Email,
                    Name = medicalStaffAccount.User.FirstName,
                    absenceRequest.FromDate,
                    absenceRequest.ToDate,
                    Reason = reason,
                    PharmacyName = absenceRequest.Pharmacy.Name
                });
            _emailDispatcher.Dispatch(absenceRequestDeclinedEmail);

            absenceRequest.Status = AbsenceRequestStatus.Refused;
            absenceRequest.Answer = reason;
            return base.Update(absenceRequest);
        }

        public IEnumerable<AbsenceRequest> CreateAbsenceRequest(AbsenceRequestDTO absenceRequestDto)
        {
            var medicalAccount = _accountService.ReadByUserId(absenceRequestDto.RequesterId);
            if (medicalAccount == null)
                throw new MissingEntityException("The given requester does not exist in the system.");
            if (absenceRequestDto.FromDate < DateTime.Now)
                throw new BadLogicException("From date is in the past.");
            if (absenceRequestDto.FromDate > absenceRequestDto.ToDate)
                throw new BadLogicException("To date is before from date.");

            var absenceRequests = medicalAccount.Role == Role.Dermatologist
                ? _dermatologistWorkPlaceService
                    .GetWorkPlacesFor(medicalAccount.UserId)
                    .Select(workPlace =>
                        CreateAbsenceRequestInstanceFor(workPlace.PharmacyId, medicalAccount, absenceRequestDto))
                    .ToList()
                : new List<AbsenceRequest>
                {
                    CreateAbsenceRequestInstanceFor(((Pharmacist) medicalAccount.User).PharmacyId, medicalAccount,
                        absenceRequestDto)
                };
            absenceRequests.ForEach(absenceRequest => Create(absenceRequest));
            return absenceRequests;
        }

        private static AbsenceRequest CreateAbsenceRequestInstanceFor(Guid pharmacyId, Account medicalAccount,
            AbsenceRequestDTO absenceRequestDto) =>
            new AbsenceRequest
            {
                RequesterId = medicalAccount.UserId,
                FromDate = absenceRequestDto.FromDate,
                ToDate = absenceRequestDto.ToDate,
                Type = absenceRequestDto.Type,
                PharmacyId = pharmacyId,
                Status = AbsenceRequestStatus.WaitingForAnswer
            };

        public IEnumerable<WorkCalendarEvent> ReadAcceptedAbsencesForCalendar(Guid medicalStaffId)
        {
            return Read()
                .Where(absenceRequest => absenceRequest.RequesterId == medicalStaffId
                       && absenceRequest.Status == AbsenceRequestStatus.Accepted)
                .ToList().Select(absenceRequest => new WorkCalendarEvent
                {
                    Id = absenceRequest.Id,
                    Start = absenceRequest.FromDate,
                    End = absenceRequest.ToDate,
                    Title = absenceRequest.Type.ToString(),
                    PharmacyName = absenceRequest.Pharmacy.Name,
                    IsAppointment = false
                });
        }
    }
}