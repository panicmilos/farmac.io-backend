using EmailService.Constracts;
using EmailService.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EmailService.Extensions
{
    public static class AddEmailDispatcherExtension
    {
        public static void AddEmailDispatcher(this IServiceCollection services, Action<ISmtpServerOptionsBuilder> optionsBuilderActions)
        {
            var optionsBuilder = new SmtpServerOptionsBuilder();
            optionsBuilderActions(optionsBuilder);
            var options = optionsBuilder.Build();

            var emailDispatcher = new EmailDispatcher(options);

            services.AddSingleton(typeof(IEmailDispatcher), emailDispatcher);
        }
    }
}