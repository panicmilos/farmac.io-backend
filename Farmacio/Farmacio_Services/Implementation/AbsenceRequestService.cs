using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;

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

        public IEnumerable<AbsenceRequest> CreateAbsenceRequest(AbsenceRequestDTO absenceRequestDTO)
        {
            var medicalAccount = _accountService.ReadByUserId(absenceRequestDTO.RequesterId);
            if (medicalAccount == null)
                throw new MissingEntityException("The given requester does not exist in the system.");

            List<AbsenceRequest> absenceRequests = new List<AbsenceRequest>();
            if (medicalAccount.Role == Role.Dermatologist)
            {
                var workPlaces = _dermatologistWorkPlaceService.GetWorkPlacesFor(medicalAccount.UserId);
                foreach (var wp in workPlaces)
                    absenceRequests.Add(new AbsenceRequest
                    {
                        RequesterId = medicalAccount.UserId,
                        FromDate = absenceRequestDTO.FromDate,
                        ToDate = absenceRequestDTO.ToDate,
                        Type = absenceRequestDTO.Type,
                        PharmacyId = wp.PharmacyId,
                        Status = AbsenceRequestStatus.WaitingForAnswer
                    });
            }
            else if (medicalAccount.Role == Role.Pharmacist)
            {
                var pharmacist = (Pharmacist)medicalAccount.User;
                absenceRequests.Add(new AbsenceRequest
                {
                    RequesterId = medicalAccount.UserId,
                    FromDate = absenceRequestDTO.FromDate,
                    ToDate = absenceRequestDTO.ToDate,
                    Type = absenceRequestDTO.Type,
                    PharmacyId = pharmacist.PharmacyId,
                    Status = AbsenceRequestStatus.WaitingForAnswer
                });
            }
            return absenceRequests;
        }
    }
}
