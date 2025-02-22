using Buttons.Signals;
using Cards.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Buttons.EventDrivenButtons.Base
{
    /// <summary>
    /// Base class for event-driven buttons that enable/disable click listeners based on game events.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class EventDrivenButton : MonoBehaviour
    {
        private Button _button;
        protected SignalBus SignalBus;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            SignalBus = signalBus;
            SubscribeToSignals();
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        
        protected abstract void OnButtonClicked();
        
        private void SubscribeToSignals()
        {
            SignalBus.Subscribe<CardDrawAnimationStartedSignal>(DisableButton);
            SignalBus.Subscribe<HandReArrangeAnimationStartedSignal>(DisableButton);
            SignalBus.Subscribe<CardDrawAnimationFinishedSignal>(EnableButton);
            SignalBus.Subscribe<HandReArrangeAnimationFinishedSignal>(EnableButton);
            SignalBus.Subscribe<RestartGameSignal>(ResetButton);
        }
        
        private void UnsubscribeFromSignals()
        {
            SignalBus.Unsubscribe<CardDrawAnimationStartedSignal>(DisableButton);
            SignalBus.Unsubscribe<HandReArrangeAnimationStartedSignal>(DisableButton);
            SignalBus.Unsubscribe<CardDrawAnimationFinishedSignal>(EnableButton);
            SignalBus.Unsubscribe<HandReArrangeAnimationFinishedSignal>(EnableButton);
            SignalBus.Unsubscribe<RestartGameSignal>(ResetButton);
        }
        
        private void DisableButton()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }
        
        private void EnableButton()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }
        
        private void ResetButton()
        {
            _button.onClick.RemoveAllListeners();
            EnableButton();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromSignals();
        }
    }
}