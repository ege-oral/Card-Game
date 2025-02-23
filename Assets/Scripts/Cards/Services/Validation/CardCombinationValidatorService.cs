using System.Collections.Generic;
using System.Linq;
using Cards.Data;

namespace Cards.Services.Validation
{
    public class CardCombinationValidatorService : ICardCombinationValidatorService
    {
        private readonly List<List<CardData>> _allValidCombinations = new();
        private readonly List<List<CardData>> _currentCombination = new();
        private readonly HashSet<CardData> _usedCards = new();
        
        public List<List<CardData>> GetValidCombinations(List<List<CardData>> possibilities)
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

            bool IsValidCombination(List<CardData> combination)
            {
                return combination.All(card => _usedCards.Contains(card) == false);
            }

            void AddCombination(List<CardData> combination)
            {
                _currentCombination.Add(combination);
                foreach (var card in combination) _usedCards.Add(card);
            }

            void RemoveCombination(List<CardData> combination)
            {
                foreach (var card in combination) _usedCards.Remove(card);
                _currentCombination.RemoveAt(_currentCombination.Count - 1);
            }
        }
    }
}