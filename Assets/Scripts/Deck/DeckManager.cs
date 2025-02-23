using System.Collections.Generic;
using Cards.Config;
using Cards.Data;
using Cards.View;
using Cards.Pool;
using UnityEngine;
using Zenject;

namespace Deck
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private Transform deckParent;
        [SerializeField] private CardDataConfig cardDataConfig;
        private const string DeckSortingLayerName = "Deck";

        private CardPool _cardPool;
        private readonly List<CardController> _deckList = new();
        private const float CardOffset = 0.015f;

        [Inject]
        public void Construct(CardPool cardPool)
        {
            _cardPool = cardPool;
        }
        
        public void CreateDeck()
        {
            var orderedCardList = GenerateOrderedCardList();
            var shuffledCardList = ShuffleCardList(orderedCardList);
            ArrangeDeckVisually(shuffledCardList);
        }

        private void ResetDeck()
        {
            ReturnAllCardsToPool();
        }
        
        public void ResetDeckAndCreateDeck()
        {
            ResetDeck();
            CreateDeck();
        }

        private void ReturnAllCardsToPool()
        {
            foreach (var card in _deckList)
            {
                _cardPool.ReturnCard(card);
            }

            _deckList.Clear();
        }
        
        public void ReturnCardToDeck(CardController card)
        {
            card.gameObject.SetActive(false);
            card.transform.SetParent(deckParent);
            _deckList.Add(card);
        }

        public bool TryDrawCard(out CardController card)
        {
            if (_deckList.Count > 0)
            {
                card = _deckList[^1];
                _deckList.RemoveAt(_deckList.Count - 1);
                return true;
            }

            card = null;
            return false;
        }

        public bool TryDrawSpecificCard(CardSuit cardSuit, int cardRank, out CardController card)
        {
            for (var i = 0; i < _deckList.Count; i++)
            {
                if (_deckList[i].CardData.Suit == cardSuit && _deckList[i].CardData.Rank == cardRank)
                {
                    card = _deckList[i];
                    _deckList.RemoveAt(i);
                    return true;
                }
            }

            card = null;
            return false;
        }
        
        private List<CardController> GenerateOrderedCardList()
        {
            _deckList.Clear();
            
            foreach (var (suit, spriteData) in cardDataConfig.CardSpritesBySuit)
            {
                foreach (var (rank, sprite) in spriteData)
                {
                    var card = _cardPool.GetCard();
                    var cardData = new CardData(suit, rank, sprite, cardDataConfig.backSprite);

                    card.Initialize(cardData);
                    _deckList.Add(card);
                }
            }

            return _deckList;
        }
        
        private List<CardController> ShuffleCardList(List<CardController> deckList)
        {
            for (var i = deckList.Count - 1; i > 0; i--)
            {
                var randomIndex = Random.Range(0, i + 1);
                (deckList[i], deckList[randomIndex]) = (deckList[randomIndex], deckList[i]);
            }

            return deckList;
        }
        
        private void ArrangeDeckVisually(List<CardController> deckList)
        {
            for (var i = 0; i < deckList.Count; i++)
            {
                var card = deckList[i];
                card.transform.SetParent(deckParent, worldPositionStays: false);
                card.transform.SetSiblingIndex(i);
                card.transform.rotation = Quaternion.Euler(-120, 0, 0);
                card.transform.position = deckParent.position + Vector3.up * (CardOffset * i);
                card.UpdateSorting(i, DeckSortingLayerName);
            }
        }
    }
}