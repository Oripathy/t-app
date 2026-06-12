using System;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Presentation
{
    public class ClickerEnergyWidget : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private TMP_Text _counter;

        private int _counterValue;
        
        public void Initialize(int value, int maxValue)
        {
            _counterValue = value;
            SetCounter(value, maxValue);
            _progressBar.rectTransform.anchorMax = new Vector2((float)value / maxValue, 1f);
        }

        public async UniTask SetEnergyCount(int value, int maxValue, float duration, CancellationToken token)
        {
            try
            {
                var progress = (float)value / maxValue;
                var progressTask = _progressBar.rectTransform.DOAnchorMax(new Vector2(progress, 1f), duration)
                    .ToUniTask(cancellationToken: token);
            
                var counterTask = SetCounter(value, maxValue, duration, token);
                await UniTask.WhenAll(progressTask, counterTask);
            }
            finally
            {
                _counterValue = value;
            }
        }

        private async UniTask SetCounter(int value, int maxValue, float duration, CancellationToken token)
        {
            await _counter.SetRangedValue(_counterValue, value, maxValue, duration, token);
        }

        private void SetCounter(int value, int maxValue)
        {
            _counter.text = $"{value} / {maxValue}";
        }
    }
}