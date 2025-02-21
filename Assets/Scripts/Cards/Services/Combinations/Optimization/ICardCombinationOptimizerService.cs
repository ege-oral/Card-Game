using System.Collections.Generic;
using Cards.Data;
using Cards.View;

namespace Cards.Services.Combinations.Optimization
{
    public interface ICardCombinationOptimizerService
    {
        List<CardData> FindBestCombination(IReadOnlyList<CardData> hand, List<List<CardData>> validCombinations);
    }
}