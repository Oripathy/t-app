using System;
using Clicker.Model.Contracts;

namespace Clicker.Model
{
    public class ClickerModel
    {
        public Currency Currency { get; }
        public Energy Energy { get; }
        public TimeSpan RemainingAutoClickRewardDelay { get; private set; }
        public TimeSpan RemainingEnergyRestorationDelay { get; private set; }
        public IClickerConfiguration Configuration { get; }

        public ClickerModel(Currency currency, Energy energy, TimeSpan remainingAutoClickRewardDelay, TimeSpan remainingEnergyRestorationDelay, IClickerConfiguration configuration)
        {
            Currency = currency;
            Energy = energy;
            RemainingAutoClickRewardDelay = remainingAutoClickRewardDelay;
            RemainingEnergyRestorationDelay = remainingEnergyRestorationDelay;
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

        private void HandleClick(long reward)
        {
            if (!Energy.Remove(Configuration.ClickCost))
            {
                return;
            }
            
            Currency.Add(reward);
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
            
            HandleClick(Configuration.AutoClickReward);
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