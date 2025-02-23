using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Services.Sorting.Base;
using Cards.Utils;

namespace Cards.Services.Sorting.Strategies
{
    public class OneTwoThreeSorting : ISorting
    {
        private readonly CardRankComparer _cardRankComparer;

        private readonly List<CardData> _priorityCards = new();
        private readonly List<CardData> _leftOverCards = new();
        private readonly List<CardData> _cardSequence = new();

        public OneTwoThreeSorting(CardRankComparer cardRankComparer)
        {
            _cardRankComparer = cardRankComparer;
        }
        
        public List<CardData> SortHand(IReadOnlyList<CardData> hand)
        {
            if (hand == null || hand.Count == 0) return null;

            _priorityCards.Clear();
            _leftOverCards.Clear();

            var cardSuitsToDataList = CardUtil.GroupCardsBySuit(hand);
            foreach (var cardDataList in cardSuitsToDataList.Values)
            {
                if (cardDataList.Count < 3)
                {
                    _leftOverCards.AddRange(cardDataList);
                    continue;
                }

                _cardSequence.Clear();
                cardDataList.Sort(_cardRankComparer); // Sort cards within the same suit by rank

                for (var i = 0; i < cardDataList.Count - 1; i++)
                {
                    var currentCard = cardDataList[i];
                    var nextCard = cardDataList[i + 1];

                    _cardSequence.Add(currentCard);

                    var isConsecutive = currentCard.Rank == nextCard.Rank - 1;
                    if (isConsecutive == false)
                    {
                        StoreSequence();
                        _cardSequence.Clear();
                    }
                }

                // Handle the last sequence
                if (_cardSequence.Count > 0)
                {
                    _cardSequence.Add(cardDataList[^1]); // Add last consecutive card in sequence
                    StoreSequence();
                }
                else
                {
                    _leftOverCards.Add(cardDataList[^1]); // Store last left-over card if no sequence exists
                }
            }

            return _priorityCards.Count == 0 ? hand.ToList() : _priorityCards.Concat(_leftOverCards).ToList();
        }
        
        private void StoreSequence()
        {
            if (_cardSequence.Count >= 3)
            {
                _priorityCards.AddRange(_cardSequence);
            }
            else
            {
                _leftOverCards.AddRange(_cardSequence);
            }
        }
    }
}