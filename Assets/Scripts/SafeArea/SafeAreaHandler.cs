using UnityEngine;

namespace SafeArea
{
    public class SafeAreaHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        private Rect _lastSafeArea = Rect.zero;
        private ScreenOrientation _lastOrientation;

        private void Awake()
        {
            ApplySafeArea();
        }

        private void Update()
        {
            if (Screen.safeArea == _lastSafeArea && Screen.orientation == _lastOrientation) return;
        
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            _lastSafeArea = Screen.safeArea;
            _lastOrientation = Screen.orientation;

            var anchorMin = _lastSafeArea.position / new Vector2(Screen.width, Screen.height);
            var anchorMax = (_lastSafeArea.position + _lastSafeArea.size) / new Vector2(Screen.width, Screen.height);

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}