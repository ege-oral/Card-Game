using System.Collections.Generic;
using Cards.Data;

namespace Cards.Services.Validation
{
    public interface ICardCombinationValidatorService
    {
        List<List<CardData>> GetValidCombinations(List<List<CardData>> possibilities);
    }
}