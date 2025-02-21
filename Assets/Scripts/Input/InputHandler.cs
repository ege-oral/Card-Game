using UnityEngine;
using Cards.View;
using Player;
using UnityEngine.InputSystem.EnhancedTouch;
using Zenject;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        
        private GameInput _gameInput;
        private CardDragHandler _cardDragHandler;
        
        [Inject]
        public void Construct(CardDragHandler cardDragHandler)
        {
            _cardDragHandler = cardDragHandler;
        }
        
        private void Awake()
        {
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
            _gameInput.Player.Click.started -= _cardDragHandler.TryDragging;
            _gameInput.Player.Click.canceled -= _cardDragHandler.StopDragging;
        }
    }
}