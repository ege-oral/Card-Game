using System.Collections.Generic;
using System.Linq;
using Cards.View;
using UnityEngine;

namespace Cards.Services.Combinations.Optimization
{
    public class CardCombinationOptimizerService : ICardCombinationOptimizerService
    {
        private List<CardController> _bestCombination = new();
        public List<CardController> FindBestCombination(IReadOnlyList<CardController> hand, List<List<CardController>> validCombinations)
        {
            _bestCombination.Clear();
            var lowestValue = int.MaxValue;

            foreach (var combination in validCombinations)
            {
                var remainingValue = EvaluateCombination(hand, combination);
                if (remainingValue < lowestValue)
                {
                    lowestValue = remainingValue;
                    _bestCombination = combination;
                }
            }

            lowestValue = lowestValue == int.MaxValue ? hand.Sum(card => card.CardData.Value) : lowestValue; // If no combination was found, return the full hand value
            Debug.Log($"Lowest remaining value: {lowestValue}");
            return _bestCombination;
        }

        private int EvaluateCombination(IReadOnlyList<CardController> hand, List<CardController> combination)
        {
            var tempHand = hand.ToList();
            foreach (var card in combination)
            {
                tempHand.Remove(card);
            }
            return tempHand.Sum(card => card.CardData.Value);
        }
    }
}