using Farmacio_Models.Domain;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacio_API.Examples
{
    public class WeatherForecastExample : IExamplesProvider<WeatherForecast>
    {
        public WeatherForecast GetExamples()
        {
            return new WeatherForecast
            {
                Id = new Guid("08d8e984-d33f-4d7d-8750-6805f2b4121a"),
                Date = DateTime.Now,
                TemperatureC = 10,
                Summary = "CAO",
                TestId = new Guid("08d8e984-d34d-4095-87b8-8e87ba44aebb"),
                Test = new TestEntity
                {
                    Id = new Guid("08d8e984-d34d-4095-87b8-8e87ba44aebb"),
                    Text = "CAO SVETE"
                }
            };
        }
    }
}