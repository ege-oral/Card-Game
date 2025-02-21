using Buttons.Signals;
using Cards.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons
{
    public class SmartOrderButton : MonoBehaviour
    {
        [SerializeField] private Button smartOrderButton;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<CardDrawAnimationFinishedSignal>(OnCardDrawAnimationFinishedSignal);
        }

        private void OnCardDrawAnimationFinishedSignal()
        {
            smartOrderButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _signalBus.Fire<SmartOrderSignal>();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<CardDrawAnimationFinishedSignal>(OnCardDrawAnimationFinishedSignal);
        }
    }
}