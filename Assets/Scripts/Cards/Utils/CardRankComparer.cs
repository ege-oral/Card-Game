using System.Collections.Generic;
using Cards.Data;

namespace Cards.Utils
{
    public class CardRankComparer : IComparer<CardData>
    {
        public int Compare(CardData x, CardData y)
        {
            if (x == null || y == null) return 0;
            
            return x.Rank.CompareTo(y.Rank);
        }
    }
}