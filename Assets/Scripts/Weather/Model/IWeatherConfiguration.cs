namespace Weather.Model
{
    public interface IWeatherConfiguration
    {
        float RequestDelay { get; }
        int RetryCount { get; }
        int Timeout { get; }
    }
}