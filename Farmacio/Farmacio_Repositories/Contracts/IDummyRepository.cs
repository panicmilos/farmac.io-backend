using Farmacio_API;
using Farmacio_Repositories.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Repositories.Contracts
{
    public interface IDummyRepository : IRepository<WeatherForecast>
    {
    }
}