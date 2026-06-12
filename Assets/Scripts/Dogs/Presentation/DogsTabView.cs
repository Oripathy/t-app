using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Dogs.Presentation
{
    public class DogsTabView : View
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _buttonsContainer;

        public RectTransform ButtonsContainer => _buttonsContainer;

        public void SetScrollEnabled(bool isEnabled)
        {
            _scrollRect.enabled = isEnabled;
        }
    }
}