using System.Collections.Generic;
using Cards.View;

namespace Cards.Services.Combinations.Validation
{
    public interface ICardCombinationValidatorService
    {
        List<List<CardController>> GetValidCombinations(List<List<CardController>> possibilities);
    }
}