using Buttons.Signals;
using Cards.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class SevenSevenSevenOrderButton : MonoBehaviour
    {
        [SerializeField] private Button sevenSevenSevenOrderButton;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CardDrawAnimationFinishedSignal>(OnCardDrawAnimationFinishedSignal);
        }

        private void OnCardDrawAnimationFinishedSignal()
        {
            sevenSevenSevenOrderButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _signalBus.Fire<SevenSevenSevenOrderSignal>();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<CardDrawAnimationFinishedSignal>(OnCardDrawAnimationFinishedSignal);
        }
    }
}