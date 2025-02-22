using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Cards.View
{
    public class CardDragHandler
    {
        private readonly PlayerController _playerController;
        private readonly CardAnimationControllerSo _cardAnimationControllerSo;
        private readonly CardHighlighter _cardHighlighter;
        private readonly CardNeighborFinder _neighborFinder;
        private readonly Camera _mainCamera;

        private CardController _selectedCard;
        private CardController _currentLeft;
        private CardController _previousLeft;
        private CardController _currentRight;
        private CardController _previousRight;

        private IReadOnlyList<CardController> PlayerHand => _playerController.PlayerHand;
        private Vector2 _previousInputPosition;
        private bool _isDragging;

        [Inject]
        public CardDragHandler(PlayerController playerController,
            CardAnimationControllerSo cardAnimationControllerSo,
            CardHighlighter cardHighlighter,
            CardNeighborFinder neighborFinder)
        {
            _playerController = playerController;
            _cardAnimationControllerSo = cardAnimationControllerSo;
            _cardHighlighter = cardHighlighter;
            _neighborFinder = neighborFinder;
            _mainCamera = Camera.main;
        }

        public void HandleDragging()
        {
            if (!_isDragging || _selectedCard == null) return;

            var inputPosition = GetInputPosition();
            _selectedCard.transform.position = GetMouseWorldPosition(inputPosition);

            var normalized = Mathf.InverseLerp(_cardAnimationControllerSo.startPoint.x, 
                                                    _cardAnimationControllerSo.endPoint.x, 
                                                    _selectedCard.transform.position.x);

            _selectedCard.transform.rotation = Quaternion.Euler(0, 0, _cardAnimationControllerSo.GetRotationAngle(normalized));

            UpdateNearestObjects();
            UpdateHighlighting();

            _previousInputPosition = inputPosition;
        }

        public void TryDragging(InputAction.CallbackContext context)
        {
            TryDragging(GetInputPosition());
        }

        public void TryDragging(Vector2 screenPosition)
        {
            var worldPosition = GetMouseWorldPosition(screenPosition);

            if (TryGetCardAtPosition(worldPosition, out _selectedCard))
            {
                _isDragging = true;
                _playerController.RemoveCardFromHand(_selectedCard);
            }
        }

        public void StopDragging(InputAction.CallbackContext context)
        {
            StopDragging();
        }

        public void StopDragging()
        {
            if (_selectedCard == null) return;

            var insertIndex = DetermineInsertIndex();
            _playerController.InsertCardInHand(insertIndex, _selectedCard);
            _cardHighlighter.ClearAllHighlights(_currentLeft, _currentRight, _selectedCard);
            _selectedCard = null;
            _isDragging = false;
        }

        private bool TryGetCardAtPosition(Vector2 position, out CardController cardController)
        {
            var hit = Physics2D.Raycast(position, Vector2.zero);
            if (hit.collider != null && hit.collider.TryGetComponent(out CardController hitCardController))
            {
                // Ensure the card is in the player's hand before returning it
                if (_playerController.PlayerHand.Contains(hitCardController))
                {
                    cardController = hitCardController;
                    return true;
                }
            }

            cardController = null;
            return false;
        }
        
        private void UpdateNearestObjects()
        {
            (_currentLeft, _currentRight) =
                _neighborFinder.GetNearestLeftAndRight(_selectedCard, PlayerHand, _cardAnimationControllerSo.maxDistance);
        }

        private void UpdateHighlighting()
        {
            _cardHighlighter.UpdateHighlighting(_selectedCard, ref _previousLeft, _currentLeft, ref _previousRight,
                _currentRight, _previousInputPosition);
        }

        private int DetermineInsertIndex()
        {
            for (var i = 0; i < PlayerHand.Count; i++)
            {
                if (PlayerHand[i] == _currentLeft)
                    return i + 1;

                if (PlayerHand[i] == _currentRight)
                    return Mathf.Max(0, i);
            }

            return PlayerHand.Count; // Insert at the end if no left/right match
        }

        private Vector2 GetInputPosition()
        {
            return UnityEngine.Input.touchCount > 0
                ? UnityEngine.Input.touches[0].position
                : Mouse.current.position.ReadValue();
        }

        private Vector2 GetMouseWorldPosition(Vector2 inputPosition)
        {
            return _mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y,
                _mainCamera.transform.position.z * -1));
        }
    }
}