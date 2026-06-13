using Clicker.Model;
using Clicker.Presentation;
using Zenject;

namespace Clicker
{
    public class ClickerInstaller : Installer<Coin, ClickerInstaller>
    {
        private readonly Coin _coinPrefab;

        public ClickerInstaller(Coin coinPrefab)
        {
            _coinPrefab = coinPrefab;
        }

        public override void InstallBindings()
        {
            Container.BindMemoryPool<Coin, Coin.Pool>()
                .WithInitialSize(10)
                .ExpandByOneAtATime()
                .FromComponentInNewPrefab(_coinPrefab);
            
            Container.Bind<ClickerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClickerTabPresenter>().AsSingle();
        }
    }
}