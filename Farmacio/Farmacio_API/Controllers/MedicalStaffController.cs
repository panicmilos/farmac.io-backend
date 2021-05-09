using System;
using AutoMapper;
using Farmacio_API.Contracts.Requests.AbsenceRequests;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Farmacio_API.Controllers
{
    [ApiController]
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
        /// Reads an existing absence requests in the pharmacy.
        /// </summary>
        /// <response code="200">Read absence requests.</response>
        [HttpGet("pharmacies/{pharmacyId}/absence-requests")]
        public IActionResult GetAbsenceRequestsForPharmacy(Guid pharmacyId)
        {
            return Ok(_absenceRequestService.ReadFor(pharmacyId));
        }

        /// <summary>
        /// Accepts absence request.
        /// </summary>
        /// <response code="200">Accepted absence request.</response>
        [HttpPost("medical-staff/absence-requests/{absenceRequestId}/accept")]
        public IActionResult CreateAbsenceRequest(Guid absenceRequestId)
        {
            return Ok(_absenceRequestService.AcceptAbsenceRequest(absenceRequestId));
        }
        
        /// <summary>
        /// Declines absence request.
        /// </summary>
        /// <response code="200">Declined absence request.</response>
        [HttpPost("medical-staff/absence-requests/{absenceRequestId}/decline")]
        public IActionResult CreateAbsenceRequest(Guid absenceRequestId, DeclineAbsenceRequestRequest declineAbsenceRequest)
        {
            return Ok(_absenceRequestService.DeclineAbsenceRequest(absenceRequestId, declineAbsenceRequest.Reason));
        }
        
        /// <summary>
        /// Creates absence request.
        /// </summary>
        /// <response code="200">Returns absence request(s).</response>
        /// <response code="400"></response>
        /// <response code="404">Requester not found.</response>
        [HttpPost("medical-staff/absence-requests")]
        public IActionResult CreateAbsenceRequest(CreateAbsenceRequestRequest request)
        {
            var absenceRequest = _mapper.Map<AbsenceRequestDTO>(request);
            return Ok(_absenceRequestService.CreateAbsenceRequest(absenceRequest));
        }
    }
}
