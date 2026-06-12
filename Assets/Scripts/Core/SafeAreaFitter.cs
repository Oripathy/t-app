using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        public bool applyOnStart = true;
        public bool autoUpdate = true;
        public bool constrainLeft = true;
        public bool constrainRight = true;
        public bool constrainTop = true;
        public bool constrainBottom = true;

        private RectTransform _rectTransform;
        private Rect _lastSafeArea;
        private Vector2Int _lastScreenSize;
        private ScreenOrientation _lastOrientation;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (applyOnStart) ApplySafeArea();
        }

        private void LateUpdate()
        {
            if (!autoUpdate) return;

            bool changed = Screen.safeArea != _lastSafeArea ||
                           Screen.width != _lastScreenSize.x ||
                           Screen.height != _lastScreenSize.y ||
                           Screen.orientation != _lastOrientation;

            if (changed) ApplySafeArea();
        }

        public void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
            
            _lastSafeArea = safeArea;
            _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            _lastOrientation = Screen.orientation;

            
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            
            if (!constrainLeft) anchorMin.x = 0f;
            if (!constrainRight) anchorMax.x = 1f;
            if (!constrainBottom) anchorMin.y = 0f;
            if (!constrainTop) anchorMax.y = 1f;

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;
        }
        
        public void ForceUpdate() => ApplySafeArea();
    }
}