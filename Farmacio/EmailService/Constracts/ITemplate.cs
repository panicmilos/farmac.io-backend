namespace EmailService.Constracts
{
    public interface ITemplate<out T>
    {
        string Name { get; }

        T GetTemplate();
    }
}