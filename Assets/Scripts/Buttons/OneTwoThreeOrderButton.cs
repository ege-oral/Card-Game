using Buttons.Signals;
using Cards.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class OneTwoThreeOrderButton : MonoBehaviour
    {
        [SerializeField] private Button oneTwoThreeOrderButton;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CardDrawAnimationFinishedSignal>(OnCardDrawAnimationFinishedSignal);
        }

        private void OnCardDrawAnimationFinishedSignal()
        {
            oneTwoThreeOrderButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _signalBus.Fire<OneTwoThreeOrderSignal>();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<CardDrawAnimationFinishedSignal>(OnCardDrawAnimationFinishedSignal);
        }
    }
}