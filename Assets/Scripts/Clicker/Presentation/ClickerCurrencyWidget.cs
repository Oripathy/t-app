using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Clicker.Presentation
{
    public class ClickerCurrencyWidget : MonoBehaviour
    {
        [SerializeField] private RectTransform _iconTransform;
        [SerializeField] private TMP_Text _counter;

        private long _counterValue;
        
        public RectTransform IconTransform => _iconTransform;

        public void Initialize(long value)
        {
            _counterValue = value;
            _counter.text = value.ToString();
        }

        public async UniTask SetValue(long value, float duration, CancellationToken token)
        {
            try
            {
                await _counter.SetValue(_counterValue, value, duration, token);
            }
            finally
            {
                _counterValue = value;
            }
        }
    }
}