using MainUI.Presentation;
using Zenject;

namespace MainUI
{
    public class MainUIInstaller : Installer<MainUIInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MainUIPresenter>().AsSingle();
        }
    }
}