using Clicker;
using Clicker.Presentation;
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
    [SerializeField] private Coin _coinPrefab;
    
    public override void InstallBindings()
    {
        CoreInstaller.Install(Container);
        ClickerInstaller.Install(Container, _coinPrefab);
        WeatherInstaller.Install(Container);
        DogsInstaller.Install(Container, _breedButtonPrefab);
        MainUIInstaller.Install(Container);

        Container.BindInterfacesAndSelfTo<Bootstrapper>()
            .FromInstance(_bootstrapper)
            .AsSingle()
            .NonLazy();
    }
}