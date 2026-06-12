using MainUI.Infrastructure;
using Zenject;

namespace MainUI
{
    public class MainUIConfigurationsInstaller : Installer<MainUIConfigurationsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MainUIConfiguration>().FromScriptableObjectResource("Configurations/MainUIConfiguration").AsSingle();
        }
    }
}