using EmailService.Constracts;
using EmailService.Models;

namespace EmailService.Implementation
{
    public class SmtpServerOptionsBuilder : ISmtpServerOptionsBuilder
    {
        private string _host;
        private int _port;
        private string _username;
        private string _password;
        private bool _ssl;

        public ISmtpServerOptionsBuilder AddServer(string host, int port)
        {
            _host = host;
            _port = port;

            return this;
        }

        public ISmtpServerOptionsBuilder AddCredentials(string username, string password)
        {
            _username = username;
            _password = password;

            return this;
        }

        public ISmtpServerOptionsBuilder EnableSsl()
        {
            _ssl = true;

            return this;
        }

        public SmtpServerOptions Build()
        {
            return new SmtpServerOptions
            {
                Host = _host,
                Port = _port,
                Username = _username,
                Password = _password,
                Ssl = _ssl
            };
        }
    }
}