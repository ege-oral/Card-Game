using Board.Services;
using Buttons.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class RestartGameButton : MonoBehaviour
    {
        [SerializeField] private Button restartGameButton;

        private IBoardAnimationService _boardAnimationService;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(IBoardAnimationService boardAnimationService, SignalBus signalBus)
        {
            _boardAnimationService = boardAnimationService;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            restartGameButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (_boardAnimationService.IsAnyAnimationPlaying()) return;
            
            _signalBus.Fire<RestartGameSignal>();
        }
        
    }
}