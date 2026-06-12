using System;
using System.Threading;
using Clicker.Model;
using Clicker.Model.Contracts;
using Core;
using Cysharp.Threading.Tasks;
using MainUI;
using MainUI.Presentation;
using UnityEngine;
using Zenject;

namespace Clicker.Presentation
{
    public class ClickerTabPresenter : Presenter<ClickerModel, ClickerTabView>, ITab, ITickable
    {
        private readonly IClickerConfiguration _clickerConfiguration;
        
        private bool _isActive;
        private CancellationTokenSource _currencyTokenSource;
        private CancellationTokenSource _energyTokenSource;
        
        public TabType TabType => TabType.Clicker;

        public ClickerTabPresenter(ClickerModel model, IClickerConfiguration clickerConfiguration) : base(model)
        {
            _clickerConfiguration = clickerConfiguration;
            Model.Currency.ValueChanged += OnCurrencyChanged;
            Model.Energy.ValueChanged += OnEnergyChanged;
        }
        
        public override void SetView(ClickerTabView view)
        {
            base.SetView(view);
            
            View.Button.onClick.AddListener(OnButtonClicked);
        }

        public override void Dispose()
        {
            base.Dispose();
            
            View.Button.onClick.RemoveListener(OnButtonClicked);
            Model.Currency.ValueChanged -= OnCurrencyChanged;
            Model.Energy.ValueChanged -= OnEnergyChanged;
        }

        public void Activate()
        {
            _isActive = true;
            View.gameObject.SetActive(true);
            View.CurrencyWidget.Initialize(Model.Currency.Value);
            View.EnergyWidget.Initialize(Model.Energy.Value, Model.Energy.MaxValue);
        }

        public void Deactivate()
        {
            _isActive = false;
            View.gameObject.SetActive(false);
        }

        public void Tick()
        {
            Model.Tick(Time.deltaTime);
        }

        private void OnButtonClicked()
        {
            Model.HandleClick();
        }

        private void OnCurrencyChanged(Currency currency)
        {
            if (!_isActive)
            {
                return;
            }
            
            _currencyTokenSource?.Cancel();
            _currencyTokenSource?.Dispose();
            _currencyTokenSource = new CancellationTokenSource();
            
            try
            {
                View.CurrencyWidget.SetValue(currency.Value, _clickerConfiguration.AnimationDuration, _currencyTokenSource.Token)
                    .Forget();
            }
            catch (OperationCanceledException e)
            {
            }
        }

        private void OnEnergyChanged(Energy energy)
        {
            if (!_isActive)
            {
                return;
            }
            
            _energyTokenSource?.Cancel();
            _energyTokenSource?.Dispose();
            _energyTokenSource = new CancellationTokenSource();
            
            try
            {
                View.EnergyWidget.SetEnergyCount(energy.Value, energy.MaxValue, _clickerConfiguration.AnimationDuration, _energyTokenSource.Token)
                    .Forget();
            }
            catch (OperationCanceledException e)
            {
            }
        }
    }
}