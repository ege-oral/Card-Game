using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using UnityEngine;

namespace Cards.Services.Optimization
{
    public class CardCombinationOptimizerService : ICardCombinationOptimizerService
    {
        private List<CardData> _bestCombination = new();
        public List<CardData> FindBestCombination(IReadOnlyList<CardData> hand, List<List<CardData>> validCombinations)
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

            lowestValue = lowestValue == int.MaxValue ? hand.Sum(card => card.Value) : lowestValue; // If no combination was found, return the full hand value
            Debug.Log($"Lowest remaining value: {lowestValue}");
            return _bestCombination;
        }

        private int EvaluateCombination(IReadOnlyList<CardData> hand, List<CardData> combination)
        {
            var tempHand = hand.ToList();
            foreach (var card in combination)
            {
                tempHand.Remove(card);
            }
            return tempHand.Sum(card => card.Value);
        }
    }
}