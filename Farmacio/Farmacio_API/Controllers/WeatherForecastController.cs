using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDummyService _dummyService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDummyService dummyService)
        {
            _logger = logger;
            _dummyService = dummyService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _dummyService.Get();
        }
    }
}