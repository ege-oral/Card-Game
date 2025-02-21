using System.Collections.Generic;
using UnityEngine;

namespace Cards.View
{
    public class CardNeighborFinder
    {
        public (CardController left, CardController right) GetNearestLeftAndRight(CardController reference,
            IReadOnlyList<CardController> cards, float maxDistance)
        {
            if (cards == null || cards.Count == 0) return (null, null);

            CardController nearestLeft = null, nearestRight = null;
            float nearestLeftDist = float.MaxValue, nearestRightDist = float.MaxValue;

            foreach (var card in cards)
            {
                if (card == reference) continue;

                var distance = Vector3.Distance(reference.transform.position, card.transform.position);
                if (distance > maxDistance) continue;

                var isOnRight = IsObjectOnRight(reference.transform, card.transform);

                if (isOnRight && distance < nearestRightDist)
                {
                    nearestRight = card;
                    nearestRightDist = distance;
                }
                else if (!isOnRight && distance < nearestLeftDist)
                {
                    nearestLeft = card;
                    nearestLeftDist = distance;
                }
            }

            return (nearestLeft, nearestRight);
        }

        private bool IsObjectOnRight(Transform reference, Transform target)
        {
            return Vector3.Dot(reference.right, (target.position - reference.position).normalized) > 0;
        }
    }
}