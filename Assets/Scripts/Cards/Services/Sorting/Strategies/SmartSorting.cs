using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Services.Combination;
using Cards.Services.Optimization;
using Cards.Services.Sorting.Base;
using Cards.Services.Validation;

namespace Cards.Services.Sorting.Strategies
{
    public class SmartSorting : ISorting
    {
        private readonly ICardCombinationsService _cardCombinationsService;
        private readonly ICardCombinationValidatorService _cardCombinationValidatorService;
        private readonly ICardCombinationOptimizerService _cardCombinationOptimizerService;
        
        private List<CardData> _priorityCards = new();
        private List<CardData> _leftOverCards = new();

        public SmartSorting(ICardCombinationsService cardCombinationsService,
            ICardCombinationValidatorService cardCombinationValidatorService,
            ICardCombinationOptimizerService cardCombinationOptimizerService)
        {
            _cardCombinationsService = cardCombinationsService;
            _cardCombinationValidatorService = cardCombinationValidatorService;
            _cardCombinationOptimizerService = cardCombinationOptimizerService;
        }

        /// <summary>
        /// Sorts the given hand of cards based on optimized valid combinations.
        /// This operation is computationally expensive and may cause visual spikes 
        /// if processing more than 30 cards at once. Use with caution to maintain performance.
        /// </summary>
        
        public List<CardData> SortHand(IReadOnlyList<CardData> hand)
        {
            if (hand == null || hand.Count == 0) return null;

            _priorityCards.Clear();
            _leftOverCards.Clear();
            
            var allCombinations = _cardCombinationsService.GenerateAllCombinations(hand);
            var validCombinations = _cardCombinationValidatorService.GetValidCombinations(allCombinations);
            _priorityCards = _cardCombinationOptimizerService.FindBestCombination(hand, validCombinations);
            _leftOverCards = GetLeftoverCards(hand, _priorityCards);

            return _priorityCards.Count == 0 ? hand.ToList() : _priorityCards.Concat(_leftOverCards).ToList();
        }

        private List<CardData> GetLeftoverCards(IReadOnlyList<CardData> hand, List<CardData> bestCombination)
        {
            foreach (var card in hand)
            {
                if(bestCombination.Contains(card)) continue;
                    
                _leftOverCards.Add(card);
            }

            return _leftOverCards;
        }
    }
}