using System.Collections.Generic;
using Cards.Data;

namespace Cards.Services.Sorting.Base
{
    public class CardSortingService : ICardSortingService
    {
        public List<CardData> SortHandByRule(IReadOnlyList<CardData> hand, ISorting sortingMethod)
        {
            return sortingMethod.SortHand(hand);
        }
    }
}