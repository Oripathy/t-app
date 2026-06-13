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
using Random = UnityEngine.Random;

namespace Clicker.Presentation
{
    public class ClickerTabPresenter : Presenter<ClickerModel, ClickerTabView>, ITab, ITickable
    {
        private readonly IClickerConfiguration _clickerConfiguration;
        private readonly Coin.Pool _coinPool;
        
        private bool _isActive;
        private CancellationTokenSource _currencyTokenSource;
        private CancellationTokenSource _energyTokenSource;
        
        public TabType TabType => TabType.Clicker;

        public ClickerTabPresenter(ClickerModel model, IClickerConfiguration clickerConfiguration, Coin.Pool coinPool) : base(model)
        {
            _clickerConfiguration = clickerConfiguration;
            _coinPool = coinPool;
            Model.Currency.ValueChanged += OnCurrencyChanged;
            Model.Energy.ValueChanged += OnEnergyChanged;
            Model.AutoClickRewardReceived += ClickButton;
        }
        
        public override void SetView(ClickerTabView view)
        {
            base.SetView(view);

            View.Button.Clicked += OnButtonClicked;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            View.Button.Clicked -= OnButtonClicked;
            Model.Currency.ValueChanged -= OnCurrencyChanged;
            Model.Energy.ValueChanged -= OnEnergyChanged;
            Model.AutoClickRewardReceived -= ClickButton;
            
            _currencyTokenSource?.Cancel();
            _currencyTokenSource?.Dispose();
            _energyTokenSource?.Cancel();
            _energyTokenSource?.Dispose();
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

        private async void OnCurrencyChanged(Currency currency, CurrencyChangeType changeType)
        {
            if (!_isActive)
            {
                return;
            }

            if (changeType == CurrencyChangeType.Decrease)
            {
                _currencyTokenSource?.Cancel();
                _currencyTokenSource?.Dispose();
                _currencyTokenSource = new CancellationTokenSource();
            
                try
                {
                    await View.CurrencyWidget.SetValue(currency.Value, _clickerConfiguration.AnimationDuration, _currencyTokenSource.Token);
                    return;
                }
                catch (OperationCanceledException)
                {
                }
            }

            Coin coin = null;
            try
            {
                _currencyTokenSource ??=  new CancellationTokenSource();
                coin = _coinPool.Spawn();
                coin.transform.SetParent(View.transform);
                coin.transform.localScale = Vector3.one;
                coin.transform.position = Random.insideUnitCircle + View.Button.MainPartPosition;
                await coin.MoveTo(View.CurrencyWidget.IconTransform.position, _clickerConfiguration.CoinAnimationDuration, _currencyTokenSource.Token);
                await View.CurrencyWidget.SetValue(currency.Value, _clickerConfiguration.AnimationDuration, _currencyTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (coin)
                {
                    _coinPool.Despawn(coin);
                }
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

        private void ClickButton()
        {
            if (!_isActive)
            {
                return;
            }
            
            View.Button.Click();
        }
    }
}