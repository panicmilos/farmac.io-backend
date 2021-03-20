using Farmacio_API;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Implementation
{
    public class WeatherForecastService : CrudService<WeatherForecast>, IWeatherForecastService
    {
        public WeatherForecastService(IDummyRepository repository):
            base(repository)
        {
        }


    }
}
