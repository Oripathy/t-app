using Weather.Infrastructure;
using Weather.Model;
using Zenject;

namespace Weather
{
    public class WeatherInstaller : Installer<WeatherInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<WeatherModel>().AsSingle(); // TODO: Replace
            Container.BindInterfacesAndSelfTo<WeatherRequester>().AsSingle().NonLazy();
        }
    }
}