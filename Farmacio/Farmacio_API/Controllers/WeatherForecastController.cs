using AutoMapper;
using Farmacio_API.Contracts.Requests;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDummyService _dummyService;
        private readonly IMapper _mapper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDummyService dummyService, IMapper mapper)
        {
            _logger = logger;
            _dummyService = dummyService;
            _mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _dummyService.Get();
        }

        /// <summary>
        /// Return weather forecast by id.
        /// </summary>
        /// <response code="200">Returns weather forecast.</response>
        /// <response code="404">Unable to return weather forecast because it does not exist.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WeatherForecast), 200)]
        public IActionResult GetById(Guid id)
        {
            var dummyObject = _dummyService.Get(id);
            if (dummyObject == null)
            {
                return NotFound();
            }

            return Ok(dummyObject);
        }

        [HttpPost("create")]
        public IActionResult Create(CreateTestEntityRequest request)
        {
            var testEntity = _mapper.Map<TestEntity>(request);

            if (new Random().Next() % 2 == 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}