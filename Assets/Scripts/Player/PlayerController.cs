using System;
using System.Collections.Generic;
using System.Linq;
using Buttons.Signals;
using Cards.Data;
using Cards.Services.Sorting.Base;
using Cards.Signals;
using Cards.View;
using Cysharp.Threading.Tasks;
using Deck;
using Input.Signals;
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
        [SerializeField] private CardAnimationController cardAnimationController;

        private readonly List<CardController> _playerHand = new();
        public IReadOnlyList<CardController> PlayerHand => _playerHand;

        private ISorting _oneTwoThreeSorting;
        private ISorting _sevenSevenSevenSorting;
        private ISorting _smartSorting;
        private const int MaxHandSize = 11;
        
        [Inject]
        public void Construct(
            DeckManager deckManager,
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

            SubscribeToSignals();
        }

        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<DrawCardsSignal>(DrawElevenCards);
            _signalBus.Subscribe<DrawSpecificCardsSignal>(DrawSpecificCards);
            _signalBus.Subscribe<OneTwoThreeOrderSignal>(OneTwoThreeOrder);
            _signalBus.Subscribe<SevenSevenSevenOrderSignal>(SevenSevenSevenOrder);
            _signalBus.Subscribe<SmartOrderSignal>(SmartOrder);
        }

        private void UnsubscribeFromSignals()
        {
            _signalBus.Unsubscribe<DrawCardsSignal>(DrawElevenCards);
            _signalBus.Unsubscribe<DrawSpecificCardsSignal>(DrawSpecificCards);
            _signalBus.Unsubscribe<OneTwoThreeOrderSignal>(OneTwoThreeOrder);
            _signalBus.Unsubscribe<SevenSevenSevenOrderSignal>(SevenSevenSevenOrder);
            _signalBus.Unsubscribe<SmartOrderSignal>(SmartOrder);
        }

        [Button]
        public async void DrawElevenCards()
        {
            try
            {
                if (_playerHand.Count >= MaxHandSize) return;
                
                for (var i = 0; i < MaxHandSize; i++)
                {
                    if (_deckManager.TryDrawCard(out var card) == false) return;
                    AddCardToHand(card);
                }
                
                await PlayDrawAnimation();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while drawing cards: {e.Message}");
            }
        }
        
        [Button]
        public async void DrawSpecificCards()
        {
            try
            {
                if (_playerHand.Count >= MaxHandSize) return;
                
                var cardTuples = new List<(CardSuit Suit, int Rank)>
                {
                    (CardSuit.Hearts, 1), (CardSuit.Spades, 2), (CardSuit.Diamonds, 5), (CardSuit.Hearts, 4), 
                    (CardSuit.Spades, 1), (CardSuit.Diamonds, 3), (CardSuit.Clubs, 4), (CardSuit.Spades, 4),
                    (CardSuit.Diamonds, 1), (CardSuit.Spades, 3), (CardSuit.Diamonds, 4)
                };

                foreach (var (suit, rank) in cardTuples)
                {
                    if (_deckManager.TryDrawSpecificCard(suit, rank, out var card))
                    {
                        AddCardToHand(card);
                    }
                }
                await PlayDrawAnimation();
                
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while drawing cards: {e.Message}");
            }
        }

        private async UniTask PlayDrawAnimation()
        {
            _signalBus.Fire<DisableInputSignal>();
            _signalBus.Fire<CardDrawAnimationStartedSignal>();
            
            var drawAnimationTasks = new List<UniTask>();
            for (var i = 0; i < _playerHand.Count; i++)
            {
                var card = _playerHand[i];
                // don't wait whole draw animation so add it to a list and continue to draw
                drawAnimationTasks.Add(cardAnimationController.PlayDrawAnimation(card, i + 1, MaxHandSize));
                await UniTask.Delay(TimeSpan.FromSeconds(cardAnimationController.GetCardDrawDelay()));
            }
            
            await UniTask.WhenAll(drawAnimationTasks);
            
            _signalBus.Fire<EnableInputSignal>();
            _signalBus.Fire<CardDrawAnimationFinishedSignal>();
        }

        private void AddCardToHand(CardController card)
        {
            _playerHand.Add(card);
            card.transform.SetParent(playerHand);
        }

        public void RemoveCardFromHand(CardController card)
        {
            if (_playerHand.Remove(card))
            {
                ReArrangeHand();
            }
        }

        public void InsertCardInHand(int index, CardController card)
        {
            if (index < 0 || index > _playerHand.Count)
            {
                index = _playerHand.Count; // Insert at end if out of bounds
            }

            _playerHand.Insert(index, card);
            ReArrangeHand();
        }
        
        public void ResetPlayerHand()
        {
            // Return all cards to the deck
            foreach (var card in _playerHand)
            {
                _deckManager.ReturnCardToDeck(card);
            }

            _playerHand.Clear();
        }
        
        private async void ReArrangeHand()
        {
            try
            {
                _signalBus.Fire<HandReArrangeAnimationStartedSignal>();
                await cardAnimationController.ReArrangeHand(_playerHand);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while rearranging hand: {e.Message}");
            }
            finally
            {
                _signalBus.Fire<HandReArrangeAnimationFinishedSignal>();
            }
        }

        [Button]
        public void OneTwoThreeOrder() => SortHand(_oneTwoThreeSorting);

        [Button]
        public void SevenSevenSevenOrder() => SortHand(_sevenSevenSevenSorting);

        [Button]
        public void SmartOrder() => SortHand(_smartSorting);

        private void SortHand(ISorting sortingAlgorithm)
        {
            if (_playerHand.Count == 0) return;

            // Get sorted order of CardData
            var sortedHand = _cardSortingService.SortHandByRule(_playerHand.Select(x => x.CardData).ToList(), sortingAlgorithm);

            // Reorder existing _playerHand list to match the sorted order
            _playerHand.Sort((a, b) => 
                sortedHand.IndexOf(a.CardData).CompareTo(sortedHand.IndexOf(b.CardData)));

            ReArrangeHand();
        }

        private void OnDestroy()
        {
            UnsubscribeFromSignals();
        }
    }
}