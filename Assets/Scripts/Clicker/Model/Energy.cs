using System;

namespace Clicker.Model
{
    public class Energy
    {
        public int Value { get; private set; }
        public int MaxValue { get; private set; }

        public event Action<Energy> ValueChanged;

        public Energy(int value, int maxValue)
        {
            Value = value;
            MaxValue = maxValue;
        }

        public void Add(int value)
        {
            if (value <= 0)
            {
                return;
            }

            SetValue(Value + value);
        }

        public bool Remove(int value)
        {
            if (value <= 0 || Value < value)
            {
                return false;
            }

            return SetValue(Value - value);
        }

        private bool SetValue(int value)
        {
            var clampedValue = Math.Clamp(value, 0, MaxValue);
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