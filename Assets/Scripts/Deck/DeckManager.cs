using System.Collections.Generic;
using Cards;
using Cards.Factory;
using UnityEngine;
using Zenject;

namespace Deck
{
    public class DeckManager : MonoBehaviour
    {
        private CardControllerFactory _cardControllerFactory;
        
        [SerializeField] private Transform deckParent;
        [SerializeField] private Sprite backSprite;
        [SerializeField] private List<Sprite> cardSprites; // Assign 52 card textures in Inspector

        private readonly CardSuit[] _suits = { CardSuit.Spades, CardSuit.Hearts, CardSuit.Diamonds, CardSuit.Clubs };
        private readonly int[] _ranks = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }; // Ace to King
        
        private readonly List<CardController> _deck = new();
        private const float CardOffset = 0.015f;

        [Inject]
        public void Construct(CardControllerFactory cardControllerFactory)
        {
            _cardControllerFactory = cardControllerFactory;
        }

        private void Start()
        {
            GenerateDeck();
        }
        
        private void GenerateDeck()
        {
            var spriteIndex = 0;
            
            foreach (var suit in _suits)
            {
                foreach (var rank in _ranks)
                {
                    if (spriteIndex >= cardSprites.Count) break;

                    var newCardController = _cardControllerFactory.Create();
                    var newCardData = new CardData(suit, rank, cardSprites[spriteIndex], backSprite);
            
                    newCardController.Initialize(newCardData);
                    newCardController.transform.SetParent(deckParent, worldPositionStays: false);

                    newCardController.transform.rotation = Quaternion.Euler(new Vector3(-120, 0, 0));
                    newCardController.transform.position = new Vector3(deckParent.position.x,
                        deckParent.position.y + CardOffset * spriteIndex, deckParent.position.z);
                    _deck.Add(newCardController);
                    spriteIndex++;
                }
            }
        }
    }
}