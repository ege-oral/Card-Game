using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Services.Combination;
using Cards.Services.Optimization;
using Cards.Services.Sorting.Base;
using Cards.Services.Sorting.Strategies;
using Cards.Services.Validation;
using Cards.Utils;
using NUnit.Framework;

namespace Tests.Editor
{
    [TestFixture]
    public class SmartSortingTests
    {
        private ISorting _smartSorting;
        private ICardCombinationsService _combinationService;
        private ICardCombinationValidatorService _validatorService;
        private ICardCombinationOptimizerService _optimizerService;
        private CardRankComparer _cardRankComparer;

        [SetUp]
        public void SetUp()
        {
            _cardRankComparer = new CardRankComparer();
            _combinationService = new CardCombinationsService(_cardRankComparer);
            _validatorService = new CardCombinationValidatorService();
            _optimizerService = new CardCombinationOptimizerService();

            _smartSorting = new SmartSorting(
                _combinationService,
                _validatorService,
                _optimizerService
            );
        }

        [Test]
        public void SmartSorting_SortHand_WhenHandIsEmpty_ReturnsNull()
        {
            var result = _smartSorting.SortHand(new List<CardData>());
            Assert.IsNull(result);
        }

        [Test]
        public void SmartSorting_SortHand_WhenNoValidCombinations_ReturnsOriginalHand()
        {
            var hand = CreateHand(new[] { (CardSuit.Hearts, 1), (CardSuit.Spades, 3) });

            var result = _smartSorting.SortHand(hand);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SmartSorting_SortHand_WhenValidCombinationsExist_ReturnsOptimizedHand()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 1), (CardSuit.Hearts, 2), (CardSuit.Hearts, 3)
            });

            var result = _smartSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }
        
        [Test]
        public void SmartSorting_SortHand_WhenHandContainsValidGroupsUseHearts3ForOtherGroup_ReturnsSortedHand()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 1), (CardSuit.Hearts, 2), (CardSuit.Hearts, 3),
                (CardSuit.Spades, 7), (CardSuit.Hearts, 7), (CardSuit.Clubs, 7),
                (CardSuit.Spades, 3), (CardSuit.Clubs, 3)
            });

            var result = _smartSorting.SortHand(hand);
            
            // (CardSuit.Hearts, 1), (CardSuit.Hearts, 2)
            var expectedLeftOverCards = result.Skip(Math.Max(0, result.Count - 2)).ToList();
            var expectedMinValue = expectedLeftOverCards.Sum(x => x.Value);
            
            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            Assert.AreEqual(expectedMinValue, 3);
        }

        [Test]
        public void SmartSorting_SortHand_WhenMultipleValidCombinationsExist_ChoosesBestOne()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Spades, 7), (CardSuit.Spades, 8), (CardSuit.Spades, 9),
                (CardSuit.Clubs, 3), (CardSuit.Diamonds, 3), (CardSuit.Hearts, 3),
                (CardSuit.Clubs, 1), (CardSuit.Clubs, 8), (CardSuit.Clubs, 2),
                (CardSuit.Spades, 4), (CardSuit.Diamonds, 6)
            });

            var result = _smartSorting.SortHand(hand);
            
            // (CardSuit.Clubs, 1), (CardSuit.Clubs, 8), (CardSuit.Clubs, 2), (CardSuit.Spades, 4), (CardSuit.Diamonds, 6)
            var expectedLeftOverCards = result.Skip(Math.Max(0, result.Count - 5)).ToList();
            var expectedMinValue = expectedLeftOverCards.Sum(x => x.Value);

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            Assert.AreEqual(expectedMinValue, 21);
        }
        
        [Test]
        public void SmartSorting_SortHand_WhenHandContainsThreeValidGroupsAndLeftovers_SortsCorrectly()
        {
            
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 1), (CardSuit.Spades, 2), (CardSuit.Diamonds, 5), (CardSuit.Hearts, 4),
                (CardSuit.Spades, 1), (CardSuit.Diamonds, 3), (CardSuit.Clubs, 4), (CardSuit.Spades, 4),
                (CardSuit.Diamonds, 1), (CardSuit.Spades, 3), (CardSuit.Diamonds, 4)
            });

            var result = _smartSorting.SortHand(hand);
            
            // (CardSuit.Hearts, 1) (CardSuit.Diamonds, 1)
            var expectedLeftOverCards = result.Skip(Math.Max(0, result.Count - 2)).ToList();
            var expectedMinValue = expectedLeftOverCards.Sum(x => x.Value);
            
            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            Assert.AreEqual(expectedMinValue, 2);
        }
        
        [Test]
        public void SmartSorting_SortHand_WhenHandContainsTwoValidGroupsAndLeftovers_SortsCorrectly()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Spades, 10), (CardSuit.Spades, 11), (CardSuit.Spades, 12), (CardSuit.Spades, 13),
                (CardSuit.Diamonds, 11),  (CardSuit.Diamonds, 9), (CardSuit.Diamonds, 10), (CardSuit.Clubs, 10), 
            });

            var result = _smartSorting.SortHand(hand);
            
            // (CardSuit.Clubs, 10)
            var expectedLeftOverCards = result.Skip(Math.Max(0, result.Count - 1)).ToList();
            var expectedMinValue = expectedLeftOverCards.Sum(x => x.Value);
            
            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            Assert.AreEqual(expectedMinValue, 10);
        }
        
        [Test]
        public void SmartSorting_SortHand_WhenHandContainsOneValidGroupsAndLeftovers_SortsCorrectly()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Spades, 1), (CardSuit.Spades, 2), (CardSuit.Spades, 3), (CardSuit.Spades, 4),
                (CardSuit.Diamonds, 3), (CardSuit.Hearts, 3)
            });

            var result = _smartSorting.SortHand(hand);
            
            // (CardSuit.Diamonds, 3), (CardSuit.Hearts, 3)
            var expectedLeftOverCards = result.Skip(Math.Max(0, result.Count - 2)).ToList();
            var expectedMinValue = expectedLeftOverCards.Sum(x => x.Value);
            
            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            Assert.AreEqual(expectedMinValue, 6);
        }
        
        [Test]
        public void SmartSorting_SortHand_WhenHandContainsTwoValidGroupsAndLeftovers_SortsCorrectly_Second()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Spades, 1), (CardSuit.Spades, 2), (CardSuit.Spades, 3), (CardSuit.Spades, 4),
                (CardSuit.Diamonds, 4), (CardSuit.Hearts, 4), (CardSuit.Hearts, 1),
            });

            var result = _smartSorting.SortHand(hand);
            
            // (CardSuit.Hearts, 1),
            var expectedLeftOverCards = result.Skip(Math.Max(0, result.Count - 1)).ToList();
            var expectedMinValue = expectedLeftOverCards.Sum(x => x.Value);
            
            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            Assert.AreEqual(expectedMinValue, 1);
        }

        private List<CardData> CreateHand(IEnumerable<(CardSuit Suit, int Rank)> cards)
        {
            return cards.Select(c => new CardData(c.Suit, c.Rank, null, null)).ToList();
        }
    }
}