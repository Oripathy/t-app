using System.Collections.Generic;
using Newtonsoft.Json;

namespace Weather.Infrastructure
{
    public class WeatherResponse
    {
        [JsonProperty("properties")]
        public WeatherProperties Properties { get; set; }
    }

    public class WeatherProperties
    {
        [JsonProperty("periods")]
        public List<WeatherPeriod> Periods { get; set; }
    }

    public class WeatherPeriod
    {
        [JsonProperty("temperature")]
        public int Temperature { get; set; }
        [JsonProperty("temperatureUnit")]
        public string Unit { get; set; }
    }
}