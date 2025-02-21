using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Services.Sorting.Base;
using Cards.Utils;

namespace Cards.Services.Sorting.Strategies
{
    public class SevenSevenSevenSorting : ISorting
    {
        private readonly List<CardData> _priorityCards = new();
        private readonly List<CardData> _leftOverCards = new();
        
        public List<CardData> SortHand(IReadOnlyList<CardData> hand)
        {
            if (hand == null || hand.Count == 0) return null;

            _priorityCards.Clear();
            _leftOverCards.Clear();

            var cardRanksToCardData = CardUtil.GroupCardsByRank(hand);
            foreach (var cardDataList in cardRanksToCardData.Values)
            {
                if (cardDataList.Count >= 3)
                {
                    _priorityCards.AddRange(cardDataList);
                }
                else
                {
                    _leftOverCards.AddRange(cardDataList);
                }
            }

            return _priorityCards.Count == 0 ? hand.ToList() : _priorityCards.Concat(_leftOverCards).ToList();
        }
    }
}