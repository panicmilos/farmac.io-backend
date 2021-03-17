using Farmacio_API;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IDummyService
    {
        IEnumerable<WeatherForecast> Get();

        WeatherForecast Get(Guid id);
    }
}