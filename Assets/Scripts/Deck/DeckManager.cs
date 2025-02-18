using System.Collections.Generic;
using Cards;
using Cards.Config;
using Cards.Factory;
using UnityEngine;
using Zenject;

namespace Deck
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private Transform deckParent;
        [SerializeField] private CardDataConfig cardDataConfig;
        
        private CardControllerFactory _cardFactory;
        private readonly List<CardController> _orderedDeckList = new();
        
        private readonly Stack<CardController> _deckStack = new();
        private const float CardOffset = 0.015f;

        [Inject]
        public void Construct(CardControllerFactory cardFactory)
        {
            _cardFactory = cardFactory;
        }

        private void Start()
        {
            GenerateOrderedCardList();
            ShuffleCardList();
            GenerateRandomStackDeck();
        }
        
        public bool TryDrawCard(out CardController card)
        {
            if (_deckStack.Count > 0)
            {
                card = _deckStack.Pop();
                return true;
            }

            card = null;
            return false;
        }
        
        private void GenerateOrderedCardList()
        {
            _orderedDeckList.Clear();
            
            foreach (var (suit, spriteData) in cardDataConfig.CardSpritesBySuit)
            {
                foreach (var (rank, sprite) in spriteData)
                {
                    var card = _cardFactory.Create();
                    var cardData = new CardData(suit, rank, sprite, cardDataConfig.backSprite);

                    card.Initialize(cardData);
                    _orderedDeckList.Add(card);
                }
            }
        }
        
        private void ShuffleCardList()
        {
            if (_orderedDeckList.Count <= 1) return;

            for (var i = _orderedDeckList.Count - 1; i > 0; i--)
            {
                var randomIndex = Random.Range(0, i + 1);
                (_orderedDeckList[i], _orderedDeckList[randomIndex]) = (_orderedDeckList[randomIndex], _orderedDeckList[i]);
            }
        }
        
        private void GenerateRandomStackDeck()
        {
            _deckStack.Clear();
            for (var i = 0; i < _orderedDeckList.Count; i++)
            {
                var card = _orderedDeckList[i];
                
                card.transform.SetParent(deckParent, worldPositionStays: false);
                card.transform.rotation = Quaternion.Euler(-120, 0, 0);
                card.transform.position = deckParent.position + Vector3.up * (CardOffset * i);

                _deckStack.Push(card);
            }
        }
    }
}