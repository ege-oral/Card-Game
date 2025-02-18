using System;
using System.Collections.Generic;
using BezierCurve;
using Cards;
using Cards.Services;
using Cysharp.Threading.Tasks;
using Deck;
using DG.Tweening;
using Sirenix.OdinInspector;
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

        private DeckManager _deckManager;
        private ICardSortOrderService _cardSortOrderService;

        [SerializeField] private Transform playerHand;
        private readonly List<CardController> _hand = new();

        [Inject]
        public void Construct(DeckManager deckManager, ICardSortOrderService cardSortOrderService)
        {
            _deckManager = deckManager;
            _cardSortOrderService = cardSortOrderService;
        }

        [Button]
        public async void DrawCards()
        {
            if (_hand.Count >= maxHandSize) return;
            if (_deckManager.TryDrawCard(out var card) == false) return;
            
            AddItToHand(card);
            await PlayDrawAnimation(card);
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
            var zRotation = Mathf.Lerp(10f, -10f, t);
            card.UpdateSorting(getNextSortingOrder);
            
            var sequence = DOTween.Sequence();
            sequence.Join(card.transform.DOMove(controlPoint.position, drawSpeed));
            sequence.Join(card.transform.DORotate(new Vector3(0f, 0f, 0f), drawSpeed));
            await sequence.AsyncWaitForCompletion();
            
            sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOMove(handPosition, drawSpeed));
            sequence.Join(card.transform.DORotate(new Vector3(0f, 0f, zRotation), drawSpeed));
        }
    }
}