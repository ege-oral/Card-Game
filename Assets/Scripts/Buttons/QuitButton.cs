using UnityEngine;
using UnityEngine.UI;

namespace Buttons
{
    public class QuitButton : MonoBehaviour
    {
        [SerializeField ]private Button quitButton;

        private void Awake()
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        
        private void OnQuitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void OnDestroy()
        {
            quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }
    }
}