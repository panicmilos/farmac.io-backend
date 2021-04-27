using AutoMapper;
using Farmacio_API.Contracts.Requests.Complaints;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [Route("complaints")]
    [ApiController]
    [Produces("application/json")]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintAboutDermatologistService _complaintAboutDermatologistService;
        private readonly IMapper _mapper;

        public ComplaintsController(
            IComplaintAboutDermatologistService complaintAboutDermatologistService,
            IMapper mapper
            )
        {
            _complaintAboutDermatologistService = complaintAboutDermatologistService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all dermatologists from the system that given patient can complaint about.
        /// </summary>
        /// <response code="200">Returns list of dermatologists.</response>
        /// <response code="404">Given patient doesn't exist in the system.</response>
        [HttpGet("{patientId}/complaintable/dermatologists")]
        public IActionResult ReadDermatologistsThatPatientCanComplaintAbout(Guid patientId)
        {
            return Ok(_complaintAboutDermatologistService.ReadThatPatientCanComplaintAbout(patientId));
        }

        /// <summary>
        /// Creates a new complaint about dermatologist in the system.
        /// </summary>
        /// <response code="200">Returns created complaint.</response>
        /// <response code="400">Given patient didn't have appointment with given dermatologist in the past.</response>
        /// <response code="404">Given patient doesn't exist in the system.</response>
        [HttpPost("complaints/about-dermatologist")]
        public IActionResult CreateComplaintAboutDermatologist(CreateComplaintAboutDermatologistRequest request)
        {
            var complaint = _mapper.Map<ComplaintAboutDermatologist>(request);
            var createdComplaint = _complaintAboutDermatologistService.Create(complaint);

            return Ok(createdComplaint);
        }
    }
}