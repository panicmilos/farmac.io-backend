using Farmacio_Models.Domain;
using System;

namespace Farmacio_API
{
    public class WeatherForecast : BaseEntity
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public Guid TestId { get; set; }
        public virtual TestEntity Test { get; set; }

        public WeatherForecast()
        {
        }
    }
}