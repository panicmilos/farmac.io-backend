using Farmacio_API;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Repositories.Contracts
{
    public interface IDummyRepository
    {
        IEnumerable<WeatherForecast> Get();
    }
}