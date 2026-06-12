using Clicker;
using Dogs;
using MainUI;
using UnityEngine;
using Weather;
using Zenject;

[CreateAssetMenu(fileName = "ConfigurationsInstaller", menuName = "Configurations/ConfigurationsInstaller")]
public class ConfigurationsInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        ClickerConfigurationsInstaller.Install(Container);
        WeatherConfigurationsInstaller.Install(Container);
        DogsConfigurationsInstaller.Install(Container);
        MainUIConfigurationsInstaller.Install(Container);
    }
}