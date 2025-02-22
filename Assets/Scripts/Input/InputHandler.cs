using Cards.View;
using Input.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
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

            _gameInput = new GameInput();
            
            // Subscribe to Input Signals
            _signalBus.Subscribe<EnableInputSignal>(OnEnableInputSignal);
            _signalBus.Subscribe<DisableInputSignal>(OnDisableInputSignal);

            // Register PC Inputs (Mouse)
            _gameInput.Player.Click.started += _cardDragHandler.TryDragging;
            _gameInput.Player.Click.canceled += _cardDragHandler.StopDragging;
        }
        
        private void OnEnable()
        {
            _gameInput.Enable();
            EnhancedTouchSupport.Enable();

            // Subscribe to Mobile Touch Inputs
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnFingerMove;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnFingerUp;
        }

        private void OnDisable()
        {
            _gameInput.Disable();
            EnhancedTouchSupport.Disable();

            // Unsubscribe from Touch Inputs
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= OnFingerMove;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnFingerUp;
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
            TouchSimulation.Enable();
        }

        private void OnDisableInputSignal()
        {
            _gameInput.Disable();
            TouchSimulation.Disable();
        }

        // 🔹 Handle Mobile Touch Inputs
        private void OnFingerDown(Finger finger)
        {
            _ = _cardDragHandler.TryDragging(finger.screenPosition);
        }

        private void OnFingerMove(Finger finger)
        {
            _cardDragHandler.HandleDragging();
        }

        private void OnFingerUp(Finger finger)
        {
            _cardDragHandler.StopDragging();
        }
    }
}