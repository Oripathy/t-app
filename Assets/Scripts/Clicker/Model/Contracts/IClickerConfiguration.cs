namespace Clicker.Model.Contracts
{
    public interface IClickerConfiguration
    {
        int ClickReward { get; }
        int AutoClickReward { get; }
        float AutoClickRewardDelay { get; }
        int ClickCost { get; }
        int InitialEnergyAmount { get; }
        int MaxEnergyAmount { get; }
        int EnergyRestorationValue { get; }
        float EnergyRestorationDelay { get; }
        float AnimationDuration { get; }
    }
}