using System.Collections.Generic;
using Cards.Data;

namespace Cards.Utils
{
    public static class CardUtil
    {
        public static Dictionary<CardSuit, List<CardData>> GroupCardsBySuit(IReadOnlyList<CardData> hand)
        {
            var cardSuitsToData = new Dictionary<CardSuit, List<CardData>>();

            foreach (var cardData in hand)
            {
                if (cardSuitsToData.TryGetValue(cardData.Suit, out var cardControllers) == false)
                {
                    cardControllers = new List<CardData>();
                    cardSuitsToData[cardData.Suit] = cardControllers;
                }
                cardControllers.Add(cardData);
            }

            return cardSuitsToData;
        }
        
        public static Dictionary<int, List<CardData>> GroupCardsByRank(IReadOnlyList<CardData> hand)
        {
            var cardRanksToData = new Dictionary<int, List<CardData>>();

            foreach (var cardData in hand)
            {
                if (cardRanksToData.TryGetValue(cardData.Rank, out var cardControllers) == false)
                {
                    cardControllers = new List<CardData>();
                    cardRanksToData[cardData.Rank] = cardControllers;
                }
                cardControllers.Add(cardData);
            }

            return cardRanksToData;
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