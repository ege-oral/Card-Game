using UnityEngine;
using Cards.View;
using Input.Signals;
using UnityEngine.InputSystem.EnhancedTouch;
using Zenject;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        private GameInput _gameInput;
        private CardDragHandler _cardDragHandler;
        private SignalBus _signalBus;
        
        [Inject]
        public void Construct(CardDragHandler cardDragHandler, SignalBus signalBus)
        {
            _cardDragHandler = cardDragHandler;
            _signalBus = signalBus;
            _signalBus.Subscribe<EnableInputSignal>(OnEnableInputSignal);
            _signalBus.Subscribe<DisableInputSignal>(OnDisableInputSignal);
            
            _gameInput = new GameInput();
            _gameInput.Player.Click.started += _cardDragHandler.TryDragging;
            _gameInput.Player.Click.canceled += _cardDragHandler.StopDragging;
        }
        
        private void OnEnable()
        {
            _gameInput.Enable();
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            _gameInput.Disable();
            EnhancedTouchSupport.Disable();
        }

        private void Update()
        {
            _cardDragHandler.HandleDragging();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<EnableInputSignal>(OnEnableInputSignal);
            _signalBus.Unsubscribe<DisableInputSignal>(OnDisableInputSignal);
            _gameInput.Player.Click.started -= _cardDragHandler.TryDragging;
            _gameInput.Player.Click.canceled -= _cardDragHandler.StopDragging;
        }

        private void OnEnableInputSignal()
        {
            _gameInput.Enable();
        }

        private void OnDisableInputSignal()
        {
            _gameInput.Disable();
        }
    }
}