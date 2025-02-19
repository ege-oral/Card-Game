using System.Collections.Generic;

namespace Cards.Utils
{
    public class CardRankComparer : IComparer<CardController>
    {
        public int Compare(CardController x, CardController y)
        {
            if (x == null || y == null) return 0;
            
            return x.CardData.Rank.CompareTo(y.CardData.Rank);
        }
    }
}