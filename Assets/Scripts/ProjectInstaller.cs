using Clicker;
using Core;
using Dogs;
using Dogs.Presentation;
using MainUI;
using UnityEngine;
using Weather;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private Bootstrapper _bootstrapper;
    [SerializeField] private BreedButton _breedButtonPrefab;
    
    public override void InstallBindings()
    {
        CoreInstaller.Install(Container);
        ClickerInstaller.Install(Container);
        WeatherInstaller.Install(Container);
        DogsInstaller.Install(Container, _breedButtonPrefab);
        MainUIInstaller.Install(Container);

        Container.BindInterfacesAndSelfTo<Bootstrapper>()
            .FromInstance(_bootstrapper)
            .AsSingle()
            .NonLazy();
    }
}