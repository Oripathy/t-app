using System;

namespace Clicker.Model
{
    public class Currency
    {
        public long Value { get; private set; }

        public event Action<Currency> ValueChanged;

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
            
            SetValue(result);
        }

        public bool Remove(long value)
        {
            if (value <= 0 || Value < value)
            {
                return false;
            }

            return SetValue(Value - value);
        }

        private bool SetValue(long value)
        {
            var clampedValue = Math.Max(0, value);
            if (Value == clampedValue)
            {
                return false;
            }

            Value = clampedValue;
            ValueChanged?.Invoke(this);
            return true;
        }
    }
}