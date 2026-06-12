using Dogs.Infrastructure;
using Zenject;

namespace Dogs
{
    public class DogsConfigurationsInstaller : Installer<DogsConfigurationsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<DogsConfiguration>().FromScriptableObjectResource("Configurations/DogsConfiguration").AsSingle();
        }
    }
}