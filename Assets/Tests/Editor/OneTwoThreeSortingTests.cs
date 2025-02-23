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
            var hand = CreateHand(new[] { (CardSuit.Hearts, 1), (CardSuit.Hearts, 2) });

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsConsecutiveCards_SortsThemCorrectly()
        {
            var hand = CreateHand(new[] { (CardSuit.Hearts, 3), (CardSuit.Hearts, 1), (CardSuit.Hearts, 2) });

            var expectedOrder = new List<int> { 1, 2, 3 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsMultipleSuitGroups_SortsEachSuitSeparately()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 3), (CardSuit.Hearts, 2), (CardSuit.Hearts, 1),
                (CardSuit.Spades, 5), (CardSuit.Spades, 4), (CardSuit.Spades, 6)
            });

            var expectedOrder = new List<int> { 1, 2, 3, 4, 5, 6 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(6, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsGaps_LeavesThemUnsorted()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 1), (CardSuit.Hearts, 3), (CardSuit.Hearts, 5),
                (CardSuit.Clubs, 2), (CardSuit.Clubs, 4), (CardSuit.Clubs, 6)
            });

            var expectedOrder = new List<int> { 1, 3, 5, 2, 4, 6 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(6, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandIsAlreadySorted_LeavesItUnchanged()
        {
            var hand = CreateHand(new[] { (CardSuit.Hearts, 1), (CardSuit.Hearts, 2), (CardSuit.Hearts, 3) });

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandHasMixedOrderedAndUnorderedSequences_SortsCorrectly()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 5), (CardSuit.Hearts, 1), (CardSuit.Hearts, 3),
                (CardSuit.Hearts, 2), (CardSuit.Hearts, 4)
            });

            var expectedOrder = new List<int> { 1, 2, 3, 4, 5 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(5, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandHasSingleSuitWithScatteredRanks_SortsCorrectly()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Clubs, 10), (CardSuit.Clubs, 5), (CardSuit.Clubs, 8),
                (CardSuit.Clubs, 6), (CardSuit.Clubs, 7)
            });

            var expectedOrder = new List<int> { 5, 6, 7, 8, 10 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(5, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandHasOnlyOneSuitWithGaps_LeavesItAsIs()
        {
            var hand = CreateHand(new[] { (CardSuit.Spades, 2), (CardSuit.Spades, 5), (CardSuit.Spades, 9) });

            var expectedOrder = new List<int> { 2, 5, 9 };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void OneTwoThreeSorting_SortHand_WhenHandContainsComplexMix_SortsCorrectly()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 7), (CardSuit.Hearts, 6), (CardSuit.Hearts, 8), (CardSuit.Hearts, 10),
                (CardSuit.Spades, 1), (CardSuit.Spades, 2), (CardSuit.Spades, 3), (CardSuit.Spades, 4),
                (CardSuit.Diamonds, 9), (CardSuit.Diamonds, 8), (CardSuit.Diamonds, 7), (CardSuit.Diamonds, 10),
                (CardSuit.Clubs, 5), (CardSuit.Clubs, 6), (CardSuit.Clubs, 4),
                (CardSuit.Clubs, 2), (CardSuit.Clubs, 3), (CardSuit.Clubs, 8)
            });

            var expectedOrder = new List<int>
            {
                6, 7, 8,  // Hearts (sorted)
                1, 2, 3, 4,  // Spades (sorted)
                7, 8, 9, 10,  // Diamonds (sorted)
                2, 3, 4, 5, 6,  // Clubs (sorted)
                10,  // Hearts (leftover)
                8  // Clubs (leftover)
            };

            var result = _oneTwoThreeSorting.SortHand(hand);

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }
        
        private List<CardData> CreateHand(IEnumerable<(CardSuit Suit, int Rank)> cards)
        {
            return cards.Select(c => new CardData(c.Suit, c.Rank, null, null)).ToList();
        }
    }
}