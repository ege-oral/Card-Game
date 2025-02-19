using System.Collections.Generic;
using Cards.View;

namespace Cards.Services
{
    public interface ICardSortingService
    { 
        List<CardController> SortHandByRule(List<CardController> hand, ISorting sortingMethod);
    }
}