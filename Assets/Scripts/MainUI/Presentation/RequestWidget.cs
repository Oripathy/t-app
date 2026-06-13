using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MainUI.Presentation
{
    public class RequestWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private RectTransform _spinner;
        
        private Tween _spinnerTween;

        public void Show(string title)
        {
            _title.text = title;
            _spinner.rotation = Quaternion.Euler(0f, 0f, 0f);
            gameObject.SetActive(true);
            AnimateSpinner();
        }

        public void Hide()
        {
            _spinnerTween.Kill();
            gameObject.SetActive(false);
        }

        private void AnimateSpinner()
        {
            _spinnerTween = _spinner.DORotate(new Vector3(0, 0, -360), 2f)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }
    }
}