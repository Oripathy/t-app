using Weather.Infrastructure;
using Zenject;

namespace Weather
{
    public class WeatherConfigurationsInstaller : Installer<WeatherConfigurationsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<WeatherConfiguration>().FromScriptableObjectResource("Configurations/WeatherConfiguration").AsSingle();
        }
    }
}