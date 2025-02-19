using System.Collections.Generic;
using Cards.View;

namespace Cards.Services
{
    public interface ISorting
    {
        List<CardController> SortHand(List<CardController> hand);
    }
}