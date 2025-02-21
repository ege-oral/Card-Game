using System.Collections.Generic;
using Cards.Data;
using Cards.View;

namespace Cards.Services.Combination
{
    public interface ICardCombinationsService
    {
        List<List<CardData>> GenerateAllCombinations(IReadOnlyList<CardData> hand);
    }
}