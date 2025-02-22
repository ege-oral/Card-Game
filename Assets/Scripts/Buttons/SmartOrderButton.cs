using Board.Services;
using Buttons.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class SmartOrderButton : MonoBehaviour
    {
        [SerializeField] private Button smartOrderButton;

        private SignalBus _signalBus;
        private IBoardAnimationService _boardAnimationService;

        [Inject]
        public void Construct(SignalBus signalBus, IBoardAnimationService boardAnimationService)
        {
            _signalBus = signalBus;
            _boardAnimationService = boardAnimationService;
        }

        private void Awake()
        {
            smartOrderButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (_boardAnimationService.IsAnyAnimationPlaying()) return;

            _signalBus.Fire<SmartOrderSignal>();
        }
    }
}