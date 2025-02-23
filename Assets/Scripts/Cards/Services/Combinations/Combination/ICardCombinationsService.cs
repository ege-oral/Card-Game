using System.Collections.Generic;
using Cards.Data;

namespace Cards.Services.Combinations.Combination
{
    public interface ICardCombinationsService
    {
        List<List<CardData>> GenerateAllCombinations(IReadOnlyList<CardData> hand);
    }
}