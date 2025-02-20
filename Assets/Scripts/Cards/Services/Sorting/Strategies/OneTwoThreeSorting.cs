using System.Collections.Generic;
using System.Linq;
using Cards.Services.Sorting.Base;
using Cards.Utils;
using Cards.View;

namespace Cards.Services.Sorting.Strategies
{
    public class OneTwoThreeSorting : ISorting
    {
        private readonly CardRankComparer _cardRankComparer;

        private readonly List<CardController> _priorityCards = new();
        private readonly List<CardController> _leftOverCards = new();
        private readonly List<CardController> _cardSequence = new();

        public OneTwoThreeSorting(CardRankComparer cardRankComparer)
        {
            _cardRankComparer = cardRankComparer;
        }
        
        public List<CardController> SortHand(IReadOnlyList<CardController> hand)
        {
            if (hand == null || hand.Count == 0) return null;

            _priorityCards.Clear();
            _leftOverCards.Clear();

            var cardSuitsToController = CardUtil.GroupCardsBySuit(hand);
            foreach (var cardControllers in cardSuitsToController.Values)
            {
                if (cardControllers.Count < 3)
                {
                    _leftOverCards.AddRange(cardControllers);
                    continue;
                }

                _cardSequence.Clear();
                cardControllers.Sort(_cardRankComparer); // Sort cards within the same suit by rank

                for (var i = 0; i < cardControllers.Count - 1; i++)
                {
                    var currentCard = cardControllers[i];
                    var nextCard = cardControllers[i + 1];

                    _cardSequence.Add(currentCard);

                    var isConsecutive = currentCard.CardData.Rank == nextCard.CardData.Rank - 1;
                    if (!isConsecutive)
                    {
                        StoreSequence();
                        _cardSequence.Clear();
                    }
                }

                // Handle the last sequence
                if (_cardSequence.Count > 0)
                {
                    _cardSequence.Add(cardControllers[^1]); // Add last consecutive card in sequence
                    StoreSequence();
                }
                else
                {
                    _leftOverCards.Add(cardControllers[^1]); // Store last left-over card if no sequence exists
                }
            }

            return _priorityCards.Concat(_leftOverCards).ToList();
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