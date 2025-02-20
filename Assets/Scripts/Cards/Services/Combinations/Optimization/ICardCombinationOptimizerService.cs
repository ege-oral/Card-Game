using System.Collections.Generic;
using Cards.View;

namespace Cards.Services.Combinations.Optimization
{
    public interface ICardCombinationOptimizerService
    {
        List<CardController> FindBestCombination(IReadOnlyList<CardController> hand, List<List<CardController>> validCombinations);
    }
}