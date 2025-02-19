using UnityEngine;
using Cards.View;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        private Camera _mainCamera;
        private GameInput _gameInput;
        private CardController _selectedCard;
        
        private bool _isDragging;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
            _gameInput = new GameInput();

            _gameInput.Player.Click.started += TryDragging;
            _gameInput.Player.Click.canceled += StopDragging;
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
            if (_isDragging == false || _selectedCard is null) return;

            var inputPosition = _gameInput.Player.Position.ReadValue<Vector2>();
            _selectedCard.transform.position = GetMouseWorldPosition(inputPosition);
        }

        private void TryDragging(InputAction.CallbackContext _)
        {
            var inputPosition = _gameInput.Player.Position.ReadValue<Vector2>();
            var worldPosition = GetMouseWorldPosition(inputPosition);

            if (TryGetCardAtPosition(worldPosition, out _selectedCard))
            {
                _isDragging = true;
                _selectedCard.UpdateSorting(0, "SelectedCard");
            }
        }

        private void StopDragging(InputAction.CallbackContext _)
        {
            if (_selectedCard != null)
            {
                _selectedCard.UpdateSorting(0);
                _selectedCard = null;
            }

            _isDragging = false;
        }

        private Vector2 GetMouseWorldPosition(Vector2 inputPosition)
        {
            return _mainCamera.ScreenToWorldPoint(
                new Vector3(inputPosition.x, inputPosition.y, _mainCamera.transform.position.z * -1));
        }

        private bool TryGetCardAtPosition(Vector2 position, out CardController cardController)
        {
            var hit = Physics2D.Raycast(position, Vector2.zero);
            if (hit.collider != null && hit.collider.transform.TryGetComponent(out CardController hitCardController))
            {
                cardController = hitCardController;
                return true;
            }

            cardController = null;
            return false;
        }

        private void OnDestroy()
        {
            _gameInput.Player.Click.started -= TryDragging;
            _gameInput.Player.Click.canceled -= StopDragging;
        }
    }
}
