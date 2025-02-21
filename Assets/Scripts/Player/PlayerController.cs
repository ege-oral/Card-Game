using System;
using Buttons.Signals;
using Cards.Services.Sorting.Base;
using Cards.View;
using Common;
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

        private readonly ReactiveList<CardController> _playerHand = new();

        private ISorting _oneTwoThreeSorting;
        private ISorting _sevenSevenSevenSorting;
        private ISorting _smartSorting;
        private const int MaxHandSize = 11; // Maximum cards in hand

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
            _signalBus.Subscribe<DrawCardsSignal>(DrawElevenCards);
            _signalBus.Subscribe<OneTwoThreeOrderSignal>(OneTwoThreeOrder);
            _signalBus.Subscribe<SevenSevenSevenOrderSignal>(SevenSevenSevenOrder);
            _signalBus.Subscribe<SmartOrderSignal>(SmartOrder);
            
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
                await cardAnimationController.PlayDrawAnimation(obj, _playerHand.Items.Count, MaxHandSize);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        [Button]
        public async void DrawElevenCards()
        {
            try
            {
                for (var i = 0; i < MaxHandSize; ++i)
                {
                    if (_playerHand.Items.Count >= MaxHandSize) return;
                    if (_deckManager.TryDrawCard(out var card) == false) return;
            
                    AddItToHand(card);
                    await UniTask.Delay(TimeSpan.FromSeconds(cardAnimationController.GetCardDrawDelay()));
                    if (_playerHand.Items.Count == MaxHandSize)
                    {
                        _signalBus.Fire<EnableInputSignal>();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
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

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<DrawCardsSignal>(DrawElevenCards);
            _signalBus.Unsubscribe<OneTwoThreeOrderSignal>(OneTwoThreeOrder);
            _signalBus.Unsubscribe<SevenSevenSevenOrderSignal>(SevenSevenSevenOrder);
            _signalBus.Unsubscribe<SmartOrderSignal>(SmartOrder);
            
            _playerHand.ItemAdded -= Draw;
            _playerHand.ItemInserted -= ReArrange;
            _playerHand.ListReplaced -= ReArrange;
            _playerHand.ItemRemoved -= ReArrange;
        }
    }
}