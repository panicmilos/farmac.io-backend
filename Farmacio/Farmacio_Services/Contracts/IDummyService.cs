﻿using Farmacio_API;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IDummyService
    {
        IEnumerable<WeatherForecast> Get();
    }
}