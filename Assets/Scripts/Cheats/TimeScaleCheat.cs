using Sirenix.OdinInspector;
using UnityEngine;

namespace Cheats
{
    public class TimeScaleCheat : MonoBehaviour
    {
        [Range(0f, 5f)]
        [SerializeField, Tooltip("Adjust game speed in real-time. 1 = Normal, 0 = Paused, >1 = Fast-forward")]
        private float timeScale = 1f;

        [Button("Set TimeScale"), GUIColor(0.2f, 0.8f, 1f)]
        private void ApplyTimeScale()
        {
            Time.timeScale = timeScale;
            Debug.Log($"TimeScale set to: {timeScale}");
        }

        [Button("Reset TimeScale"), GUIColor(1f, 0.3f, 0.3f)]
        private void ResetTimeScale()
        {
            timeScale = 1f;
            Time.timeScale = timeScale;
            Debug.Log("TimeScale reset to normal (1.0)");
        }

        private void Update()
        {
            Time.timeScale = timeScale;
        }
    }
}