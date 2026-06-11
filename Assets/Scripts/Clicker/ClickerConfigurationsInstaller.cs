using Clicker.Infrastructure;
using Zenject;

namespace Clicker
{
    public class ClickerConfigurationsInstaller : Installer<ClickerConfigurationsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ClickerConfiguration>().FromScriptableObjectResource("Configurations/ClickerConfiguration").AsSingle();
        }
    }
}