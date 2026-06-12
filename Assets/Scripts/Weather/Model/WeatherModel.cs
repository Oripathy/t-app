using System;

namespace Weather.Model
{
    public class WeatherModel
    {
        public int CachedTemperature { get; private set; }
        public string CachedUnit { get; private set; }
        public bool IsValid { get; private set; }

        public event Action WeatherUpdated;

        public void Update(int temperature, string unit)
        {
            CachedTemperature = temperature;
            CachedUnit = unit;
            IsValid = true;
            WeatherUpdated?.Invoke();
        }
    }
}