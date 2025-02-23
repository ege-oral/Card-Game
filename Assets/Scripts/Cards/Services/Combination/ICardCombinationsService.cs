using System.Collections.Generic;
using Cards.Data;

namespace Cards.Services.Combination
{
    public interface ICardCombinationsService
    {
        List<List<CardData>> GenerateAllCombinations(IReadOnlyList<CardData> hand);
    }
}