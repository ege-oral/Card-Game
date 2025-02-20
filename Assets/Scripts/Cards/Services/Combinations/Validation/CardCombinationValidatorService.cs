using System.Collections.Generic;
using System.Linq;
using Cards.View;

namespace Cards.Services.Combinations.Validation
{
    public class CardCombinationValidatorService : ICardCombinationValidatorService
    {
        private readonly List<List<CardController>> _allValidCombinations = new();
        private readonly List<List<CardController>> _currentCombination = new();
        private readonly HashSet<CardController> _usedCards = new();
        
        public List<List<CardController>> GetValidCombinations(List<List<CardController>> possibilities)
        {
            _allValidCombinations.Clear();
            _usedCards.Clear();
            _currentCombination.Clear();
            
            SearchValidCombinations(0);
            return _allValidCombinations;

            void SearchValidCombinations(int index)
            {
                while (true)
                {
                    if (index >= possibilities.Count)
                    {
                        if (_currentCombination.Count > 0)
                        {
                            _allValidCombinations.Add(_currentCombination.SelectMany(combination => combination).ToList());
                        }

                        return;
                    }

                    var combination = possibilities[index];

                    if (IsValidCombination(combination))
                    {
                        AddCombination(combination);
                        SearchValidCombinations(index + 1);
                        RemoveCombination(combination);
                    }

                    // Try skipping this combination
                    index += 1;
                }
            }

            bool IsValidCombination(List<CardController> combination)
            {
                return combination.All(card => _usedCards.Contains(card) == false);
            }

            void AddCombination(List<CardController> combination)
            {
                _currentCombination.Add(combination);
                foreach (var card in combination) _usedCards.Add(card);
            }

            void RemoveCombination(List<CardController> combination)
            {
                foreach (var card in combination) _usedCards.Remove(card);
                _currentCombination.RemoveAt(_currentCombination.Count - 1);
            }
        }
    }
}