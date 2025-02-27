using System.Collections.Generic;
using BezierCurve;
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
        [InlineEditor] [SerializeField] private CardAnimationControllerSo cardAnimationControllerSo;

        private ICardSortOrderService _cardSortOrderService;
        private const string PlayerHandSortingLayerName = "PlayerHand";

        [Inject]
        public void Construct(ICardSortOrderService cardSortOrderService)
        {
            _cardSortOrderService = cardSortOrderService;
        }

        public async UniTask PlayDrawAnimation(CardController card, int currentHandSize, int maxHandSize)
        {
            var t = CalculateNormalizedPosition(currentHandSize, maxHandSize);
            var handPosition = GetHandPosition(t);
            var zRotation = cardAnimationControllerSo.GetRotationAngle(t);

            card.UpdateSorting(_cardSortOrderService.GetNextSortingOrder(), PlayerHandSortingLayerName);

            // First phase: Move to control point
            await AnimateCard(card, cardAnimationControllerSo.controlPoint, Vector3.zero);

            // Second phase: Move to hand position
            await AnimateCard(card, handPosition, new Vector3(0f, 0f, zRotation));
        }

        public async UniTask ReArrangeHand(IReadOnlyList<CardController> hand)
        {
            var reArrangeHandAnimationTasks = new List<UniTask>();

            for (var i = 0; i < hand.Count; i++)
            {
                var card = hand[i];
                var t = CalculateNormalizedPosition(i + 1, hand.Count);
                var handPosition = GetHandPosition(t);
                var zRotation = cardAnimationControllerSo.GetRotationAngle(t);

                card.UpdateSorting(_cardSortOrderService.GetNextSortingOrder(), PlayerHandSortingLayerName);

                // Properly pass the cancellation token
                reArrangeHandAnimationTasks.Add(
                    AnimateCard(card, handPosition, new Vector3(0f, 0f, zRotation))
                );
            }

            await UniTask.WhenAll(reArrangeHandAnimationTasks);
        }

        public float GetCardDrawDelay()
        {
            return cardAnimationControllerSo.cardDrawDelay;
        }

        private UniTask AnimateCard(CardController card, Vector3 position, Vector3 rotation)
        {
            return DOTween.Sequence()
                .Join(card.transform.DOMove(position, cardAnimationControllerSo.drawSpeed))
                .Join(card.transform.DORotate(rotation, cardAnimationControllerSo.drawSpeed))
                .AsyncWaitForCompletion().AsUniTask();
        }

        private float CalculateNormalizedPosition(int currentHandSize, int maxHandSize)
        {
            return (float)(currentHandSize - 1) / Mathf.Max(1, maxHandSize - 1); // Ensures t starts from 0
        }

        private Vector3 GetHandPosition(float t)
        {
            return BezierUtility.GetPoint(
                cardAnimationControllerSo.startPoint,
                cardAnimationControllerSo.controlPoint,
                cardAnimationControllerSo.endPoint,
                t
            );
        }
    }
}