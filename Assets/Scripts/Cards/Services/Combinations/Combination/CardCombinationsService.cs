using System.Collections.Generic;
using Cards.Data;
using Cards.Utils;

namespace Cards.Services.Combinations.Combination
{
    public class CardCombinationsService : ICardCombinationsService
    {
        private readonly CardRankComparer _cardRankComparer;
        private readonly List<List<CardData>> _allPossibilities = new();
        private readonly List<List<CardData>> _sevenSevenSevenPossibilities = new();
        private readonly List<List<CardData>> _oneTwoThreePossibilities = new();

        public CardCombinationsService(CardRankComparer cardRankComparer)
        {
            _cardRankComparer = cardRankComparer;
        }

        public List<List<CardData>> GenerateAllCombinations(IReadOnlyList<CardData> hand)
        {
            _allPossibilities.Clear();
            
            var oneTwoThreePossibilities = CalculateAllOneTwoThreeCombinations(hand);
            var sevenSevenSevenPossibilities = CalculateAllSevenSevenSevenCombinations(hand);
            
            _allPossibilities.AddRange(oneTwoThreePossibilities);
            _allPossibilities.AddRange(sevenSevenSevenPossibilities);
            
            return _allPossibilities;
        }
        
        private List<List<CardData>> CalculateAllOneTwoThreeCombinations(IReadOnlyList<CardData> hand)
        {
            _oneTwoThreePossibilities.Clear();
            
            var cardSuitsToCardData = CardUtil.GroupCardsBySuit(hand);

            foreach (var (_, cardData) in cardSuitsToCardData)
            {
                if (cardData.Count < 3) continue; // Need at least 3 cards

                cardData.Sort(_cardRankComparer); // Sort by rank

                // Find all possible consecutive groups
                for (var i = 0; i <= cardData.Count - 3; i++)
                {
                    for (var j = i + 2; j < cardData.Count; j++)
                    {
                        if (cardData[j].Rank == cardData[j - 1].Rank + 1 &&
                            cardData[j - 1].Rank == cardData[j - 2].Rank + 1)
                        {
                            _oneTwoThreePossibilities.Add(cardData.GetRange(i, j - i + 1)); // Add valid sequence
                        }
                        else break; // Stop if the sequence is broken
                    }
                }
            }

            return _oneTwoThreePossibilities;
        }
        
        private List<List<CardData>> CalculateAllSevenSevenSevenCombinations(IReadOnlyList<CardData> hand)
        {
            _sevenSevenSevenPossibilities.Clear();
            
            var cardRanksToCardData = CardUtil.GroupCardsByRank(hand);
            
            foreach (var (_, cardData) in cardRanksToCardData)
            {
                if (cardData.Count < 3) continue; // Need at least 3 cards

                _sevenSevenSevenPossibilities.Add(cardData);
                if (cardData.Count == 4)
                {
                    _sevenSevenSevenPossibilities.AddRange(CardUtil.GetCombinations(cardData, 3));
                }
            }

            return _sevenSevenSevenPossibilities;
        }
    }
}