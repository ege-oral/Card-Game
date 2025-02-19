using System;
using System.Collections.Generic;
using Cards.Services;
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


        [Inject]
        public void Construct(DeckManager deckManager,
            ICardSortingService cardSortingService,
            SignalBus signalBus,
            [Inject(Id = "OneTwoThreeSorting")] ISorting oneTwoThreeSorting,
            [Inject(Id = "SevenSevenSevenSorting")] ISorting sevenSevenSevenSorting)
        {
            _deckManager = deckManager;
            _cardSortingService = cardSortingService;
            _signalBus = signalBus;
            _oneTwoThreeSorting = oneTwoThreeSorting;
            _sevenSevenSevenSorting = sevenSevenSevenSorting;
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
            var sortHandByOneTwoThree = _cardSortingService.SortHandByRule(_handTest.Value, _sevenSevenSevenSorting);
            _handTest.Value = sortHandByOneTwoThree;
            cardAnimationController.ReArrangeHand(_handTest.Value);
        }
    }
}