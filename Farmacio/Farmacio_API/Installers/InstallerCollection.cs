using System.Collections.Generic;

namespace Farmacio_API.Installers
{
    public class InstallerCollection : IInstallerCollection
    {
        private readonly IList<IInstaller> _installers;

        public InstallerCollection()
        {
            _installers = new List<IInstaller>();
        }

        public InstallerCollection(params IInstaller[] installers)
        {
            _installers = new List<IInstaller>(installers);
        }

        public void AddInstaller(IInstaller installer)
        {
            _installers.Add(installer);
        }

        public void Install()
        {
            foreach (var installer in _installers)
            {
                installer.Install();
            }
        }
    }
}