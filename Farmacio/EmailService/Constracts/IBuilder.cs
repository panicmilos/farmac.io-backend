namespace EmailService.Contracts
{
    public interface IBuilder<T>
    {
        T Build();
    }
}