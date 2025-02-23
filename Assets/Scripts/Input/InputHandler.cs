using Cards.View;
using UnityEngine;
using Zenject;

namespace Input
{
    public class InputHandler : MonoBehaviour
    {
        private CardDragHandler _cardDragHandler;

        [Inject]
        public void Construct(CardDragHandler cardDragHandler, SignalBus signalBus)
        {
            _cardDragHandler = cardDragHandler;
        }

        private void Update()
        {
            HandleMouseInput();
            HandleTouchInput();
        }

        private void HandleMouseInput()
        {
            if (UnityEngine.Input.GetMouseButton(0))
                _ = _cardDragHandler.TryDragging();
            
            if (UnityEngine.Input.GetMouseButton(0))
                _cardDragHandler.HandleDragging();
            
            if (UnityEngine.Input.GetMouseButtonUp(0))
                _cardDragHandler.StopDragging();
        }

        private void HandleTouchInput()
        {
            if (UnityEngine.Input.touchCount == 0) return;

            var touch = UnityEngine.Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _ = _cardDragHandler.TryDragging();
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    _cardDragHandler.HandleDragging();
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    _cardDragHandler.StopDragging();
                    break;
            }
        }
    }
}