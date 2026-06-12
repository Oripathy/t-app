using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Core
{
    public static class TMPExtensions
    {
        public static async UniTask SetRangedValue(this TMP_Text text, int initialValue, int value, int maxValue,
            float duration, CancellationToken token)
        {
            if (Math.Abs(value - initialValue) == 1)
            {
                text.text = $"{value} / {maxValue}";
                return;
            }
            
            var remainingDuration = duration;
            while (remainingDuration >= 0)
            {
                token.ThrowIfCancellationRequested();
                var intermediateValue = (int)Mathf.Lerp(initialValue, value, (duration - remainingDuration) / duration);
                text.text = $"{intermediateValue} / {maxValue}";
                remainingDuration -= Time.deltaTime;
                await UniTask.Yield();
            }

            text.text = $"{value} / {maxValue}";
        }

        public static async UniTask SetValue(this TMP_Text text, long initialValue, long value, float duration,
            CancellationToken token)
        {
            if (Math.Abs(value - initialValue) == 1)
            {
                text.text = value.ToString();
                return;
            }
            
            var remainingDuration = duration;
            while (remainingDuration >= 0)
            {
                token.ThrowIfCancellationRequested();
                var intermediateValue = (long)Mathf.Lerp(initialValue, value, (duration - remainingDuration) / duration);
                text.text = intermediateValue.ToString();
                remainingDuration -= Time.deltaTime;
                await UniTask.Yield();
            }
            
            text.text = value.ToString();
        }
    }
}