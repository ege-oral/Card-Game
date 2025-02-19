using System.Collections.Generic;
using Cards.View;

namespace Cards.Services
{
    public class CardSortingService : ICardSortingService
    {
        public List<CardController> SortHandByRule(List<CardController> hand, ISorting sortingMethod)
        {
            return sortingMethod.SortHand(hand);
        }
    }
}