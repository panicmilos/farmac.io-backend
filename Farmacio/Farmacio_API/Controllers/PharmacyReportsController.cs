using AutoMapper;
using Farmacio_API.Contracts.Requests.PharmacyReports;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class PharmacyReportsController : ControllerBase
    {
        private readonly IPharmacyReportsService _pharmacyReportsService;
        private readonly IMapper _mapper;

        public PharmacyReportsController(IPharmacyReportsService pharmacyReportsService, IMapper mapper)
        {
            _pharmacyReportsService = pharmacyReportsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Generates a pharmacy examinations report based on the given time interval.
        /// </summary>
        /// <response code="200">Generated report.</response>
        [HttpPost("pharmacies/{pharmacyId}/examination-report")]
        public IActionResult PharmacyExaminationReport(Guid pharmacyId, TimePeriodRequest timePeriodRequest)
        {
            var timePeriodDto = _mapper.Map<TimePeriodDTO>(timePeriodRequest);
            return Ok(_pharmacyReportsService.GenerateExaminationsReportFor(pharmacyId, timePeriodDto));
        }

        /// <summary>
        /// Generates a pharmacy medicine consumption report based on the given time interval.
        /// </summary>
        /// <response code="200">Generated report.</response>
        [HttpPost("pharmacies/{pharmacyId}/medicine-consumption-report")]
        public IActionResult PharmacyMedicineConsumptionReport(Guid pharmacyId, TimePeriodRequest timePeriodRequest)
        {
            var timePeriodDto = _mapper.Map<TimePeriodDTO>(timePeriodRequest);
            return Ok(_pharmacyReportsService.GenerateMedicineConsumptionReportFor(pharmacyId, timePeriodDto));
        }

        /// <summary>
        /// Generates a pharmacy income report based on the given time interval.
        /// </summary>
        /// <response code="200">Generated report.</response>
        [HttpPost("pharmacies/{pharmacyId}/pharmacy-income-report")]
        public IActionResult PharmacyIncomeReport(Guid pharmacyId, TimePeriodRequest timePeriodRequest)
        {
            var timePeriodDto = _mapper.Map<TimePeriodDTO>(timePeriodRequest);
            return Ok(_pharmacyReportsService.GeneratePharmacyIncomeReportFor(pharmacyId, timePeriodDto));
        }
    }
}