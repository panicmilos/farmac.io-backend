using Farmacio_API;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Implementation
{
    public class DummyService : IDummyService
    {
        private readonly IDummyRepository _dummyRepository;

        public DummyService(IDummyRepository dummyRepository)
        {
            _dummyRepository = dummyRepository;
        }

        public IEnumerable<WeatherForecast> Get()
        {
            return _dummyRepository.Get();
        }

        public WeatherForecast Get(Guid id)
        {
            return _dummyRepository.Get(id);
        }
    }
}