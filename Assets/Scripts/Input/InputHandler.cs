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
            _signalBus.Subscribe<EnableInputSignal>(EnableInput);
            _signalBus.Subscribe<DisableInputSignal>(DisableInput);
            
            _gameInput = new GameInput();
            _gameInput.Player.Click.started += _cardDragHandler.TryDragging;
            _gameInput.Player.Click.canceled += _cardDragHandler.StopDragging;
            EnhancedTouchSupport.Enable();
            DisableInput();
        }

        private void Update()
        {
            _cardDragHandler.HandleDragging();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<EnableInputSignal>(EnableInput);
            _signalBus.Unsubscribe<DisableInputSignal>(DisableInput);
            _gameInput.Player.Click.started -= _cardDragHandler.TryDragging;
            _gameInput.Player.Click.canceled -= _cardDragHandler.StopDragging;
            EnhancedTouchSupport.Disable();
        }

        private void EnableInput()
        {
            _gameInput.Enable();
        }

        private void DisableInput()
        {
            _gameInput.Disable();
        }
    }
}