using MainUI;
using MainUI.Infrastructure;
using MainUI.Presentation;
using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour, IInitializable
{
    [Inject] private readonly MainUIPresenter _mainUIPresenter;
    [Inject] private readonly MainUIConfiguration _mainUIConfiguration;
    
    [SerializeField] private MainUIView _mainUIView;

    public void Initialize()
    {
        _mainUIPresenter.SetView(_mainUIView);
        _mainUIPresenter.ActivateTab(_mainUIConfiguration.InitialTab);
    }
}