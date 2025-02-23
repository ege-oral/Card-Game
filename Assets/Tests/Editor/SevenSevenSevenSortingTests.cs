using System.Collections.Generic;
using System.Linq;
using Cards.Data;
using Cards.Services.Sorting.Base;
using Cards.Services.Sorting.Strategies;
using NUnit.Framework;

namespace Tests.Editor
{
    [TestFixture]
    public class SevenSevenSevenSortingTests
    {
        private ISorting _sevenSevenSevenSorting;

        [SetUp]
        public void SetUp()
        {
            _sevenSevenSevenSorting = new SevenSevenSevenSorting();
        }
        
        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandIsEmpty_ReturnsNull()
        {
            var result = _sevenSevenSevenSorting.SortHand(new List<CardData>());
            Assert.IsNull(result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsSingleCard_ReturnsSameHand()
        {
            var hand = CreateHand(new[] { (CardSuit.Hearts, 7) });

            var result = _sevenSevenSevenSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsTwoCardsOfSameRank_ReturnsSameHand()
        {
            var hand = CreateHand(new[] { (CardSuit.Hearts, 5), (CardSuit.Spades, 5) });

            var result = _sevenSevenSevenSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsThreeSameRank_CorrectlyGroupsThem()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 3), (CardSuit.Spades, 3), (CardSuit.Clubs, 4), (CardSuit.Diamonds, 3)
            });

            var result = _sevenSevenSevenSorting.SortHand(hand);
            var expectedOrder = new List<int> { 3, 3, 3, 4 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsMultipleGroups_SortsCorrectly()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 6), (CardSuit.Hearts, 2), (CardSuit.Spades, 6),
                (CardSuit.Diamonds, 6), (CardSuit.Spades, 2), (CardSuit.Diamonds, 2)
            });

            var result = _sevenSevenSevenSorting.SortHand(hand);
            var expectedOrder = new List<int> { 6, 6, 6, 2, 2, 2 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsScatteredRanks_KeepsNonGroupedCardsAtEnd()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 10), (CardSuit.Hearts, 8), (CardSuit.Spades, 8),
                (CardSuit.Clubs, 5), (CardSuit.Diamonds, 8)
            });

            var result = _sevenSevenSevenSorting.SortHand(hand);
            var expectedOrder = new List<int> { 8, 8, 8, 10, 5 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsFourSameRank_GroupsThreeAndLeavesOne()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 9), (CardSuit.Spades, 9), (CardSuit.Diamonds, 9), (CardSuit.Clubs, 9)
            });

            var result = _sevenSevenSevenSorting.SortHand(hand);
            var expectedOrder = new List<int> { 9, 9, 9, 9 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsMultipleSuitsButNoGroups_KeepsOriginalOrder()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 1), (CardSuit.Spades, 2), (CardSuit.Diamonds, 3), (CardSuit.Clubs, 4)
            });

            var result = _sevenSevenSevenSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsScatteredPairs_DoesNotGroupPairs()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 5), (CardSuit.Spades, 5), (CardSuit.Hearts, 9),
                (CardSuit.Spades, 9), (CardSuit.Clubs, 6)
            });

            var result = _sevenSevenSevenSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandHasMoreThanOneValidGroup_PrioritizesLargerGroups()
        {
            var hand = CreateHand(new[]
            {
                (CardSuit.Hearts, 7), (CardSuit.Spades, 3), (CardSuit.Spades, 7),
                (CardSuit.Diamonds, 7), (CardSuit.Hearts, 3), (CardSuit.Diamonds, 3),
                (CardSuit.Clubs, 2), (CardSuit.Clubs, 3), (CardSuit.Clubs, 4), (CardSuit.Clubs, 7)
            });

            var result = _sevenSevenSevenSorting.SortHand(hand);
            var expectedOrder = new List<int> { 7, 7, 7, 7, 3, 3, 3, 3, 2, 4 };

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