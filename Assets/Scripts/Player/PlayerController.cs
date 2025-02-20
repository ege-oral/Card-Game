using System;
using System.Collections.Generic;
using Cards.Data;
using Cards.Services;
using Cards.Services.Sorting;
using Cards.Services.Sorting.Base;
using Cards.View;
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
        private ICardSortingService _cardSortingService;
        private SignalBus _signalBus;

        [SerializeField] private Transform playerHand;
        [SerializeField] private int maxHandSize = 11; // Maximum cards in hand
        [SerializeField] private CardAnimationController cardAnimationController;
        
        // private List<CardController> _hand = new();
        private readonly ReactiveProperty<List<CardController>> _handTest = new();
        
        private ISorting _oneTwoThreeSorting;
        private ISorting _sevenSevenSevenSorting;
        private ISorting _smartSorting;


        [Inject]
        public void Construct(DeckManager deckManager,
            ICardSortingService cardSortingService,
            SignalBus signalBus,
            [Inject(Id = "OneTwoThreeSorting")] ISorting oneTwoThreeSorting,
            [Inject(Id = "SevenSevenSevenSorting")] ISorting sevenSevenSevenSorting,
            [Inject(Id = "SmartSorting")] ISorting smartSorting)
        {
            _deckManager = deckManager;
            _cardSortingService = cardSortingService;
            _signalBus = signalBus;
            _oneTwoThreeSorting = oneTwoThreeSorting;
            _sevenSevenSevenSorting = sevenSevenSevenSorting;
            _smartSorting = smartSorting;
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
        public void DrawMaxNumberCardsWithEffects()
        {
            for (var i = 0; i < maxHandSize; ++i)
            {
                if (_handTest.Value.Count >= maxHandSize) return;
                if (_deckManager.TryDrawCard(out var card) == false) return;
                
                AddItToHand(card);
                cardAnimationController.PlayDrawAnimation(card, _handTest.Value.Count, maxHandSize);
            }
        }

        [Button]
        public void DrawSpecificCards()
        {
            var cardTuples = new List<(CardSuit Suit, int Rank)>
            {
                (CardSuit.Hearts, 1),
                (CardSuit.Spades, 2),
                (CardSuit.Diamonds, 5),
                (CardSuit.Hearts, 4),
                (CardSuit.Spades, 1),
                (CardSuit.Diamonds, 3),
                (CardSuit.Clubs, 4),
                (CardSuit.Spades, 4),
                (CardSuit.Diamonds, 1),
                (CardSuit.Spades, 3),
                (CardSuit.Diamonds, 4)
            };
           
            if (_handTest.Value.Count >= maxHandSize) return;

            foreach (var (suit, rank) in cardTuples)
            {
                if (_deckManager.TryDrawSpecificCard(suit, rank, out var card))
                {
                    AddItToHand(card);
                    cardAnimationController.PlayDrawAnimation(card, _handTest.Value.Count, maxHandSize);
                }
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
            var sortHandByOneTwoThree = _cardSortingService.SortHandByRule(_handTest.Value, _oneTwoThreeSorting);
            _handTest.Value = sortHandByOneTwoThree;
            cardAnimationController.ReArrangeHand(_handTest.Value);
        }
        
        [Button]
        public void SevenSevenSevenOrder()
        {
            var sortHandBySevenSevenSeven = _cardSortingService.SortHandByRule(_handTest.Value, _sevenSevenSevenSorting);
            _handTest.Value = sortHandBySevenSevenSeven;
            cardAnimationController.ReArrangeHand(_handTest.Value);
        }
        
        [Button]
        public void SmartOrder()
        {
            var sortHandBySmart = _cardSortingService.SortHandByRule(_handTest.Value, _smartSorting);
            _handTest.Value = sortHandBySmart;
            cardAnimationController.ReArrangeHand(_handTest.Value);
        }
    }
}