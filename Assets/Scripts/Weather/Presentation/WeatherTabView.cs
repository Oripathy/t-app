using Core;
using TMPro;
using UnityEngine;

namespace Weather.Presentation
{
    public class WeatherTabView : View
    {
        [SerializeField] private RectTransform _temperatureContainer;
        [SerializeField] private TMP_Text _temperatureText;

        public void SetContainerActive(bool isActive)
        {
            _temperatureContainer.gameObject.SetActive(isActive);
        }
        
        public void SetTemperature(int value, string unit)
        {
            _temperatureText.text = $"Today: {value}{unit}";
        }
    }
}