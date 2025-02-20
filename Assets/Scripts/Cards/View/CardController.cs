using Cards.Data;
using UnityEngine;

namespace Cards.View
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer frontRenderer;
        [SerializeField] private SpriteRenderer backRenderer;

        public CardData CardData;

        private int _savedSortingOrder;
        private string _savedSortingLayerName;
        
        public void Initialize(CardData cardData)
        {
            CardData = cardData;
            frontRenderer.sprite = cardData.FrontSprite;
            backRenderer.sprite = cardData.BackSprite;
        }

        public void UpdateSorting(int sortingOrder, string sortingLayerName = "Default")
        {
            _savedSortingOrder = sortingOrder;
            _savedSortingLayerName = sortingLayerName;
            
            frontRenderer.sortingOrder = sortingOrder;
            frontRenderer.sortingLayerName = sortingLayerName;
            
            backRenderer.sortingOrder = sortingOrder;
            backRenderer.sortingLayerName = sortingLayerName;
        }
        
        public void Highlight(int sortingOrder, string sortingLayerName = "Default")
        {
            transform.localScale = Vector3.one * 1.1f;
            frontRenderer.sortingOrder = sortingOrder;
            frontRenderer.sortingLayerName = sortingLayerName;
            
            backRenderer.sortingOrder = sortingOrder;
            backRenderer.sortingLayerName = sortingLayerName;
        }

        public void DeHighlight()
        {
            transform.localScale = Vector3.one * 1.0f;
            frontRenderer.sortingOrder = _savedSortingOrder;
            frontRenderer.sortingLayerName = _savedSortingLayerName;
            
            backRenderer.sortingOrder = _savedSortingOrder;
            backRenderer.sortingLayerName = _savedSortingLayerName;
        }
    }
}