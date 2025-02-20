using System.Collections.Generic;
using Cards.View;

namespace Cards.Services.Combination
{
    public interface ICardCombinationsService
    {
        List<List<CardController>> GenerateAllCombinations(IReadOnlyList<CardController> hand);
    }
}