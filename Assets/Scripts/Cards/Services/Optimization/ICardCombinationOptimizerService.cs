using System.Collections.Generic;
using Cards.Data;

namespace Cards.Services.Optimization
{
    public interface ICardCombinationOptimizerService
    {
        List<CardData> FindBestCombination(IReadOnlyList<CardData> hand, List<List<CardData>> validCombinations);
    }
}