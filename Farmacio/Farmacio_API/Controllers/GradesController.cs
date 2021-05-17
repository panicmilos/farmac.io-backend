using AutoMapper;
using Farmacio_API.Contracts.Requests.Grades;
using Farmacio_API.Contracts.Responses.Grades;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Farmacio_API.Controllers
{
    [Route("grades")]
    [ApiController]
    [Produces("application/json")]
    public class GradesController : ControllerBase
    {
        private readonly IMedicalStaffGradeService _medicalStaffGradeService;
        private readonly IMedicalStaffService _medicalStaffService;
        private readonly IMedicineGradeService _medicineGradeService;
        private readonly IPharmacyGradeService _pharmacyGradeService;
        private readonly IMapper _mapper;

        public GradesController(
            IMedicalStaffGradeService medicalStaffGradeService,
            IMedicalStaffService medicalStaffService,
            IMedicineGradeService medicineGradeService,
            IPharmacyGradeService pharmacyGradeService,
            IMapper mapper
            )
        {
            _medicalStaffGradeService = medicalStaffGradeService;
            _medicalStaffService = medicalStaffService;
            _medicineGradeService = medicineGradeService;
            _pharmacyGradeService = pharmacyGradeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Rate the medical staff.
        /// </summary>
        /// <response code="200">Returns grade.</response>
        /// <response code="400">Patient cannot rate the medical staff or already has rated.</response>
        /// <response code="404">Given patient or medical staff does not exist in the system.</response>
        [HttpPost("rate")]
        public IActionResult RateTheMedicalStaff(CreateMedicalStaffGradeRequest request)
        {
            var medicalStaffGrade = _mapper.Map<MedicalStaffGrade>(request);
            return Ok(_medicalStaffGradeService.GradeMedicalStaff(medicalStaffGrade));
        }

        /// <summary>
        /// Reads an existing dermatologist in the system that patietn can rate.
        /// </summary>
        /// <response code="200">Read dermatologists.</response>
        [HttpGet("{patientId}/can-rate-dermatologists")]
        public IActionResult GetDermatologistToRate(Guid patientId)
        {
            return Ok(_medicalStaffGradeService.ReadDermatologistThatPatientCanRate(patientId));
        }

        /// <summary>
        /// Reads an existing dermatologists in the system that patient rated.
        /// </summary>
        /// <response code="200">Read dermatologist.</response>
        [HttpGet("{patientId}/rated-dermatologists")]
        public IActionResult GetRatedDermatologist(Guid patientId)
        {
            return Ok(_medicalStaffGradeService.ReadDermatologistThatPatientRated(patientId).Select(dermatologist =>
            {
                var grade = _medicalStaffGradeService.Read(patientId, dermatologist.UserId);
                return new MedicalStafftWithGradeResponse
                {
                    MedicalStaff = dermatologist.User as MedicalStaff,
                    Grade = grade.Value,
                    GradeId = grade.Id
                };
            }));
        }

        /// <summary>
        /// Reads an existing dermatologists in the system that patient rated for n-th page.
        /// </summary>
        /// <response code="200">Read dermatologist.</response>
        [HttpGet("{patientId}/rated-dermatologists/page-to")]
        public IActionResult GetRatedDermatologistPageTo(Guid patientId, [FromQuery] PageDTO pageDTO)
        {
            return Ok(_medicalStaffGradeService.ReadDermatologistThatPatientRatedPageTo(patientId, pageDTO).Select(dermatologist =>
            {
                var grade = _medicalStaffGradeService.Read(patientId, dermatologist.UserId);
                return new MedicalStafftWithGradeResponse
                {
                    MedicalStaff = dermatologist.User as MedicalStaff,
                    Grade = grade.Value,
                    GradeId = grade.Id
                };
            }));
        }

        /// <summary>
        /// Return dermatologist's grade.
        /// </summary>
        /// <response code="200">Read grade.</response>
        [HttpGet("{medicalStaffId}/grade/{patientId}")]
        public IActionResult GetDermatologistGrade(Guid medicalStaffId, Guid patientId)
        {
            var medicalStaff = _medicalStaffService.ReadByUserId(medicalStaffId);
            if (medicalStaff == null)
            {
                throw new MissingEntityException("The given dermatologist does not exist in the system.");
            }

            return Ok(_medicalStaffGradeService.Read(patientId, medicalStaffId));
        }

        /// <summary>
        /// Reads an existing pharmacist in the system that patietn can rate.
        /// </summary>
        /// <response code="200">Read pharmacist.</response>
        [HttpGet("{patientId}/can-rate-pharmacists")]
        public IActionResult GetPharmacistToRate(Guid patientId)
        {
            return Ok(_medicalStaffGradeService.ReadPharmacistsThatPatientCanRate(patientId));
        }

        /// <summary>
        /// Rate medicine.
        /// </summary>
        /// <response code="200">Returns medicine grade.</response>
        /// <response code="400">Patient cannot rate the medicine or already has rated it.</response>
        /// <response code="404">Given patient or medicine does not exist in the system.</response>
        [HttpPost("rate-medicine")]
        public IActionResult RateMedicine(CreateMedicineGradeRequest request)
        {
            var medicineGrade = _mapper.Map<MedicineGrade>(request);
            return Ok(_medicineGradeService.Create(medicineGrade));
        }

        /// <summary>
        /// Reads medicines that patient can rate.
        /// </summary>
        /// <response code="200">Returns medicines.</response>
        [HttpGet("{patientId}/can-rate-medicines")]
        public IActionResult GetMedicinesThatPatientCanRate(Guid patientId)
        {
            return Ok(_medicineGradeService.ReadThatPatientCanRate(patientId));
        }

        /// <summary>
        /// Rate pharmacy.
        /// </summary>
        /// <response code="200">Returns pharmacy grade.</response>
        /// <response code="400">Patient cannot rate the pharmacy or already has rated it.</response>
        /// <response code="404">Given patient or pharmacy does not exist in the system.</response>
        [HttpPost("rate-pharmacy")]
        public IActionResult RatePharmacy(CreatePharmacyGradeRequest request)
        {
            var pharmacyGrade = _mapper.Map<PharmacyGrade>(request);
            return Ok(_pharmacyGradeService.Create(pharmacyGrade));
        }

        /// <summary>
        /// Reads pharmacies that patient can rate.
        /// </summary>
        /// <response code="200">Returns pharmacies.</response>
        [HttpGet("{patientId}/can-rate-pharmacies")]
        public IActionResult GetPharmaciesThatPatientCanRate(Guid patientId)
        {
            return Ok(_pharmacyGradeService.ReadThatPatientCanRate(patientId));
        }

        /// <summary>
        /// Change medical staff's grade.
        /// </summary>
        /// <response code="200">Returns changed grade.</response>
        /// <response code="400">The grade is not in the interval between 1 and 5.</response>
        /// <response code="404">Given grade does not exist in the system.</response>
        [HttpPost("change-medicalStaff-grade")]
        public IActionResult ChangeMedicalStaffGrade(ChangeGradeRequest changeGradeRequest)
        {
            return Ok(_medicalStaffGradeService.ChangeGrade(changeGradeRequest.GradeId, changeGradeRequest.Value));
        }

        /// <summary>
        /// Change medicine's grade.
        /// </summary>
        /// <response code="200">Returns changed grade.</response>
        /// <response code="400">The grade is not in the interval between 1 and 5.</response>
        /// <response code="404">Given grade does not exist in the system.</response>
        [HttpPost("change-medicine-grade")]
        public IActionResult ChangeMedicineGrade(ChangeGradeRequest changeGradeRequest)
        {
            return Ok(_medicineGradeService.ChangeGrade(changeGradeRequest.GradeId, changeGradeRequest.Value));
        }

        /// <summary>
        /// Change grade of pharmacy.
        /// </summary>
        /// <response code="200">Returns changed grade.</response>
        /// <response code="400">The grade is not in the interval between 1 and 5.</response>
        /// <response code="404">Given grade does not exist in the system.</response>
        [HttpPost("change-pharmacy-grade")]
        public IActionResult ChangePharmacyGrade(ChangeGradeRequest changeGradeRequest)
        {
            return Ok(_pharmacyGradeService.ChangeGrade(changeGradeRequest.GradeId, changeGradeRequest.Value));
        }

        /// <summary>
        /// Reads an existing medicines in the system that patient rated.
        /// </summary>
        /// <response code="200">Read medicines.</response>
        [HttpGet("{patientId}/rated-medicines")]
        public IActionResult GetRatedMedicines(Guid patientId)
        {
            return Ok(_medicineGradeService.ReadMedicinesThatPatientRated(patientId).Select(medicine =>
            {
                var grade = _medicineGradeService.Read(patientId, medicine.Id);
                return new MedicineWithGradeResponse
                {
                    Medicine = medicine,
                    Grade = grade.Value,
                    GradeId = grade.Id
                };
            }));
        }

        /// <summary>
        /// Reads an existing medicines in the system that patient rated for n-th page.
        /// </summary>
        /// <response code="200">Read medicines.</response>
        [HttpGet("{patientId}/rated-medicines/pages-to")]
        public IActionResult GetRatedMedicinesPageTo(Guid patientId, [FromQuery] PageDTO pageDTO)
        {
            return Ok(_medicineGradeService.ReadMedicinesThatPatientRatedPageTo(patientId, pageDTO).Select(medicine =>
            {
                var grade = _medicineGradeService.Read(patientId, medicine.Id);
                return new MedicineWithGradeResponse
                {
                    Medicine = medicine,
                    Grade = grade.Value,
                    GradeId = grade.Id
                };
            }));
        }

        /// <summary>
        /// Reads an existing pharmacies in the system that patient rated.
        /// </summary>
        /// <response code="200">Read pharmacies.</response>
        [HttpGet("{patientId}/rated-pharmacies")]
        public IActionResult GetRatedPharmacies(Guid patientId)
        {
            return Ok(_pharmacyGradeService.ReadPharmaciesThatPatientRated(patientId).Select(pharmacy =>
            {
                var grade = _pharmacyGradeService.Read(patientId, pharmacy.Id);
                return new PharmacyWithGradeResponse
                {
                    Pharmacy = pharmacy,
                    Grade = grade.Value,
                    GradeId = grade.Id
                };
            }));
        }

        /// <summary>
        /// Reads an existing pharmacies in the system that patient rated for n-th page.
        /// </summary>
        /// <response code="200">Read pharmacies.</response>
        [HttpGet("{patientId}/rated-pharmacies/page-to")]
        public IActionResult GetRatedPharmacies(Guid patientId, [FromQuery] PageDTO pageDTO)
        {
            return Ok(_pharmacyGradeService.ReadPharmaciesThatPatientRatedPageTo(patientId, pageDTO).Select(pharmacy =>
            {
                var grade = _pharmacyGradeService.Read(patientId, pharmacy.Id);
                return new PharmacyWithGradeResponse
                {
                    Pharmacy = pharmacy,
                    Grade = grade.Value,
                    GradeId = grade.Id
                };
            }));
        }

        /// <summary>
        /// Reads an existing pharmacists in the system that patient rated.
        /// </summary>
        /// <response code="200">Read pharmacists.</response>
        [HttpGet("{patientId}/rated-pharmacists")]
        public IActionResult GetRatedPharmacists(Guid patientId)
        {
            return Ok(_medicalStaffGradeService.ReadPharmacistsThatPatientRated(patientId).Select(pharmacist =>
            {
                var grade = _medicalStaffGradeService.Read(patientId, pharmacist.UserId);
                return new MedicalStafftWithGradeResponse
                {
                    MedicalStaff = pharmacist.User as MedicalStaff,
                    Grade = grade.Value,
                    GradeId = grade.Id
                };
            }));
        }

        /// <summary>
        /// Reads an existing pharmacists in the system that patient rated for n-th page.
        /// </summary>
        /// <response code="200">Read pharmacists.</response>
        [HttpGet("{patientId}/rated-pharmacists/page-to")]
        public IActionResult GetRatedPharmacistsPageTo(Guid patientId, [FromQuery] PageDTO pageDTO)
        {
            return Ok(_medicalStaffGradeService.ReadPharmacistsThatPatientRatedPageTo(patientId, pageDTO).Select(pharmacist => {
                var grade = _medicalStaffGradeService.Read(patientId, pharmacist.UserId);
                return new MedicalStafftWithGradeResponse
                {
                    MedicalStaff = pharmacist.User as MedicalStaff,
                    Grade = grade.Value,
                    GradeId = grade.Id
                };
            }));
        }
    }
}