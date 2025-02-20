using System.Collections.Generic;
using Cards.View;

namespace Cards.Services.Sorting.Base
{
    public interface ISorting
    {
        List<CardController> SortHand(IReadOnlyList<CardController> hand);
    }
}