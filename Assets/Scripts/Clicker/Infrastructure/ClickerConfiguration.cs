using Clicker.Model.Contracts;
using UnityEngine;

namespace Clicker.Infrastructure
{
    [CreateAssetMenu(fileName = "ClickerConfiguration", menuName = "Configurations/ClickerConfiguration")]
    public class ClickerConfiguration : ScriptableObject, IClickerConfiguration
    {
        [Header("Currency")]
        [SerializeField] private int _clickReward;
        [SerializeField] private int _autoClickReward;
        [SerializeField] private float _autoClickRewardDelay;
        
        [Space]
        [Header("Energy")]
        [SerializeField] private int _clickCost;
        [SerializeField] private int _initialEnergyAmount;
        [SerializeField] private int _maxEnergyAmount;
        [SerializeField] private int _energyRestorationValue;
        [SerializeField] private float _energyRestorationDelay;
        
        [Space]
        [Header("Animations")]
        [SerializeField] private float _animationDuration;
        [SerializeField] private float _coinAnimationDuration;

        public int ClickReward => _clickReward;
        public int AutoClickReward => _autoClickReward;
        public float AutoClickRewardDelay => _autoClickRewardDelay;
        public int ClickCost => _clickCost;
        public int InitialEnergyAmount => _initialEnergyAmount;
        public int MaxEnergyAmount => _maxEnergyAmount;
        public int EnergyRestorationValue => _energyRestorationValue;
        public float EnergyRestorationDelay => _energyRestorationDelay;
        public float AnimationDuration => _animationDuration;
        public float CoinAnimationDuration => _coinAnimationDuration;
    }
}