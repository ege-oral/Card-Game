namespace Cards.Services
{
    public interface ICardSortOrderService
    {
        int GetNextSortingOrder();
        void ResetSortingOrder();
    }
}