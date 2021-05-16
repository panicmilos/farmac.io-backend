using System;
using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.AbsenceRequests;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("medical-staff")]
    [Produces("application/json")]
    public class MedicalStaffController : ControllerBase
    {
        private readonly IMedicalStaffService _medicalStaffService;
        private readonly IAbsenceRequestService _absenceRequestService;
        private readonly IMapper _mapper;

        public MedicalStaffController(IMedicalStaffService medicalStaffService
            , IAbsenceRequestService absenceRequestService
            , IMapper mapper)
        {
            _medicalStaffService = medicalStaffService;
            _absenceRequestService = absenceRequestService;
            _mapper = mapper;
        }

        /// <summary>
        /// Accepts absence request.
        /// </summary>
        /// <response code="200">Accepted absence request.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpPost("absence-requests/{absenceRequestId}/accept")]
        public IActionResult CreateAbsenceResponse(Guid absenceRequestId)
        {
            var absenceRequest = _absenceRequestService.Read(absenceRequestId);
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(absenceRequest.PharmacyId))
                .Authorize();
            return Ok(_absenceRequestService.AcceptAbsenceRequest(absenceRequestId));
        }

        /// <summary>
        /// Declines absence request.
        /// </summary>
        /// <response code="200">Declined absence request.</response>
        [Authorize(Roles = "PharmacyAdmin")]
        [HttpPost("absence-requests/{absenceRequestId}/decline")]
        public IActionResult CreateAbsenceResponse(Guid absenceRequestId, DeclineAbsenceRequestRequest declineAbsenceRequest)
        {
            var absenceRequest = _absenceRequestService.Read(absenceRequestId);
            AuthorizationRuleSet.For(HttpContext)
                .Rule(IsPharmacyAdmin.Of(absenceRequest.PharmacyId))
                .Authorize();
            return Ok(_absenceRequestService.DeclineAbsenceRequest(absenceRequestId, declineAbsenceRequest.Reason));
        }

        /// <summary>
        /// Creates absence request.
        /// </summary>
        /// <response code="200">Returns absence request(s).</response>
        /// <response code="400"></response>
        /// <response code="404">Requester not found.</response>
        [Authorize(Roles = "Dermatologist, Pharmacist")]
        [HttpPost("absence-requests")]
        public IActionResult CreateAbsenceRequest(CreateAbsenceRequestRequest request)
        {
            var absenceRequest = _mapper.Map<AbsenceRequestDTO>(request);
            return Ok(_absenceRequestService.CreateAbsenceRequest(absenceRequest));
        }
    }
}