using System.Collections.Generic;
using Cards.Data;
using Cards.View;

namespace Cards.Utils
{
    public static class CardUtil
    {
        public static Dictionary<CardSuit, List<CardController>> GroupCardsBySuit(List<CardController> hand)
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
        
        public static Dictionary<int, List<CardController>> GroupCardsByRank(List<CardController> hand)
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
    }
}