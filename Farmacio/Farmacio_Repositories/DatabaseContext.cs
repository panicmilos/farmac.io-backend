using Farmacio_API;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Repositories
{
    public class DatabaseContext : DbContext
    {
        public DbSet<WeatherForecast> Weather { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }
    }
}