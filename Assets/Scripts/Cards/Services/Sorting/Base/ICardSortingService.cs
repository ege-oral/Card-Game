using System.Collections.Generic;
using Cards.Data;

namespace Cards.Services.Sorting.Base
{
    public interface ICardSortingService
    { 
        List<CardData> SortHandByRule(IReadOnlyList<CardData> hand, ISorting sortingMethod);
    }
}