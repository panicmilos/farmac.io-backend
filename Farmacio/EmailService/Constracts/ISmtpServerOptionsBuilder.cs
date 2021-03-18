using EmailService.Contracts;
using EmailService.Models;

namespace EmailService.Constracts
{
    public interface ISmtpServerOptionsBuilder : IBuilder<SmtpServerOptions>
    {
        ISmtpServerOptionsBuilder AddServer(string host, int port);

        ISmtpServerOptionsBuilder AddCredentials(string username, string password);

        ISmtpServerOptionsBuilder EnableSsl();
    }
}