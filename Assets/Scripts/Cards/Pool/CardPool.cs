using System.Collections.Generic;
using Cards.View;
using Cards.Factory;
using UnityEngine;
using Zenject;

namespace Cards.Pool
{
    public class CardPool : MonoBehaviour
    {
        [SerializeField] private Transform poolParent;

        private CardControllerFactory _cardFactory;
        private readonly Queue<CardController> _cardPool = new();
        private const int InitialPoolSize = 52;

        [Inject]
        public void Construct(CardControllerFactory cardFactory)
        {
            _cardFactory = cardFactory;
            InitializePool();
        }

        private void InitializePool()
        {
            for (var i = 0; i < InitialPoolSize; i++)
            {
                var card = CreateNewCard();
                _cardPool.Enqueue(card);
            }
        }

        private CardController CreateNewCard()
        {
            var card = _cardFactory.Create();
            card.transform.SetParent(poolParent);
            card.gameObject.SetActive(false);
            return card;
        }

        public CardController GetCard()
        {
            if (_cardPool.Count == 0)
            {
                return CreateNewCard();
            }

            var card = _cardPool.Dequeue();
            card.gameObject.SetActive(true);
            return card;
        }

        public void ReturnCard(CardController card)
        {
            card.gameObject.SetActive(false);
            card.transform.SetParent(poolParent);
            _cardPool.Enqueue(card);
        }
    }
}