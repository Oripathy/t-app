using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Presentation
{
    public class ClickerTabView : View
    {
        [SerializeField] private Button _button;
        [SerializeField] private ClickerCurrencyWidget currencyWidget;
        [SerializeField] private ClickerEnergyWidget energyWidget;

        public Button Button => _button;
        public ClickerCurrencyWidget CurrencyWidget => currencyWidget;
        public ClickerEnergyWidget EnergyWidget => energyWidget;
    }
}