using System;
using System.Collections.Generic;
using System.Linq;
using Cards.View;
using Cards.Utils;
using Common;
using Deck;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private DeckManager _deckManager;
        private SignalBus _signalBus;
        private readonly CardRankComparer _cardRankComparer = new();

        [SerializeField] private Transform playerHand;
        [SerializeField] private int maxHandSize = 11; // Maximum cards in hand
        [SerializeField] private CardAnimationController cardAnimationController;
        
        // private List<CardController> _hand = new();
        private readonly ReactiveProperty<List<CardController>> _handTest = new();
        
        private readonly List<CardController> _priorityCards = new();
        private readonly List<CardController> _leftOverCards = new();
        private readonly List<CardController> _cardSequence = new();

        [Inject]
        public void Construct(DeckManager deckManager, SignalBus signalBus)
        {
            _deckManager = deckManager;
            _signalBus = signalBus;
            _handTest.Value = new List<CardController>();
            _handTest.ValueChanged += TestMethod;
        }

        private void TestMethod(List<CardController> obj)
        {
        }

        [Button]
        public async void DrawACard()
        {
            try
            {
                if (_handTest.Value.Count >= maxHandSize) return;
                if (_deckManager.TryDrawCard(out var card) == false) return;
            
                AddItToHand(card);
                await cardAnimationController.PlayDrawAnimation(card, _handTest.Value.Count, maxHandSize);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        [Button]
        public async void DrawNumberOfCards(int drawCount)
        {
            try
            {
                for (var i = 0; i < drawCount; ++i)
                {
                    if (_handTest.Value.Count >= maxHandSize) return;
                    if (_deckManager.TryDrawCard(out var card) == false) return;
                
                    AddItToHand(card);
                    await cardAnimationController.PlayDrawAnimation(card, _handTest.Value.Count, maxHandSize);

                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        [Button]
        public void DrawNumberOfCardsWithEffects(int drawCount)
        {
            for (var i = 0; i < drawCount; ++i)
            {
                if (_handTest.Value.Count >= maxHandSize) return;
                if (_deckManager.TryDrawCard(out var card) == false) return;
                
                AddItToHand(card);
                cardAnimationController.PlayDrawAnimation(card, _handTest.Value.Count, maxHandSize);
            }
        }
        
        private void AddItToHand(CardController card)
        {
            _handTest.Value.Add(card);
            card.transform.SetParent(playerHand);
        }
        
        [Button]
        public void OneTwoThreeOrder()
        {
            _priorityCards.Clear();
            _leftOverCards.Clear();

            var cardSuitsToController = CardUtil.GroupCardsBySuit(_handTest.Value);
            foreach (var cardControllers in cardSuitsToController.Values)
            {
                if (cardControllers.Count < 3)
                {
                    _leftOverCards.AddRange(cardControllers);
                    continue;
                }

                _cardSequence.Clear();
                cardControllers.Sort(_cardRankComparer); // Sort cards within the same suit by rank

                for (var i = 0; i < cardControllers.Count - 1; i++) 
                {
                    var currentCard = cardControllers[i];
                    var nextCard = cardControllers[i + 1];

                    _cardSequence.Add(currentCard);

                    var isConsecutive = currentCard.CardData.Rank == nextCard.CardData.Rank - 1;
                    if (isConsecutive == false)
                    {
                        StoreSequence(_cardSequence);
                        _cardSequence.Clear();    
                    }
                }

                // Handle the last Sequence
                if (_cardSequence.Count > 0)
                {
                    _cardSequence.Add(cardControllers[^1]); // Add last consecutive card in sequence
                    StoreSequence(_cardSequence);
                }
                else
                {
                    _leftOverCards.Add(cardControllers[^1]); // Store last left-over card if no sequence exists
                }
            }

            _priorityCards.AddRange(_leftOverCards);
            _handTest.Value = _priorityCards.ToList();
            cardAnimationController.ReArrangeHand(_handTest.Value);
        }
        
        
        private void StoreSequence(List<CardController> cardSequence)
        {
            if (cardSequence.Count >= 3)
            {
                _priorityCards.AddRange(cardSequence);
            }
            else
            {
                _leftOverCards.AddRange(cardSequence);
            }
        }
        
    }
}