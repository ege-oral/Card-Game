using System.Collections.Generic;
using UnityEngine;
using Cards.View;
using Player;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Zenject;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CardAnimationControllerSo cardAnimationControllerSo;

        private Camera _mainCamera;
        private GameInput _gameInput;
        private CardController _selectedCard;

        private bool _isDragging;
        private Vector2 _previousInputPosition;
        private IReadOnlyList<CardController> _hand;

        private CardController _currentLeft;
        private CardController _previousLeft;

        private CardController _currentRight;
        private CardController _previousRight;

        private CardHighlighter _cardHighlighter;
        private CardNeighborFinder _neighborFinder;

        [Inject]
        public void Construct(CardHighlighter cardHighlighter, CardNeighborFinder neighborFinder)
        {
            _cardHighlighter = cardHighlighter;
            _neighborFinder = neighborFinder;
        }
        
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
            if (_isDragging == false || _selectedCard == null) return;

            var inputPosition = _gameInput.Player.Position.ReadValue<Vector2>();
            _selectedCard.transform.position = GetMouseWorldPosition(inputPosition);

            var normalized = Mathf.InverseLerp(cardAnimationControllerSo.startPoint.x,
                cardAnimationControllerSo.endPoint.x, _selectedCard.transform.position.x);
            
            _selectedCard.transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(normalized));

            UpdateNearestObjects();
            UpdateHighlighting();
            
            _previousInputPosition = inputPosition;
        }

        private void UpdateNearestObjects()
        {
            (_currentLeft, _currentRight) = _neighborFinder.GetNearestLeftAndRight(_selectedCard, _hand);
        }

        private void UpdateHighlighting()
        {
            _cardHighlighter.UpdateHighlighting(_selectedCard, ref _previousLeft, _currentLeft, ref _previousRight,
                _currentRight, _previousInputPosition);
        }

        private void TryDragging(InputAction.CallbackContext _)
        {
            var inputPosition = _gameInput.Player.Position.ReadValue<Vector2>();
            var worldPosition = GetMouseWorldPosition(inputPosition);

            if (TryGetCardAtPosition(worldPosition, out _selectedCard))
            {
                _isDragging = true;
                var playerHand = playerController.GetHand();
                playerHand.Remove(_selectedCard);
                _hand = playerHand.Items;
            }
        }

        private void StopDragging(InputAction.CallbackContext _)
        {
            if (_selectedCard == null) return;

            var playerHand = playerController.GetHand();
            var insertIndex = DetermineInsertIndex();

            playerHand.Insert(insertIndex, _selectedCard);
    
            _cardHighlighter.ClearAllHighlights(_currentLeft, _currentRight, _selectedCard);
            _selectedCard = null;
            _isDragging = false;
        }

        private int DetermineInsertIndex()
        {
            for (var i = 0; i < _hand.Count; i++)
            {
                if (_hand[i] == _currentLeft)
                    return i + 1;

                if (_hand[i] == _currentRight)
                    return Mathf.Max(0, i);
            }

            return 0; // Insert at the beginning if no left/right match
        }

        private Vector2 GetMouseWorldPosition(Vector2 inputPosition)
        {
            return _mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, _mainCamera.transform.position.z * -1));
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

        private float GetRotationAngle(float t)
        {
            return Mathf.Lerp(cardAnimationControllerSo.zRotationRange, -cardAnimationControllerSo.zRotationRange, t);
        }
    }
}