using Weather.Infrastructure;
using Weather.Model;
using Weather.Presentation;
using Zenject;

namespace Weather
{
    public class WeatherInstaller : Installer<WeatherInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<WeatherModel>().AsSingle();
            Container.Bind<WeatherTabPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherRequester>().AsSingle().NonLazy();
        }
    }
}