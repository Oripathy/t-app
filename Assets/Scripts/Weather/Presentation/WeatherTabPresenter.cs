using Core;
using MainUI;
using MainUI.Presentation;
using Weather.Infrastructure;
using Weather.Model;

namespace Weather.Presentation
{
    public class WeatherTabPresenter : Presenter<WeatherModel, WeatherTabView>, ITab
    {
        private readonly WeatherRequester _weatherRequester;
        
        public WeatherTabPresenter(WeatherModel model, WeatherRequester weatherRequester) : base(model)
        {
            _weatherRequester = weatherRequester;
            model.WeatherUpdated += OnWeatherUpdated;
        }

        public TabType TabType => TabType.Weather;
        
        public void Activate()
        {
            View.gameObject.SetActive(true);
            _weatherRequester.StartWeatherCheckup();
            if (!Model.IsValid)
            {
                return;
            }
            
            OnWeatherUpdated();
        }

        public void Deactivate()
        {
            View.gameObject.SetActive(false);
            View.SetContainerActive(false);
            _weatherRequester.StopWeatherCheckup();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            Model.WeatherUpdated -= OnWeatherUpdated;
        }

        private void OnWeatherUpdated()
        {
            View.SetContainerActive(true);
            View.SetTemperature(Model.CachedTemperature, Model.CachedUnit);
        }
    }
}