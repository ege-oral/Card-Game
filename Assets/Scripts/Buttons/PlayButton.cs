using SceneLoader.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        private SignalBus _signalBus;
        private bool _isClicked;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (_isClicked) return; // Prevent multiple clicks
            
            _isClicked = true;
            _signalBus.Fire<LoadNextLevelSignal>();
        }
    }
}