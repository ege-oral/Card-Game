using System.Collections.Generic;
using Cards.Services.Combination;
using Cards.Utils;
using Cards.View;

namespace Cards.Services.Combinations.Combination
{
    public class CardCombinationsService : ICardCombinationsService
    {
        private readonly CardRankComparer _cardRankComparer;
        private readonly List<List<CardController>> _allPossibilities = new();
        private readonly List<List<CardController>> _sevenSevenSevenPossibilities = new();
        private readonly List<List<CardController>> _oneTwoThreePossibilities = new();

        public CardCombinationsService(CardRankComparer cardRankComparer)
        {
            _cardRankComparer = cardRankComparer;
        }

        public List<List<CardController>> GenerateAllCombinations(IReadOnlyList<CardController> hand)
        {
            _allPossibilities.Clear();
            
            var oneTwoThreePossibilities = CalculateAllOneTwoThreeCombinations(hand);
            var sevenSevenSevenPossibilities = CalculateAllSevenSevenSevenCombinations(hand);
            
            _allPossibilities.AddRange(oneTwoThreePossibilities);
            _allPossibilities.AddRange(sevenSevenSevenPossibilities);
            
            return _allPossibilities;
        }
        
        private List<List<CardController>> CalculateAllOneTwoThreeCombinations(IReadOnlyList<CardController> hand)
        {
            _oneTwoThreePossibilities.Clear();
            
            var cardSuitsToController = CardUtil.GroupCardsBySuit(hand);

            foreach (var (_, suitCards) in cardSuitsToController)
            {
                if (suitCards.Count < 3) continue; // Need at least 3 cards

                suitCards.Sort(_cardRankComparer); // Sort by rank

                // Find all possible consecutive groups
                for (var i = 0; i <= suitCards.Count - 3; i++)
                {
                    for (var j = i + 2; j < suitCards.Count; j++)
                    {
                        if (suitCards[j].CardData.Rank == suitCards[j - 1].CardData.Rank + 1 &&
                            suitCards[j - 1].CardData.Rank == suitCards[j - 2].CardData.Rank + 1)
                        {
                            _oneTwoThreePossibilities.Add(suitCards.GetRange(i, j - i + 1)); // Add valid sequence
                        }
                        else break; // Stop if the sequence is broken
                    }
                }
            }

            return _oneTwoThreePossibilities;
        }
        
        private List<List<CardController>> CalculateAllSevenSevenSevenCombinations(IReadOnlyList<CardController> hand)
        {
            _sevenSevenSevenPossibilities.Clear();
            
            var cardRanksToController = CardUtil.GroupCardsByRank(hand);
            
            foreach (var (_, cards) in cardRanksToController)
            {
                if (cards.Count < 3) continue; // Need at least 3 cards

                _sevenSevenSevenPossibilities.Add(cards);
                if (cards.Count == 4)
                {
                    _sevenSevenSevenPossibilities.AddRange(CardUtil.GetCombinations(cards, 3));
                }
            }

            return _sevenSevenSevenPossibilities;
        }
    }
}