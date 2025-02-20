using System.Collections.Generic;
using Cards.View;

namespace Cards.Services.Sorting.Base
{
    public interface ICardSortingService
    { 
        List<CardController> SortHandByRule(List<CardController> hand, ISorting sortingMethod);
    }
}