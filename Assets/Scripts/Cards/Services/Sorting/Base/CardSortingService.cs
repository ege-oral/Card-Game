using System.Collections.Generic;
using Cards.View;

namespace Cards.Services.Sorting.Base
{
    public class CardSortingService : ICardSortingService
    {
        public List<CardController> SortHandByRule(IReadOnlyList<CardController> hand, ISorting sortingMethod)
        {
            return sortingMethod.SortHand(hand);
        }
    }
}