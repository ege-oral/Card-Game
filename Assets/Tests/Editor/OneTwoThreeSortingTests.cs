using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Services.Sorting.Base;
using Cards.Services.Sorting.Strategies;
using Cards.Utils;
using NUnit.Framework;

namespace Tests.Editor
{
    [TestFixture]
    public class OneTwoThreeSortingTests
    {
        private ISorting _oneTwoThreeSorting;
        private CardRankComparer _comparer;

        [SetUp]
        public void SetUp()
        {
            _comparer = new CardRankComparer();
            _oneTwoThreeSorting = new OneTwoThreeSorting(_comparer);
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandIsEmpty_ReturnsNull()
        {
            var result = _oneTwoThreeSorting.SortHand(new List<CardData>());
            Assert.IsNull(result);
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsLessThanThreeCards_ReturnsSameHand()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 1, null, null),
                new(CardSuit.Hearts, 2, null, null)
            };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsConsecutiveCards_SortsThemCorrectly()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 3, null, null),
                new(CardSuit.Hearts, 1, null, null),
                new(CardSuit.Hearts, 2, null, null)
            };

            var expectedOrder = new List<int> { 1, 2, 3 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsMultipleSuitGroups_SortsEachSuitSeparately()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 3, null, null),
                new(CardSuit.Hearts, 2, null, null),
                new(CardSuit.Hearts, 1, null, null),

                new(CardSuit.Spades, 5, null, null),
                new(CardSuit.Spades, 4, null, null),
                new(CardSuit.Spades, 6, null, null)
            };

            var expectedOrder = new List<int> { 1, 2, 3, 4, 5, 6 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(6, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsGaps_LeavesThemUnsorted()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 1, null, null),
                new(CardSuit.Hearts, 3, null, null),
                new(CardSuit.Hearts, 5, null, null),

                new(CardSuit.Clubs, 2, null, null),
                new(CardSuit.Clubs, 4, null, null),
                new(CardSuit.Clubs, 6, null, null)
            };

            var expectedOrder = new List<int> { 1, 3, 5, 2, 4, 6 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(6, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandIsAlreadySorted_LeavesItUnchanged()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 1, null, null),
                new(CardSuit.Hearts, 2, null, null),
                new(CardSuit.Hearts, 3, null, null)
            };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandHasMixedOrderedAndUnorderedSequences_SortsCorrectly()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 5, null, null),
                new(CardSuit.Hearts, 1, null, null),
                new(CardSuit.Hearts, 3, null, null),
                new(CardSuit.Hearts, 2, null, null),
                new(CardSuit.Hearts, 4, null, null)
            };

            var expectedOrder = new List<int> { 1, 2, 3, 4, 5 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(5, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandHasSingleSuitWithScatteredRanks_SortsCorrectly()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Clubs, 10, null, null),
                new(CardSuit.Clubs, 5, null, null),
                new(CardSuit.Clubs, 8, null, null),
                new(CardSuit.Clubs, 6, null, null),
                new(CardSuit.Clubs, 7, null, null)
            };

            var expectedOrder = new List<int> { 5, 6, 7, 8, 10 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(5, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandHasOnlyOneSuitWithGaps_LeavesItAsIs()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Spades, 2, null, null),
                new(CardSuit.Spades, 5, null, null),
                new(CardSuit.Spades, 9, null, null)
            };

            var expectedOrder = new List<int> { 2, 5, 9 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }
        
        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsComplexMix_SortsCorrectly()
        {
            var hand = new List<CardData>
            {
                // Mixed and unordered sequences from different suits
                new(CardSuit.Hearts, 7, null, null),
                new(CardSuit.Hearts, 6, null, null),
                new(CardSuit.Hearts, 8, null, null),
                new(CardSuit.Hearts, 10, null, null),
        
                new(CardSuit.Spades, 1, null, null),
                new(CardSuit.Spades, 2, null, null),
                new(CardSuit.Spades, 3, null, null),
                new(CardSuit.Spades, 4, null, null),
        
                new(CardSuit.Diamonds, 9, null, null),
                new(CardSuit.Diamonds, 8, null, null),
                new(CardSuit.Diamonds, 7, null, null),
                new(CardSuit.Diamonds, 10, null, null),
        
                new(CardSuit.Clubs, 5, null, null),
                new(CardSuit.Clubs, 6, null, null),
                new(CardSuit.Clubs, 4, null, null),
        
                new(CardSuit.Clubs, 2, null, null),
                new(CardSuit.Clubs, 3, null, null),
                new(CardSuit.Clubs, 8, null, null),
            };

            var expectedOrder = new List<int>
            {
                // Sorted sequences
                6, 7, 8, // Hearts (sorted)
                1, 2, 3, 4, // Spades (sorted)
                7, 8, 9, 10, // Diamonds (sorted)
                2, 3, 4, 5, 6, // Clubs (sorted)

                // Leftover unordered cards
                10, // Hearts 
                8 // Clubs
            };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }
    }
}