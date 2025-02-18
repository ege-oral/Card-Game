using UnityEngine;
using Zenject;

namespace Cards
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer frontRenderer;
        [SerializeField] private SpriteRenderer backRenderer;

        private Card _card;
        private CardData _cardData;

        [Inject]
        public void Construct(Card card)
        {
            _card = card;
        }
        
        public void Initialize(CardData cardData)
        {
            _cardData = cardData;
            frontRenderer.sprite = cardData.FrontSprite;
            backRenderer.sprite = cardData.BackSprite;
        }

        public void UpdateSorting(int sortingOrder, string sortingLayerName = "Default")
        {
            frontRenderer.sortingOrder = sortingOrder;
            frontRenderer.sortingLayerName = sortingLayerName;
            
            backRenderer.sortingOrder = sortingOrder;
            backRenderer.sortingLayerName = sortingLayerName;
        }
    }
}