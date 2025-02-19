using UnityEngine;

namespace Cards.Data
{
    public class CardData
    {
        public CardSuit Suit { get; private set; }
        public int Rank { get; private set; }
        public int Value { get; private set; } // Automatically assigned
        public Sprite FrontSprite { get; private set; }
        public Sprite BackSprite { get; private set; }

        public CardData(CardSuit suit, int rank, Sprite frontSprite, Sprite backSprite)
        {
            Suit = suit;
            Rank = rank;
            FrontSprite = frontSprite;
            BackSprite = backSprite;
            Value = CalculateValue(rank);
        }

        private int CalculateValue(int rank)
        {
            return rank > 10 ? 10 : rank; // Face cards (J, Q, K) = 10, Ace = 1
        }
    }
}