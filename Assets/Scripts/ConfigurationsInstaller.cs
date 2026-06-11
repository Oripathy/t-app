using UnityEngine;
using Weather;
using Zenject;

[CreateAssetMenu(fileName = "ConfigurationsInstaller", menuName = "Configurations/ConfigurationsInstaller")]
public class ConfigurationsInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        WeatherConfigurationsInstaller.Install(Container);
    }
}