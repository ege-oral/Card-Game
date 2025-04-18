using Board.Services;
using Buttons.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class OneTwoThreeOrderButton : MonoBehaviour
    {
        [SerializeField] private Button oneTwoThreeButton;

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
            oneTwoThreeButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (_boardAnimationService.IsAnyAnimationPlaying()) return;

            _signalBus.Fire<OneTwoThreeOrderSignal>();
        }
    }
}