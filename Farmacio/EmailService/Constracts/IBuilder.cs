namespace EmailService.Contracts
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}