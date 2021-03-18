using EmailService.Models;

namespace EmailService.Constracts
{
    public interface IEmailDispatcher
    {
        void Dispatch(Email email);
    }
}