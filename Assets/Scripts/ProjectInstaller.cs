using Core;
using Weather;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        CoreInstaller.Install(Container);
        WeatherInstaller.Install(Container);
    }
}