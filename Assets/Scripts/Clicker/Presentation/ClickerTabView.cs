using Core;
using Dogs.Presentation;
using UnityEngine;

namespace Clicker.Presentation
{
    public class ClickerTabView : View
    {
        [SerializeField] private ClickerButton _button;
        [SerializeField] private ClickerCurrencyWidget currencyWidget;
        [SerializeField] private ClickerEnergyWidget energyWidget;

        public ClickerButton Button => _button;
        public ClickerCurrencyWidget CurrencyWidget => currencyWidget;
        public ClickerEnergyWidget EnergyWidget => energyWidget;
    }
}