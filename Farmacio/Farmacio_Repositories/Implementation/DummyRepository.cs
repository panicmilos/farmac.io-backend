using Farmacio_API;
using Farmacio_Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Repositories.Implementation
{
    public class DummyRepository : IDummyRepository
    {
        private readonly DatabaseContext _context;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public DummyRepository(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var weathers = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                Test = new Farmacio_Models.Domain.TestEntity()
            })
            .ToArray();
            _context.Add(weathers[0]);
            _context.SaveChanges();
            LoadNotLoadedRefferences(weathers[0]);

            return weathers;
        }

        public WeatherForecast Get(Guid id)
        {
            return _context.Weather.Find(id);
        }

        private void LoadNotLoadedRefferences(WeatherForecast entity)
        {
            foreach (var reference in _context.Entry(entity).References)
            {
                if (!reference.IsLoaded)
                {
                    reference.Load();
                }
            }
        }
    }
}