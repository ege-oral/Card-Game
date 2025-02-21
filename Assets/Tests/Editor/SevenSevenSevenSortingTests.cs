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
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 7, null, null)
            };

            var result = _sevenSevenSevenSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsTwoCardsOfSameRank_ReturnsSameHand()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 5, null, null),
                new(CardSuit.Spades, 5, null, null)
            };

            var result = _sevenSevenSevenSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsThreeSameRank_CorrectlyGroupsThem()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 3, null, null),
                new(CardSuit.Spades, 3, null, null),
                new(CardSuit.Clubs, 4, null, null),
                new(CardSuit.Diamonds, 3, null, null),
            };
            
            var result = _sevenSevenSevenSorting.SortHand(hand);

            var expectedOrder = new List<int> { 3, 3, 3, 4 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsMultipleGroups_SortsCorrectly()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 6, null, null),
                new(CardSuit.Hearts, 2, null, null),
                new(CardSuit.Spades, 6, null, null),
                new(CardSuit.Diamonds, 6, null, null),
                new(CardSuit.Spades, 2, null, null),
                new(CardSuit.Diamonds, 2, null, null)
            };

            var result = _sevenSevenSevenSorting.SortHand(hand);

            var expectedOrder = new List<int> { 6, 6, 6, 2, 2, 2 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsScatteredRanks_KeepsNonGroupedCardsAtEnd()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 10, null, null),
                new(CardSuit.Hearts, 8, null, null),
                new(CardSuit.Spades, 8, null, null),
                new(CardSuit.Clubs, 5, null, null),
                new(CardSuit.Diamonds, 8, null, null)
            };

            var result = _sevenSevenSevenSorting.SortHand(hand);

            var expectedOrder = new List<int> { 8, 8, 8, 10, 5 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsFourSameRank_GroupsThreeAndLeavesOne()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 9, null, null),
                new(CardSuit.Spades, 9, null, null),
                new(CardSuit.Diamonds, 9, null, null),
                new(CardSuit.Clubs, 9, null, null)
            };

            var result = _sevenSevenSevenSorting.SortHand(hand);

            var expectedOrder = new List<int> { 9, 9, 9, 9 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsMultipleSuitsButNoGroups_KeepsOriginalOrder()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 1, null, null),
                new(CardSuit.Spades, 2, null, null),
                new(CardSuit.Diamonds, 3, null, null),
                new(CardSuit.Clubs, 4, null, null)
            };

            var result = _sevenSevenSevenSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandContainsScatteredPairs_DoesNotGroupPairs()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 5, null, null),
                new(CardSuit.Spades, 5, null, null),
                new(CardSuit.Hearts, 9, null, null),
                new(CardSuit.Spades, 9, null, null),
                new(CardSuit.Clubs, 6, null, null)
            };

            var result = _sevenSevenSevenSorting.SortHand(hand);

            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(hand, result);
        }

        [Test]
        public void SevenSevenSevenSorting_SortHand_WhenHandHasMoreThanOneValidGroup_PrioritizesLargerGroups()
        {
            var hand = new List<CardData>
            {
                new(CardSuit.Hearts, 7, null, null),
                new(CardSuit.Spades, 3, null, null),
                new(CardSuit.Spades, 7, null, null),
                new(CardSuit.Diamonds, 7, null, null),
                new(CardSuit.Hearts, 3, null, null),
                new(CardSuit.Diamonds, 3, null, null),
                new(CardSuit.Clubs, 2, null, null),
                new(CardSuit.Clubs, 3, null, null),
                new(CardSuit.Clubs, 4, null, null),
                new(CardSuit.Clubs, 7, null, null),
            };

            var result = _sevenSevenSevenSorting.SortHand(hand);

            var expectedOrder = new List<int> { 7, 7, 7, 7, 3, 3, 3, 3, 2, 4 };

            Assert.NotNull(result);
            Assert.AreEqual(hand.Count, result.Count);
            CollectionAssert.AreEqual(expectedOrder, result.Select(card => card.Rank));
        }
    }
}