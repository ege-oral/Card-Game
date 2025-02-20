using System.Collections.Generic;
using BezierCurve;
using Cards.Services;
using Cards.View.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Cards.View
{
    public class CardAnimationController : MonoBehaviour
    {
        [InlineEditor]
        [SerializeField] private CardAnimationControllerSo cardAnimationControllerSo;
        
        private ICardSortOrderService _cardSortOrderService;

        [Inject]
        public void Construct(ICardSortOrderService cardSortOrderService)
        {
            _cardSortOrderService = cardSortOrderService;
        }
        
        public async UniTask PlayDrawAnimation(CardController card, int handSize, int maxHandSize)
        {
            var t = (float)(handSize - 1) / Mathf.Max(1, maxHandSize - 1); // Ensures t starts from 0
            var handPosition = BezierUtility.GetPoint(cardAnimationControllerSo.startPoint,
                cardAnimationControllerSo.controlPoint, cardAnimationControllerSo.endPoint, t);
            var getNextSortingOrder = _cardSortOrderService.GetNextSortingOrder();
            var zRotation = Mathf.Lerp(cardAnimationControllerSo.zRotationRange, -cardAnimationControllerSo.zRotationRange, t);
            card.UpdateSorting(getNextSortingOrder);
            
            var sequence = DOTween.Sequence();
            sequence.Join(card.transform.DOMove(cardAnimationControllerSo.controlPoint, cardAnimationControllerSo.drawSpeed));
            sequence.Join(card.transform.DORotate(new Vector3(0f, 0f, 0f), cardAnimationControllerSo.drawSpeed));
            await sequence.AsyncWaitForCompletion();
            
            sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOMove(handPosition, cardAnimationControllerSo.drawSpeed));
            sequence.Join(card.transform.DORotate(new Vector3(0f, 0f, zRotation), cardAnimationControllerSo.drawSpeed));
        }
        
        public void ReArrangeHand(List<CardController> hand)
        {
            for (var i = 0; i < hand.Count; i++)
            {
                var card = hand[i];
                card.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                var t = (float)i / Mathf.Max(1, hand.Count - 1);  
                var handPosition = BezierUtility.GetPoint(cardAnimationControllerSo.startPoint,
                    cardAnimationControllerSo.controlPoint, cardAnimationControllerSo.endPoint, t);
                var getNextSortingOrder = _cardSortOrderService.GetNextSortingOrder();
                var zRotation = Mathf.Lerp(cardAnimationControllerSo.zRotationRange, -cardAnimationControllerSo.zRotationRange, t);
                card.UpdateSorting(getNextSortingOrder);
                var sequence = DOTween.Sequence();
                sequence.Append(card.transform.DOMove(handPosition, cardAnimationControllerSo.drawSpeed));
                sequence.Join(card.transform.DORotate(new Vector3(0f, 0f, zRotation), cardAnimationControllerSo.drawSpeed));

            }
            
        }
    }
}