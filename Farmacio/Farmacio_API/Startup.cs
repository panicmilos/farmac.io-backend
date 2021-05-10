using Farmacio_API.Filters;
using Farmacio_API.Installers;
using Farmacio_API.Settings;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Farmacio_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var installerCollection = new InstallerCollection(
                new DependencyInjectionInstaller(services),
                new DatabaseInstaller(services, Configuration),
                new SwaggerInstaller(services),
                new AutoMapperInstaller(services),
                new FluentValidationInstaller(services),
                new EmailServiceInstaller(services, Configuration),
                new JwtBearerInstaller(services),
                new HangfireInstaller(services)
            );
            installerCollection.Install();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IBackgroundJobClient backgroundJobClient, 
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGlobalExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var swaggerSettings = new SwaggerSettings();
            Configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerSettings);

            app.UseSwagger(options => { options.RouteTemplate = swaggerSettings.JsonRoute; });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerSettings.UIEndpoint, swaggerSettings.Description);
                options.SupportedSubmitMethods(new[] {
                     SubmitMethod.Get, SubmitMethod.Post,
                     SubmitMethod.Put, SubmitMethod.Delete });
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DashboardFilter() }
            });

            recurringJobManager.AddOrUpdate("Delete negative points", () => serviceProvider.GetService<IPatientService>().DeleteNegativePoints(), "0 0 1 * *");
            recurringJobManager.AddOrUpdate("Delete not picked up reservations", () => serviceProvider.GetService<IReservationService>().DeleteNotPickedUpReservations(), "0 0 0 * * ?");
        }
    }
}