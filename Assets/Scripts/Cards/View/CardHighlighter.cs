using UnityEngine;

namespace Cards.View
{
    public class CardHighlighter
    {
        private const string SelectedCardSortingLayer = "SelectedCard";

        public void UpdateHighlighting(CardController selectedCard, ref CardController previousLeft,
            CardController currentLeft, ref CardController previousRight, CardController currentRight, Vector2 previousInputPosition)
        {
            var isDraggingRight = selectedCard.transform.position.x > previousInputPosition.x;

            // Highlight the selected card
            selectedCard.Highlight(0, SelectedCardSortingLayer);

            // Highlight nearest left & right cards
            SwapHighlight(ref previousLeft, currentLeft, isDraggingRight ? 1 : -1);
            SwapHighlight(ref previousRight, currentRight, isDraggingRight ? -1 : 1);
        }

        private void SwapHighlight(ref CardController previous, CardController current, int direction)
        {
            if (previous != current)
            {
                previous?.DeHighlight();
                previous = current;
            }

            current?.Highlight(direction, SelectedCardSortingLayer);
        }

        public void ClearAllHighlights(CardController left, CardController right, CardController selected)
        {
            left?.DeHighlight();
            right?.DeHighlight();
            selected?.DeHighlight();
        }
    }
}