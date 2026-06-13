using System;

namespace Clicker.Model
{
    public class Currency
    {
        public long Value { get; private set; }

        public event Action<Currency, CurrencyChangeType> ValueChanged;

        public Currency(long value)
        {
            Value = value;
        }

        public void Add(long value)
        {
            if (value <= 0)
            {
                return;
            }

            var result = long.MaxValue - Value < value
                ? long.MaxValue
                : Value + value;
            
            SetValue(result, CurrencyChangeType.Increase);
        }

        public bool Remove(long value)
        {
            if (value <= 0 || Value < value)
            {
                return false;
            }

            return SetValue(Value - value, CurrencyChangeType.Decrease);
        }

        private bool SetValue(long value, CurrencyChangeType changeType)
        {
            var clampedValue = Math.Max(0, value);
            if (Value == clampedValue)
            {
                return false;
            }

            Value = clampedValue;
            ValueChanged?.Invoke(this, changeType);
            return true;
        }
    }

    public enum CurrencyChangeType
    {
        Increase = 0,
        Decrease = 1,
    }
}