using System.Collections.Generic;
using System.Linq;
using Cards.Utils;
using Cards.View;
using UnityEngine;

namespace Cards.Services
{
    public class SmartSorting : ISorting
    {
        private readonly CardRankComparer _cardRankComparer;
        private readonly List<List<CardController>> _possibilities = new();

        public SmartSorting(CardRankComparer cardRankComparer)
        {
            _cardRankComparer = cardRankComparer;
        }

        public List<CardController> SortHand(IReadOnlyList<CardController> hand)
        {
            var possibility = new List<List<CardController>>();

            CalculateAllSevenSevenSevenCombinations(hand, possibility);
            CalculateAllOneTwoThreeCombinations(hand, possibility);

            var validCombinations = GetAllValidCombinations();

            var bestCombination = FindBestCombination(hand, validCombinations);
            var leftOverCards = GetLeftoverCards(hand, bestCombination);

            return bestCombination.Concat(leftOverCards).ToList();
        }
        
        private void CalculateAllSevenSevenSevenCombinations(IReadOnlyList<CardController> hand,
            List<List<CardController>> possibility)
        {
            var cardRanksToController = CardUtil.GroupCardsByRank(hand);

            foreach (var (_, cards) in cardRanksToController)
            {
                possibility.Clear();
                if (cards.Count < 3) continue; // Need at least 3 cards

                if (cards.Count == 4)
                {
                    possibility.Add(cards);
                    possibility.AddRange(CardUtil.GetCombinations(cards, 3));
                }
                else
                {
                    possibility.Add(cards);
                }

                _possibilities.AddRange(possibility);
            }
        }

        private void CalculateAllOneTwoThreeCombinations(IReadOnlyList<CardController> hand,
            List<List<CardController>> possibility)
        {
            var cardSuitsToController = CardUtil.GroupCardsBySuit(hand);

            foreach (var (_, suitCards) in cardSuitsToController)
            {
                if (suitCards.Count < 3) continue; // Need at least 3 cards

                suitCards.Sort(_cardRankComparer); // Sort by rank

                // Find all possible consecutive groups
                for (var i = 0; i <= suitCards.Count - 3; i++)
                {
                    for (var j = i + 2; j < suitCards.Count; j++)
                    {
                        if (suitCards[j].CardData.Rank == suitCards[j - 1].CardData.Rank + 1 &&
                            suitCards[j - 1].CardData.Rank == suitCards[j - 2].CardData.Rank + 1)
                        {
                            possibility.Add(suitCards.GetRange(i, j - i + 1)); // Add valid sequence
                        }
                        else break; // Stop if the sequence is broken
                    }
                }
            }

            _possibilities.AddRange(possibility);
        }

        private List<List<CardController>> GetAllValidCombinations()
        {
            var allValidCombinations = new List<List<CardController>>();
            var usedCards = new HashSet<CardController>();
            var currentCombination = new List<List<CardController>>();

            SearchValidCombinations(0);
            return allValidCombinations;

            void SearchValidCombinations(int index)
            {
                while (true)
                {
                    if (index >= _possibilities.Count)
                    {
                        if (currentCombination.Count > 0)
                        {
                            var result = currentCombination.SelectMany(combination => combination).ToList();
                            allValidCombinations.Add(result);
                        }

                        return;
                    }

                    var combination = _possibilities[index];

                    if (IsValidCombination(combination))
                    {
                        AddCombination(combination);
                        SearchValidCombinations(index + 1);
                        RemoveCombination(combination);
                    }

                    index += 1;
                }
            }

            bool IsValidCombination(List<CardController> combination)
            {
                foreach (var card in combination)
                {
                    if (usedCards.Contains(card))
                        return false;
                }

                return true;
            }

            void AddCombination(List<CardController> combination)
            {
                currentCombination.Add(combination);
                foreach (var card in combination)
                {
                    usedCards.Add(card);
                }
            }

            void RemoveCombination(List<CardController> combination)
            {
                foreach (var card in combination)
                {
                    usedCards.Remove(card);
                }

                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        private List<CardController> FindBestCombination(IReadOnlyList<CardController> hand,
            List<List<CardController>> validCombinations)
        {
            var bestCombination = new List<CardController>();
            var lowestValue = int.MaxValue;

            foreach (var combination in validCombinations)
            {
                var remainingValue = EvaluateCombination(hand, combination);
                if (remainingValue < lowestValue)
                {
                    lowestValue = remainingValue;
                    bestCombination = combination;
                }
            }

            Debug.Log($"Lowest remaining value: {lowestValue}");
            return bestCombination;
        }

        private int EvaluateCombination(IReadOnlyList<CardController> hand, List<CardController> combination)
        {
            var tempHand = hand.ToList();
            foreach (var card in combination)
            {
                tempHand.Remove(card);
            }

            return tempHand.Sum(card => card.CardData.Value);
        }

        private List<CardController> GetLeftoverCards(IReadOnlyList<CardController> hand,
            List<CardController> bestCombination)
        {
            return hand.Where(card => !bestCombination.Contains(card)).ToList();
        }
    }
}