using System.Collections.Generic;
using UnityEngine;
using Cards.View;
using Player;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CardAnimationControllerSo cardAnimationControllerSo; // todo: for testing remove this 
        
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

        private const string SelectedCardSortingLayerName = "SelectedCard";

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
            
            var normalized = Mathf.InverseLerp(cardAnimationControllerSo.startPoint.x, cardAnimationControllerSo.endPoint.x, _selectedCard.transform.position.x);
            _selectedCard.transform.rotation = Quaternion.Euler(0, 0, GetRotationAngle(normalized));
            Debug.Log(normalized);
            HighlightCard(_selectedCard, null, 0);

            UpdateNearestObjects();
            UpdateHighlighting();

            _previousInputPosition = inputPosition;
        }
        
        private void UpdateNearestObjects()
        {
            (_currentLeft, _currentRight) = GetNearestLeftAndRight(_selectedCard, _hand);
        }
        
        private void UpdateHighlighting()
        {
            var dragDirection = IsDraggingRight(_selectedCard.transform.position) ? 1 : -1;

            HighlightCard(_currentLeft, _previousLeft, dragDirection);
            _previousLeft = _currentLeft;

            HighlightCard(_currentRight, _previousRight, -dragDirection);
            _previousRight = _currentRight;
        }
        
        private void HighlightCard(CardController current, CardController previous, int direction)
        {
            if (current != null)
                current.Highlight(direction, SelectedCardSortingLayerName);

            if (previous != null && previous != current)
                previous.DeHighlight();
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
            if (_selectedCard != null)
            {
                var playerHand = playerController.GetHand();

                for (var i = 0; i < _hand.Count; i++)
                {
                    if (_hand[i] == _currentLeft)
                    {
                        playerHand.Insert(i + 1, _selectedCard);
                        break;
                    }
            
                    if (_hand[i] == _currentRight)
                    {
                        var insertIndex = Mathf.Max(0, i);
                        playerHand.Insert(insertIndex, _selectedCard);
                        break;
                    }
                }

                _currentLeft?.DeHighlight();
                _currentRight?.DeHighlight();
                _selectedCard.DeHighlight();
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

        private bool IsDraggingRight(Vector2 currentPosition)
        {
            var isDraggingRight = currentPosition.x > _previousInputPosition.x;
            return isDraggingRight;
        }

        private (CardController left, CardController right) GetNearestLeftAndRight(CardController reference, IReadOnlyList<CardController> cards, float maxDistance = 3f)
        {
            if (cards == null || cards.Count == 0) return (null, null);

            CardController nearestLeft = null, nearestRight = null;
            float nearestLeftDist = float.MaxValue, nearestRightDist = float.MaxValue;

            foreach (var card in cards)
            {
                if (card == reference) continue; // Skip the reference card

                var distance = Vector3.Distance(reference.transform.position, card.transform.position);
                if (distance > maxDistance) continue; // Ignore cards beyond the max distance

                var isOnRight = IsObjectOnRight(reference.transform, card.transform);

                if (isOnRight && distance < nearestRightDist)
                {
                    nearestRight = card;
                    nearestRightDist = distance;
                }
                else if (isOnRight == false && distance < nearestLeftDist)
                {
                    nearestLeft = card;
                    nearestLeftDist = distance;
                }
            }

            return (nearestLeft, nearestRight);
        }
        
        private bool IsObjectOnRight(Transform reference, Transform target)
        {
            var directionToTarget = (target.position - reference.position).normalized;
            return Vector3.Dot(reference.right, directionToTarget) > 0;
        }
        
        private float GetRotationAngle(float t)
        {
            return Mathf.Lerp(cardAnimationControllerSo.zRotationRange, -cardAnimationControllerSo.zRotationRange, t);
        }
    }
}