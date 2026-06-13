using Clicker.Presentation;
using Core;
using Dogs.Presentation;
using MainUI.Infrastructure;
using Weather.Presentation;

namespace MainUI.Presentation
{
    public class MainUIPresenter : Presenter<MainUIView>
    {
        private readonly ClickerTabPresenter _clickerTabPresenter;
        private readonly WeatherTabPresenter _weatherTabPresenter;
        private readonly DogsTabPresenter _dogsTabPresenter;
        private readonly RequestSender _requestSender;
        private readonly MainUIConfiguration _mainUIConfiguration;
        
        private ITab _activeTab;

        public MainUIPresenter(ClickerTabPresenter clickerTabPresenter, WeatherTabPresenter weatherTabPresenter,
            DogsTabPresenter dogsTabPresenter, RequestSender requestSender, MainUIConfiguration mainUIConfiguration)
        {
            _clickerTabPresenter = clickerTabPresenter;
            _weatherTabPresenter = weatherTabPresenter;
            _dogsTabPresenter = dogsTabPresenter;
            _requestSender = requestSender;
            _mainUIConfiguration = mainUIConfiguration;
        }

        public override void SetView(MainUIView view)
        {
            base.SetView(view);
            
            _clickerTabPresenter.SetView(View.ClickerTabView);
            _weatherTabPresenter.SetView(View.WeatherTabView);
            _dogsTabPresenter.SetView(View.DogsTabView);
            
            View.NavigationPanelView.ClickerButton.onClick.AddListener(ActivateClickerTab);
            View.NavigationPanelView.WeatherButton.onClick.AddListener(ActivateWeatherTab);
            View.NavigationPanelView.DogsButton.onClick.AddListener(ActivateDogsTab);

            _requestSender.RequestSent += OnRequestSent;
            _requestSender.RequestProcessed += OnRequestProcessed;
        }

        public override void Dispose()
        {
            View.NavigationPanelView.ClickerButton.onClick.RemoveListener(ActivateClickerTab);
            View.NavigationPanelView.WeatherButton.onClick.RemoveListener(ActivateWeatherTab);
            View.NavigationPanelView.DogsButton.onClick.RemoveListener(ActivateDogsTab);
            _requestSender.RequestSent -= OnRequestSent;
            _requestSender.RequestProcessed -= OnRequestProcessed;
        }

        public void ActivateTab(TabType tabType)
        {
            switch (tabType)
            {
                case TabType.Clicker:
                    ActivateClickerTab();
                    break;
                
                case TabType.Weather:
                    ActivateWeatherTab();
                    break;
                
                case TabType.Dogs:
                    ActivateDogsTab();
                    break;
            }
        }

        private void ActivateClickerTab()
        {
            ActivateTab(_clickerTabPresenter);
            View.NavigationPanelView.ClickerButton.image.color = _mainUIConfiguration.ActiveButtonColor;
            View.NavigationPanelView.WeatherButton.image.color = _mainUIConfiguration.InactiveButtonColor;
            View.NavigationPanelView.DogsButton.image.color = _mainUIConfiguration.InactiveButtonColor;
        }

        private void ActivateWeatherTab()
        {
            ActivateTab(_weatherTabPresenter);
            View.NavigationPanelView.ClickerButton.image.color = _mainUIConfiguration.InactiveButtonColor;
            View.NavigationPanelView.WeatherButton.image.color = _mainUIConfiguration.ActiveButtonColor;
            View.NavigationPanelView.DogsButton.image.color = _mainUIConfiguration.InactiveButtonColor;
        }

        private void ActivateDogsTab()
        {
            ActivateTab(_dogsTabPresenter);
            View.NavigationPanelView.ClickerButton.image.color = _mainUIConfiguration.InactiveButtonColor;
            View.NavigationPanelView.WeatherButton.image.color = _mainUIConfiguration.InactiveButtonColor;
            View.NavigationPanelView.DogsButton.image.color = _mainUIConfiguration.ActiveButtonColor;
        }

        private void ActivateTab(ITab tab)
        {
            if (_activeTab == tab)
            {
                return;
            }
            
            _activeTab?.Deactivate();
            _activeTab = tab;
            _activeTab.Activate();
        }

        private void OnRequestSent(string title)
        {
            View.RequestWidget.Show(title);
        }

        private void OnRequestProcessed(string title)
        {
            View.RequestWidget.Hide();
        }
    }
}