using System.Collections.Generic;
using Cards.Data;

namespace Cards.Services.Sorting.Base
{
    public interface ISorting
    {
        List<CardData> SortHand(IReadOnlyList<CardData> hand);
    }
}