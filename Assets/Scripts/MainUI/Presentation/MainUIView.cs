using Clicker.Presentation;
using Core;
using Dogs.Presentation;
using UnityEngine;
using Weather.Presentation;

namespace MainUI.Presentation
{
    public class MainUIView : View
    {
        [SerializeField] private ClickerTabView clickerTabView;
        [SerializeField] private WeatherTabView weatherTabView;
        [SerializeField] private DogsTabView dogsTabView;
        [SerializeField] private NavigationPanelView _navigationPanelView;
        [SerializeField] private RequestWidget _requestWidget;

        public ClickerTabView ClickerTabView => clickerTabView;
        public WeatherTabView WeatherTabView => weatherTabView;
        public DogsTabView DogsTabView => dogsTabView;
        public NavigationPanelView NavigationPanelView => _navigationPanelView;
        public RequestWidget RequestWidget => _requestWidget;
    }
}