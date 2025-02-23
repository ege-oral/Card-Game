using System;
using System.Collections.Generic;
using System.Linq;
using Board.Services;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;
using Zenject;

namespace Cards.View
{
    public class CardDragHandler
    {
        private readonly PlayerController _playerController;
        private readonly CardAnimationControllerSo _cardAnimationControllerSo;
        private readonly CardHighlighter _cardHighlighter;
        private readonly CardNeighborFinder _neighborFinder;
        private readonly IBoardAnimationService _boardAnimationService;
        private readonly Camera _mainCamera;

        private CardController _selectedCard;
        private CardController _currentLeft;
        private CardController _previousLeft;
        private CardController _currentRight;
        private CardController _previousRight;

        private IReadOnlyList<CardController> PlayerHand => _playerController.PlayerHand;
        private Vector2 _previousInputPosition;
        private bool _isDragging;
        private const float CoolDownDelay = 0.2f;

        [Inject]
        public CardDragHandler(
            PlayerController playerController,
            CardAnimationControllerSo cardAnimationControllerSo,
            CardHighlighter cardHighlighter,
            CardNeighborFinder neighborFinder,
            IBoardAnimationService boardAnimationService)
        {
            _playerController = playerController;
            _cardAnimationControllerSo = cardAnimationControllerSo;
            _cardHighlighter = cardHighlighter;
            _neighborFinder = neighborFinder;
            _boardAnimationService = boardAnimationService;
            _mainCamera = Camera.main;
        }

        public void HandleDragging()
        {
            if (!_isDragging || _selectedCard == null) return;

            var inputPosition = GetPointerPosition();
            _selectedCard.transform.position = GetWorldPosition(inputPosition);

            var normalized = Mathf.InverseLerp(
                _cardAnimationControllerSo.startPoint.x,
                _cardAnimationControllerSo.endPoint.x,
                _selectedCard.transform.position.x);

            _selectedCard.transform.rotation = Quaternion.Euler(0, 0, _cardAnimationControllerSo.GetRotationAngle(normalized));

            UpdateNearestObjects();
            UpdateHighlighting();

            _previousInputPosition = inputPosition;
        }

        public async UniTaskVoid TryDragging()
        {
            if (_isDragging || _boardAnimationService.IsAnyAnimationPlaying()) return;

            var screenPosition = GetPointerPosition();
            var worldPosition = GetWorldPosition(screenPosition);
            if (TryGetCardAtPosition(worldPosition, out _selectedCard) == false) return;

            _isDragging = true;
            _playerController.RemoveCardFromHand(_selectedCard);

            await UniTask.Delay(TimeSpan.FromSeconds(CoolDownDelay)); // Prevent spam clicking
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
            cardController = null;
            var hit = Physics2D.Raycast(position, Vector2.zero);
            if (hit.collider == null || hit.collider.TryGetComponent(out CardController hitCardController) == false) return false;
            if (PlayerHand.Contains(hitCardController) == false) return false;

            cardController = hitCardController;
            return true;
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

        private Vector2 GetPointerPosition()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                return UnityEngine.Input.GetTouch(0).position;
            }

            return UnityEngine.Input.mousePosition;
        }

        private Vector2 GetWorldPosition(Vector2 inputPosition)
        {
            return _mainCamera.ScreenToWorldPoint(new Vector3(
                inputPosition.x, inputPosition.y, _mainCamera.transform.position.z * -1));
        }
    }
}