using System.Collections.Generic;
using Cards.Data;
using Cards.View;

namespace Cards.Utils
{
    public static class CardUtil
    {
        public static Dictionary<CardSuit, List<CardController>> GroupCardsBySuit(IReadOnlyList<CardController> hand)
        {
            var cardSuitsToController = new Dictionary<CardSuit, List<CardController>>();

            foreach (var card in hand)
            {
                if (cardSuitsToController.TryGetValue(card.CardData.Suit, out var cardControllers) == false)
                {
                    cardControllers = new List<CardController>();
                    cardSuitsToController[card.CardData.Suit] = cardControllers;
                }
                cardControllers.Add(card);
            }

            return cardSuitsToController;
        }
        
        public static Dictionary<int, List<CardController>> GroupCardsByRank(IReadOnlyList<CardController> hand)
        {
            var cardRanksToController = new Dictionary<int, List<CardController>>();

            foreach (var card in hand)
            {
                if (cardRanksToController.TryGetValue(card.CardData.Rank, out var cardControllers) == false)
                {
                    cardControllers = new List<CardController>();
                    cardRanksToController[card.CardData.Rank] = cardControllers;
                }
                cardControllers.Add(card);
            }

            return cardRanksToController;
        }
        
        public static List<List<T>> GetCombinations<T>(List<T> list, int r)
        {
            var result = new List<List<T>>();
            GenerateCombinations(list, new List<T>(), 0, r, result);
            return result;
        }

        private static void GenerateCombinations<T>(List<T> list, List<T> temp, int start, int r, List<List<T>> result)
        {
            if (temp.Count == r)
            {
                result.Add(new List<T>(temp));
                return;
            }

            for (var i = start; i < list.Count; i++)
            {
                temp.Add(list[i]);
                GenerateCombinations(list, temp, i + 1, r, result);
                temp.RemoveAt(temp.Count - 1);
            }
        }
    }
}