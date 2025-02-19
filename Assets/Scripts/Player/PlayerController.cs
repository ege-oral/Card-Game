using System;
using System.Collections.Generic;
using BezierCurve;
using Cards;
using Cards.Services;
using Cards.Utils;
using Cysharp.Threading.Tasks;
using Deck;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Bezier Control Points")]
        [SerializeField] private Transform startPoint;   // The starting position (Deck area)
        [SerializeField] private Transform controlPoint; // The Bezier curve control point
        [SerializeField] private Transform endPoint;     // The final hand position

        [Header("Card Settings")]
        [SerializeField] private float drawSpeed = 0.5f;
        [SerializeField] private int maxHandSize = 11; // Maximum cards in hand
        [SerializeField] private float zRotationRange = 20;

        private DeckManager _deckManager;
        private ICardSortOrderService _cardSortOrderService;
        private SignalBus _signalBus;
        private readonly CardRankComparer _cardRankComparer = new();

        [SerializeField] private Transform playerHand;
        private List<CardController> _hand = new();
        
        private readonly List<CardController> _priorityCards = new();
        private readonly List<CardController> _leftOverCards = new();
        private readonly List<CardController> _cardSequence = new();

        [Inject]
        public void Construct(DeckManager deckManager, ICardSortOrderService cardSortOrderService, SignalBus signalBus)
        {
            _deckManager = deckManager;
            _cardSortOrderService = cardSortOrderService;
            _signalBus = signalBus;
        }

        [Button]
        public async void DrawCards()
        {
            try
            {
                if (_hand.Count >= maxHandSize) return;
                if (_deckManager.TryDrawCard(out var card) == false) return;
            
                AddItToHand(card);
                await PlayDrawAnimation(card);
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
                    if (_hand.Count >= maxHandSize) return;
                    if (_deckManager.TryDrawCard(out var card) == false) return;
                
                    AddItToHand(card);
                    await PlayDrawAnimation(card);
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
                if (_hand.Count >= maxHandSize) return;
                if (_deckManager.TryDrawCard(out var card) == false) return;
                
                AddItToHand(card);
                PlayDrawAnimation(card);
            }
        }
        
        [Button]
        public void OneTwoThreeOrder()
        {
            _priorityCards.Clear();
            _leftOverCards.Clear();
            _cardSequence.Clear();
            
            var cardSuitsToController = GroupCardsBySuit(_hand);

            foreach (var cardControllers in cardSuitsToController.Values)
            {
                if (cardControllers.Count < 3)
                {
                    _leftOverCards.AddRange(cardControllers);
                    continue;
                }

                cardControllers.Sort(_cardRankComparer); // Sort cards within the same suit by rank

                foreach (var card in cardControllers)
                {
                    if (_cardSequence.Count > 0 && card.CardData.Rank != _cardSequence[^1].CardData.Rank + 1)
                    {
                        StoreSequence(); // Store the previous sequence before starting a new one
                        _cardSequence.Clear();
                    }
                    _cardSequence.Add(card);
                }

                StoreSequence(); // Store the last sequence after the loop finishes
            }

            // Merge all valid sequences and leftovers
            _priorityCards.AddRange(_leftOverCards);
            _hand = _priorityCards;
            ReArrangeHand();
        }

        private void ReArrangeHand()
        {
            for (var i = 0; i < _hand.Count; i++)
            {
                var card = _hand[i];
                card.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                var t = (float)(i + 1) / _hand.Count;
                var handPosition = BezierUtility.GetPoint(startPoint.position, controlPoint.position, endPoint.position, t);
                var getNextSortingOrder = _cardSortOrderService.GetNextSortingOrder();
                var zRotation = Mathf.Lerp(zRotationRange, -zRotationRange, t);
                card.UpdateSorting(getNextSortingOrder);
                var sequence = DOTween.Sequence();
                sequence.Append(card.transform.DOMove(handPosition, drawSpeed));
                sequence.Join(card.transform.DORotate(new Vector3(0f, 0f, zRotation), drawSpeed));

            }
            
        }
        
        private void AddItToHand(CardController card)
        {
            _hand.Add(card);
            card.transform.SetParent(playerHand);
        }
        
        private async UniTask PlayDrawAnimation(CardController card)
        {
            var t = (float)_hand.Count / maxHandSize;
            var handPosition = BezierUtility.GetPoint(startPoint.position, controlPoint.position, endPoint.position, t);
            var getNextSortingOrder = _cardSortOrderService.GetNextSortingOrder();
            var zRotation = Mathf.Lerp(zRotationRange, -zRotationRange, t);
            card.UpdateSorting(getNextSortingOrder);
            
            var sequence = DOTween.Sequence();
            sequence.Join(card.transform.DOMove(controlPoint.position, drawSpeed));
            sequence.Join(card.transform.DORotate(new Vector3(0f, 0f, 0f), drawSpeed));
            await sequence.AsyncWaitForCompletion();
            
            sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOMove(handPosition, drawSpeed));
            sequence.Join(card.transform.DORotate(new Vector3(0f, 0f, zRotation), drawSpeed));
        }
        
        private void StoreSequence()
        {
            if (_cardSequence.Count >= 3)
            {
                _priorityCards.AddRange(_cardSequence);
            }
            else
            {
                _leftOverCards.AddRange(_cardSequence);
            }
        }
        
        private Dictionary<CardSuit, List<CardController>> GroupCardsBySuit(List<CardController> hand)
        {
            var cardSuitsToController = new Dictionary<CardSuit, List<CardController>>();

            foreach (var card in hand)
            {
                if (cardSuitsToController.TryGetValue(card.CardData.Suit, out var cardControllers) == false)
                {
                    cardControllers = new List<CardController>();
                    cardSuitsToController[card.CardData.Suit] = cardControllers;
                }
                cardControllers.Add(card);
            }

            return cardSuitsToController;
        }
        
        [Button]
        public void OneTwoThreeOrder2()
        {
            _priorityCards.Clear();
            _leftOverCards.Clear();
            
            var cardSuitsToController = new Dictionary<CardSuit, List<CardController>>();

            foreach (var card in _hand)
            {
                if (cardSuitsToController.TryGetValue(card.CardData.Suit, out var suitGroup) == false)
                {
                    suitGroup = new List<CardController>();
                    cardSuitsToController[card.CardData.Suit] = suitGroup;
                }
                suitGroup.Add(card);
            }

            foreach (var suitGroup in cardSuitsToController.Values)
            {
                suitGroup.Sort(_cardRankComparer); // Sort cards within the same suit by rank
                
                _cardSequence.Clear();

                var isConsecutive = false;
                CardController currentCard;
                CardController nextCard = null;
                for (var i = 0; i < suitGroup.Count - 1; i++) 
                {
                    currentCard = suitGroup[i];
                    nextCard = suitGroup[i + 1];

                    isConsecutive = currentCard.CardData.Rank == nextCard.CardData.Rank - 1;

                    _cardSequence.Add(currentCard);
                    if (isConsecutive == false)
                    {
                        StoreSequence();
                        _cardSequence.Clear();
                    }
                }

                if (isConsecutive)
                {
                    _cardSequence.Add(nextCard);
                    StoreSequence();
                }
                else
                {
                    _leftOverCards.Add(nextCard);
                }
                
            }

            _priorityCards.AddRange(_leftOverCards);
            _hand = _priorityCards;
            ReArrangeHand();
        }
    }
}