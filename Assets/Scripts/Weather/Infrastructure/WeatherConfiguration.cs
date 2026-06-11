using UnityEngine;
using Weather.Model;

namespace Weather.Infrastructure
{
    [CreateAssetMenu(fileName = "WeatherConfiguration", menuName = "Configurations/WeatherConfiguration")]
    public class WeatherConfiguration : ScriptableObject, IWeatherConfiguration
    {
        [SerializeField] private float _requestDelay;
        [SerializeField] private int _retryCount;
        [SerializeField] private int _timeout;
        [SerializeField] private string _uri;

        public float RequestDelay => _requestDelay;
        public int RetryCount => _retryCount;
        public int Timeout => _timeout;
        public string Uri => _uri;
    }
}