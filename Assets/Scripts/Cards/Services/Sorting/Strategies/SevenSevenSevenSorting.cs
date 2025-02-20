using System.Collections.Generic;
using System.Linq;
using Cards.Services.Sorting.Base;
using Cards.Utils;
using Cards.View;

namespace Cards.Services.Sorting.Strategies
{
    public class SevenSevenSevenSorting : ISorting
    {
        private readonly List<CardController> _priorityCards = new();
        private readonly List<CardController> _leftOverCards = new();
        
        public List<CardController> SortHand(IReadOnlyList<CardController> hand)
        {
            if (hand == null || hand.Count == 0) return null;

            _priorityCards.Clear();
            _leftOverCards.Clear();

            var cardRanksToController = CardUtil.GroupCardsByRank(hand);
            foreach (var cardControllers in cardRanksToController.Values)
            {
                if (cardControllers.Count >= 3)
                {
                    _priorityCards.AddRange(cardControllers);
                }
                else
                {
                    _leftOverCards.AddRange(cardControllers);
                }
            }

            return _priorityCards.Count == 0 ? hand.ToList() : _priorityCards.Concat(_leftOverCards).ToList();
        }
    }
}