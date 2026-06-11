using Zenject;

namespace Core
{
    public class CoreInstaller : Installer<CoreInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RequestSender>().AsSingle();
        }
    }
}