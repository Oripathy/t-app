using Clicker.Model;
using Clicker.Presentation;
using Zenject;

namespace Clicker
{
    public class ClickerInstaller : Installer<ClickerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ClickerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClickerTabPresenter>().AsSingle();
        }
    }
}