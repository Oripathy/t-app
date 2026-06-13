using System;
using Clicker.Model.Contracts;

namespace Clicker.Model
{
    public class ClickerModel
    {
        public Currency Currency { get; private set; }
        public Energy Energy { get; private set; }
        public TimeSpan RemainingAutoClickRewardDelay { get; private set; }
        public TimeSpan RemainingEnergyRestorationDelay { get; private set; }
        public IClickerConfiguration Configuration { get; }

        public event Action AutoClickRewardReceived;

        public ClickerModel(IClickerConfiguration configuration)
        {
            Currency = new Currency(0);
            Energy = new Energy(configuration.InitialEnergyAmount, configuration.MaxEnergyAmount);
            RemainingAutoClickRewardDelay = TimeSpan.FromSeconds(configuration.AutoClickRewardDelay);
            RemainingEnergyRestorationDelay = TimeSpan.FromSeconds(configuration.EnergyRestorationDelay);
            Configuration = configuration;
        }

        public void HandleClick()
        {
            HandleClick(Configuration.ClickReward);
        }

        public void Tick(float deltaTime)
        {
            UpdateRewardDelay(deltaTime);
            UpdateEnergyRestorationDelay(deltaTime);
        }

        private bool HandleClick(long reward)
        {
            if (!Energy.Remove(Configuration.ClickCost))
            {
                return false;
            }
            
            Currency.Add(reward);
            return true;
        }

        private void UpdateRewardDelay(float deltaTime)
        {
            if (RemainingAutoClickRewardDelay == TimeSpan.Zero)
            {
                return;
            }
            
            RemainingAutoClickRewardDelay -= TimeSpan.FromSeconds(deltaTime);
            if (RemainingAutoClickRewardDelay > TimeSpan.Zero)
            {
                return;
            }
            
            var result = HandleClick(Configuration.AutoClickReward);
            if (result)
            {
                AutoClickRewardReceived?.Invoke();
            }
            
            RemainingAutoClickRewardDelay = TimeSpan.FromSeconds(Configuration.AutoClickRewardDelay);
        }

        private void UpdateEnergyRestorationDelay(float deltaTime)
        {
            if (RemainingEnergyRestorationDelay == TimeSpan.Zero)
            {
                return;
            }
            
            RemainingEnergyRestorationDelay -= TimeSpan.FromSeconds(deltaTime);
            if (RemainingEnergyRestorationDelay > TimeSpan.Zero)
            {
                return;
            }
            
            Energy.Add(Configuration.EnergyRestorationValue);
            if (Energy.Value >= Energy.MaxValue)
            {
                RemainingEnergyRestorationDelay = TimeSpan.Zero;
                return;
            }

            RemainingEnergyRestorationDelay = TimeSpan.FromSeconds(Configuration.EnergyRestorationDelay);
        }
    }
}