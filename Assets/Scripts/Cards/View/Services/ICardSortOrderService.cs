namespace Cards.View.Services
{
    public interface ICardSortOrderService
    {
        int GetNextSortingOrder();
        void ResetSortingOrder();
    }
}