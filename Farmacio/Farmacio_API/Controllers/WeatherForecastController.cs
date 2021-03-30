using AutoMapper;
using EmailService.Constracts;
using EmailService.Models;
using Farmacio_API.Contracts.Requests;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Implementation;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ITemplatesProvider _templatesProvider;
        private readonly IDummyService _dummyService;
        private readonly IMapper _mapper;
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly ITokenProvider _tokenService;

        public WeatherForecastController(IEmailDispatcher emailDispatcher, ITemplatesProvider templatesProvider, IDummyService dummyService, IMapper mapper,
            IWeatherForecastService weatherForecastService, ITokenProvider tokenService)
        {
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
            _dummyService = dummyService;
            _mapper = mapper;
            _weatherForecastService = weatherForecastService;
            _tokenService = tokenService;
        }

        [HttpGet("sendEmail")]
        public Email SendEmail()
        {
            var email = _templatesProvider.FromTemplate<Email>("Email3", new { to = "panic.milos99@gmail.com", name = "Milos" });
            _emailDispatcher.Dispatch(email);

            return email;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var salt1 = CryptographyUtils.GetRandomSalt();
            var pw1 = CryptographyUtils.GetSaltedAndHashedPassword("@admin123", salt1);

            var salt2 = CryptographyUtils.GetRandomSalt();
            var pw2 = CryptographyUtils.GetSaltedAndHashedPassword("p@anic123", salt2);

            Console.WriteLine(salt1 + " " + pw1 + " " + salt2 + " " + pw2);
            return _weatherForecastService.Read();
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

        [ProducesResponseType(typeof(WeatherForecast), 200)]
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok(_weatherForecastService.Delete(id));
        }

        [HttpGet("testGEH")]
        public IActionResult Test()
        {
            var throwingService = new ThrowingService();
            throwingService.Throw();

            return Ok();
        }

        [HttpGet("getJwtToken")]
        public IActionResult getJwtToken(Role role)
        {
            Account account = new Account
            {
                Id = new Guid("40f229d5-5394-436a-89ee-6823ab0aae9f"),
                Role = role
            };
            var token = _tokenService.GenerateAuthTokenFor(account);

            return Ok(token);
        }

        [Authorize(Roles = "Patient,Dermatologist")]
        [HttpGet("patientOnly")]
        public IActionResult patientOnly()
        {
            return Ok();
        }
    }
}