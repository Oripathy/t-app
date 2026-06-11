using System;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Weather.Model;
using Zenject;

namespace Weather.Infrastructure
{
    public class WeatherRequester : IInitializable, IDisposable
    {
        private readonly RequestSender _requestSender;
        private readonly WeatherConfiguration _weatherConfiguration;
        private readonly WeatherModel _weatherModel;
        
        private CancellationTokenSource _tokenSource;
        private bool _isRunning;

        public WeatherRequester(RequestSender requestSender, WeatherConfiguration weatherConfiguration, WeatherModel weatherModel)
        {
            _requestSender = requestSender;
            _weatherConfiguration = weatherConfiguration;
            _weatherModel = weatherModel;
        }

        public void Initialize()
        {
            _tokenSource = new CancellationTokenSource();
            StartWeatherCheckup(); // TODO REMOVE
        }

        public void Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }

        private void StartWeatherCheckup()
        {
            if (_isRunning)
            {
                return;
            }
            
            _isRunning = true;
            _tokenSource = new CancellationTokenSource();
            CheckupWeather().Forget();
        }

        private void StopWeatherCheckup()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _isRunning = false;
        }

        private async UniTaskVoid CheckupWeather()
        {
            try
            {
                while (!_tokenSource.IsCancellationRequested)
                {
                    var result = await _requestSender.SendRequest(RequestFactory, _tokenSource.Token);
                    HandleResponse(result);
                    await UniTask.Delay(TimeSpan.FromSeconds(_weatherConfiguration.RequestDelay), cancellationToken: _tokenSource.Token);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }

            return;
            
            UnityWebRequest RequestFactory()
            {
                var request = UnityWebRequest.Get(_weatherConfiguration.Uri);
                request.timeout = _weatherConfiguration.Timeout;
                return request;
            }
        }

        private void HandleResponse(UnityWebRequestResult result)
        {
            var response = JsonConvert.DeserializeObject<WeatherResponse>(result.Text);
            if (response?.properties == null || response.properties.periods.Count == 0)
            {
                Debug.LogWarning("No periods found");
                return;
            }
            
            var currentTemperature = response.properties.periods[0].temperature;
            var unit = response.properties.periods[0].temperatureUnit;
            Debug.Log($"Temperature: {currentTemperature} {unit}");
            // UpdateModel
        }
    }
}