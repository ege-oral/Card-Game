using Cards.Services;

namespace Cards.View.Services
{
    public class CardSortOrderService : ICardSortOrderService
    {
        private int _sortingOrderCounter;
        private const int IncreaseCount = 10;
        
        public int GetNextSortingOrder()
        {
            var currentOrder = _sortingOrderCounter;
            _sortingOrderCounter += IncreaseCount;
            return currentOrder;
        }
        
        public void ResetSortingOrder()
        {
            _sortingOrderCounter = 0;
        }
    }
}