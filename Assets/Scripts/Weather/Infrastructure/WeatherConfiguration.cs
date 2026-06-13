using UnityEngine;
using Weather.Model;

namespace Weather.Infrastructure
{
    [CreateAssetMenu(fileName = "WeatherConfiguration", menuName = "Configurations/WeatherConfiguration")]
    public class WeatherConfiguration : ScriptableObject
    {
        [SerializeField] private float _requestDelay;
        [SerializeField] private int _timeout;
        [SerializeField] private string _uri;

        public float RequestDelay => _requestDelay;
        public int Timeout => _timeout;
        public string Uri => _uri;
    }
}