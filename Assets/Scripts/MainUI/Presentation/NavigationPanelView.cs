using UnityEngine;
using UnityEngine.UI;

namespace MainUI.Presentation
{
    public class NavigationPanelView : MonoBehaviour
    {
        [SerializeField] private Button _clickerButton;
        [SerializeField] private Button _weatherButton;
        [SerializeField] private Button _dogsButton;

        public Button ClickerButton => _clickerButton;
        public Button WeatherButton => _weatherButton;
        public Button DogsButton => _dogsButton;
    }
}