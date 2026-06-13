using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dogs.Presentation
{
    public class ClickerButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _mainPart;
        [SerializeField] private float _mainPartOffset;
        [SerializeField] private float _duration;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private AudioSource _audioSource;

        private Sequence _sequence;
        
        public Vector2 MainPartPosition => _mainPart.rectTransform.position;
        
        public event Action Clicked;

        private void Awake()
        {
            _mainPart.alphaHitTestMinimumThreshold = 1f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != _mainPart.gameObject)
            {
                return;
            }
            
            Click();
            Clicked?.Invoke();
        }

        public void Click()
        {
            _sequence?.Kill();
            _particleSystem.Stop();
            _particleSystem.Play();
            _audioSource.Play();
            _sequence = DOTween.Sequence()
                .Append(_mainPart.rectTransform.DOAnchorPosY(-_mainPartOffset, GetPressDuration()))
                .Append(_mainPart.rectTransform.DOAnchorPosY(0f, _duration));
        }

        public void Reset()
        {
            _sequence?.Kill();
            _mainPart.rectTransform.anchoredPosition = Vector2.zero;
        }

        private float GetPressDuration()
        {
            var ratio = (_mainPartOffset - Mathf.Abs(_mainPart.rectTransform.anchoredPosition.y)) / _mainPartOffset;
            return ratio * _duration;
        }
    }
}