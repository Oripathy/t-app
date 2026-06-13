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
        }

        public void Dispose()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }

        public void StartWeatherCheckup()
        {
            if (_isRunning)
            {
                return;
            }
            
            _isRunning = true;
            _tokenSource = new CancellationTokenSource();
            CheckupWeather().Forget();
        }

        public void StopWeatherCheckup()
        {
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = null;
            _isRunning = false;
        }

        private async UniTaskVoid CheckupWeather()
        {
            var token = _tokenSource.Token;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await RequestWeather(token);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                if (token.IsCancellationRequested)
                {
                    return;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(_weatherConfiguration.RequestDelay), cancellationToken: token)
                    .SuppressCancellationThrow();
            }
        }

        private async UniTask RequestWeather(CancellationToken token)
        {
            var result = await _requestSender.SendRequest(RequestFactory, "Receiving Weather", token);
            HandleResponse(result);
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
            if (response?.Properties == null || response.Properties.Periods.Count == 0)
            {
                Debug.LogWarning("No periods found");
                return;
            }
            
            var temperature = response.Properties.Periods[0].Temperature;
            var unit = response.Properties.Periods[0].Unit;
            _weatherModel.Update(temperature, unit);
        }
    }
}