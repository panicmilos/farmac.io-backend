namespace Farmacio_API.Installers
{
    public interface IInstallerCollection : IInstaller
    {
        void AddInstaller(IInstaller installer);
    }
}