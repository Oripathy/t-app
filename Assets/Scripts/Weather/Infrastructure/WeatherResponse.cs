using System;
using System.Collections.Generic;

namespace Weather.Infrastructure
{
    [Serializable]
    public class WeatherResponse
    {
        public WeatherProperties properties;
    }

    [Serializable]
    public class WeatherProperties
    {
        public List<WeatherPeriod> periods;
    }

    [Serializable]
    public class WeatherPeriod
    {
        public int number;
        public string name;
        public int temperature;
        public string temperatureUnit;
        public string shortForecast;
        public string detailedForecast;
    }
}