using AutoMapper;
using EmailService.Constracts;
using EmailService.Enums;
using EmailService.Implementation;
using EmailService.Models;
using Farmacio_API.Contracts.Requests;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider<Email> _templatesProvider;
        private readonly IDummyService _dummyService;
        private readonly IMapper _mapper;

        public WeatherForecastController(IEmailDispatcher emailDispatcher, ITemplatesProvider<Email> templatesProvider, IDummyService dummyService, IMapper mapper)
        {
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
            _dummyService = dummyService;
            _mapper = mapper;
        }

        [HttpGet("sendEmail")]
        public Email SendEmail()
        {
            var email = _templatesProvider.FromTemplate("Email1", "panic.milos99@gmail.com", "Milos");
            _emailDispatcher.Dispatch(email);

            return email;
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