namespace EmailService.Constracts
{
    public interface ITemplate<T>
    {
        string Name { get; }

        T GetTemplate(params object[] templateParams);
    }
}