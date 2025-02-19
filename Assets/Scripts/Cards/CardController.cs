using UnityEngine;

namespace Cards
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer frontRenderer;
        [SerializeField] private SpriteRenderer backRenderer;

        public CardData CardData;
        
        public void Initialize(CardData cardData)
        {
            CardData = cardData;
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