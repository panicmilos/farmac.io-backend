using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class AbsenceRequestService : CrudService<AbsenceRequest>, IAbsenceRequestService
    {
        private readonly IAccountService _accountService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;

        public AbsenceRequestService(IRepository<AbsenceRequest> repository
            ,IAccountService accountService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService) : base(repository)
        {
            _accountService = accountService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
        }

        public IEnumerable<AbsenceRequest> ReadFor(Guid pharmacyId)
        {
            return Read().Where(absenceRequest => absenceRequest.PharmacyId == pharmacyId).ToList();
        }

        public IEnumerable<AbsenceRequest> CreateAbsenceRequest(AbsenceRequestDTO absenceRequestDto)
        {
            var medicalAccount = _accountService.ReadByUserId(absenceRequestDto.RequesterId);
            if (medicalAccount == null)
                throw new MissingEntityException("The given requester does not exist in the system.");

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
    }
}
