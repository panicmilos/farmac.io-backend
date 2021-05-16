using AutoMapper;
using Farmacio_API.Authorization;
using Farmacio_API.Contracts.Requests.Complaints;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [Route("complaints")]
    [ApiController]
    [Produces("application/json")]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintService<Complaint> _complaintService;
        private readonly IComplaintAboutDermatologistService _complaintAboutDermatologistService;
        private readonly IComplaintAboutPharmacistService _complaintAboutPharmacistService;
        private readonly IComplaintAboutPharmacyService _complaintAboutPharmacyService;
        private readonly IComplaintAnswerService _complaintAnswerService;
        private readonly IMapper _mapper;

        public ComplaintsController(
            IComplaintService<Complaint> complaintService,
            IComplaintAboutDermatologistService complaintAboutDermatologistService,
            IComplaintAboutPharmacistService complaintAboutPharmacistService,
            IComplaintAboutPharmacyService complaintAboutPharmacyService,
            IComplaintAnswerService complaintAnswerService,
            IMapper mapper
            )
        {
            _complaintService = complaintService;
            _complaintAboutDermatologistService = complaintAboutDermatologistService;
            _complaintAboutPharmacistService = complaintAboutPharmacistService;
            _complaintAboutPharmacyService = complaintAboutPharmacyService;
            _complaintAnswerService = complaintAnswerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns all existing complaints from the system.
        /// </summary>
        /// <response code="200">Returns list of complaints.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet]
        public IActionResult ReadComplaints()
        {
            return Ok(_complaintService.Read());
        }

        /// <summary>
        /// Returns existing complaint from the system specified by id.
        /// </summary>
        /// <response code="200">Return complaint.</response>
        [Authorize(Roles = "Patient, SystemAdmin")]
        [HttpGet("{id}")]
        public IActionResult ReadComplaint(Guid id)
        {
            var complaint = _complaintService.TryToRead(id);

            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(complaint.WriterId))
                                .Or(AllDataAllowed.For(Role.SystemAdmin))
                                .Authorize();

            return Ok(complaint);
        }

        /// <summary>
        /// Returns all existing complaints from the system writen by given writerId.
        /// </summary>
        /// <response code="200">Returns list of complaints.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("by/{writerId}")]
        public IActionResult ReadComplaintsBy(Guid writerId)
        {
            AuthorizationRuleSet.For(HttpContext)
                    .Rule(UserSpecific.For(writerId))
                    .Authorize();

            return Ok(_complaintService.ReadBy(writerId));
        }

        /// <summary>
        /// Returns all dermatologists from the system that given patient can complaint about.
        /// </summary>
        /// <response code="200">Returns list of dermatologists.</response>
        /// <response code="404">Given patient doesn't exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("{patientId}/complaintable/dermatologists")]
        public IActionResult ReadDermatologistsThatPatientCanComplaintAbout(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(patientId))
                                .Authorize();

            return Ok(_complaintAboutDermatologistService.ReadThatPatientCanComplaintAbout(patientId));
        }

        /// <summary>
        /// Creates a new complaint about dermatologist in the system.
        /// </summary>
        /// <response code="200">Returns created complaint.</response>
        /// <response code="400">Given patient didn't have an appointment with given dermatologist in the past.</response>
        /// <response code="404">Given patient doesn't exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpPost("about-dermatologist")]
        public IActionResult CreateComplaintAboutDermatologist(CreateComplaintAboutDermatologistRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(request.WriterId))
                                .Authorize();

            var complaint = _mapper.Map<ComplaintAboutDermatologist>(request);
            var createdComplaint = _complaintAboutDermatologistService.Create(complaint);

            return Ok(createdComplaint);
        }

        /// <summary>
        /// Returns all pharmacists from the system that given patient can complaint about.
        /// </summary>
        /// <response code="200">Returns list of pharmacists.</response>
        /// <response code="404">Given patient doesn't exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("{patientId}/complaintable/pharmacists")]
        public IActionResult ReadPharmacistsThatPatientCanComplaintAbout(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(patientId))
                                .Authorize();

            return Ok(_complaintAboutPharmacistService.ReadThatPatientCanComplaintAbout(patientId));
        }

        /// <summary>
        /// Creates a new complaint about pharmacist in the system.
        /// </summary>
        /// <response code="200">Returns created complaint.</response>
        /// <response code="400">Given patient didn't have an appointment with given pharmacist in the past.</response>
        /// <response code="404">Given patient doesn't exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpPost("about-pharmacists")]
        public IActionResult CreateComplaintAboutPharmacists(CreateComplaintAboutPharmacistRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(request.WriterId))
                                .Authorize();

            var complaint = _mapper.Map<ComplaintAboutPharmacist>(request);
            var createdComplaint = _complaintAboutPharmacistService.Create(complaint);

            return Ok(createdComplaint);
        }

        /// <summary>
        /// Returns all pharmacies from the system that given patient can complaint about.
        /// </summary>
        /// <response code="200">Returns list of pharmacies.</response>
        /// <response code="404">Given patient doesn't exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpGet("{patientId}/complaintable/pharmacies")]
        public IActionResult ReadPharmaciesThatPatientCanComplaintAbout(Guid patientId)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(patientId))
                                .Authorize();

            return Ok(_complaintAboutPharmacyService.ReadThatPatientCanComplaintAbout(patientId));
        }

        /// <summary>
        /// Creates a new complaint about pharmacy in the system.
        /// </summary>
        /// <response code="200">Returns created pharmacy.</response>
        /// <response code="400">Given patient didn't consume services of with given pharmacy in the past.</response>
        /// <response code="404">Given patient doesn't exist in the system.</response>
        [Authorize(Roles = "Patient")]
        [HttpPost("about-pharmacies")]
        public IActionResult CreateComplaintAboutPharmacy(CreateComplaintAboutPharmacyRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(request.WriterId))
                                .Authorize();

            var complaint = _mapper.Map<ComplaintAboutPharmacy>(request);
            var createdComplaint = _complaintAboutPharmacyService.Create(complaint);

            return Ok(createdComplaint);
        }

        /// <summary>
        /// Returns all existing answers from the system writen by given writerId.
        /// </summary>
        /// <response code="200">Returns list of answers.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpGet("answered-by/{writerId}")]
        public IActionResult ReadAnswersBy(Guid writerId)
        {
            return Ok(_complaintAnswerService.ReadBy(writerId));
        }

        /// <summary>
        /// Creates a new answer for complaint in the system.
        /// </summary>
        /// <response code="200">Returns created answer.</response>
        /// <response code="400">Given system admin has already answered to given complaint.</response>
        /// <response code="404">Given system admin or complaint doesn't exist in the system.</response>
        [Authorize(Roles = "SystemAdmin")]
        [HttpPost("{complaintId}/answers")]
        public IActionResult CreateAnswerToComplaint(CreateComplaintAnswerRequest request)
        {
            AuthorizationRuleSet.For(HttpContext)
                                .Rule(UserSpecific.For(request.WriterId))
                                .Authorize();

            var answer = _mapper.Map<ComplaintAnswer>(request);
            var createdAnswer = _complaintAnswerService.Create(answer);

            return Ok(createdAnswer);
        }
    }
}