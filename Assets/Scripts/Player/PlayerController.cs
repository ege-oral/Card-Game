using System;
using System.Collections.Generic;
using Cards.Data;
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
        
        private readonly ReactiveList<CardController> _playerHand = new();
        
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
            _playerHand.ItemAdded += Draw;
            _playerHand.ItemInserted += ReArrange;
            _playerHand.ListReplaced += ReArrange;
            _playerHand.ItemRemoved += ReArrange;
        }

        private void ReArrange(CardController arg1, int arg2)
        {
            cardAnimationController.ReArrangeHand(_playerHand.Items);

        }
        
        private void ReArrange(object _)
        {
            cardAnimationController.ReArrangeHand(_playerHand.Items);
        }

        private async void Draw(CardController obj)
        {
            try
            {
                await cardAnimationController.PlayDrawAnimation(obj, _playerHand.Items.Count, maxHandSize);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

       
        
        [Button]
        public void DrawACard()
        {
            try
            {
                if (_playerHand.Items.Count >= maxHandSize) return;
                if (_deckManager.TryDrawCard(out var card) == false) return;
            
                AddItToHand(card);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        [Button]
        public void DrawNumberOfCards(int drawCount)
        {
            try
            {
                for (var i = 0; i < drawCount; ++i)
                {
                    if (_playerHand.Items.Count >= maxHandSize) return;
                    if (_deckManager.TryDrawCard(out var card) == false) return;
                
                    AddItToHand(card);

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
                if (_playerHand.Items.Count >= maxHandSize) return;
                if (_deckManager.TryDrawCard(out var card) == false) return;
                
                AddItToHand(card);
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
           
            if (_playerHand.Items.Count >= maxHandSize) return;

            foreach (var (suit, rank) in cardTuples)
            {
                if (_deckManager.TryDrawSpecificCard(suit, rank, out var card))
                {
                    AddItToHand(card);
                }
            }
        }
        
        private void AddItToHand(CardController card)
        {
            _playerHand.Add(card);
            card.transform.SetParent(playerHand);
        }
        
        [Button]
        public void OneTwoThreeOrder()
        {
            if(_playerHand.Items == null || _playerHand.Items.Count == 0) return;
            
            var sortHandByOneTwoThree = _cardSortingService.SortHandByRule(_playerHand.Items, _oneTwoThreeSorting);
            _playerHand.Replace(sortHandByOneTwoThree);
        }
        
        [Button]
        public void SevenSevenSevenOrder()
        {
            if(_playerHand.Items == null || _playerHand.Items.Count == 0) return;

            var sortHandBySevenSevenSeven = _cardSortingService.SortHandByRule(_playerHand.Items, _sevenSevenSevenSorting);
            _playerHand.Replace(sortHandBySevenSevenSeven);
        }
        
        [Button]
        public void SmartOrder()
        {
            if(_playerHand.Items == null || _playerHand.Items.Count == 0) return;

            var sortHandBySmart = _cardSortingService.SortHandByRule(_playerHand.Items, _smartSorting);
            _playerHand.Replace(sortHandBySmart);
        }

        public ReactiveList<CardController> GetHand()
        {
            return _playerHand;
        }
        
        // todo: add OnDestroy
    }
}