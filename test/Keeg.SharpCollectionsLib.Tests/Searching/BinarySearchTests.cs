using System;
using System.Collections.Generic;
using System.Linq;
using Keeg.SharpCollectionsLib.Searching;
using Xunit;

namespace Keeg.SharpCollectionsLib.Tests.Searching
{
    public class BinarySearchTests
    {
        internal static int[] sortedTestArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        internal class SearchCustomComparer : Comparer<int>
        {
            public override int Compare(int x, int y)
            {
                if (x < y)
                {
                    return -1;
                }
                else if (x == y)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        [Theory]
        [InlineData(4, 4)]
        public void Search_SortedArrayDefaultCompare(int item, int expected)
        {
            int position = BinarySearch.Search(sortedTestArray, item, 0, sortedTestArray.Length);
            Assert.Equal(position, expected);
        }

        [Theory]
        [InlineData(4, 4)]
        public void Search_SortedArrayCustomCompare(int item, int expected)
        {
            int position = BinarySearch.Search(sortedTestArray, item, 0, sortedTestArray.Length, new SearchCustomComparer());
            Assert.Equal(position, expected);
        }

        [Theory]
        [InlineData(10, -1)]
        public void Search_SortedArrayNotFoundDefaultCompare(int item, int expected)
        {
            int position = BinarySearch.Search(sortedTestArray, item, 0, sortedTestArray.Length);
            Assert.Equal(position, expected);
        }

        [Theory]
        [InlineData(10, -1)]
        public void Search_SortedArrayNotFoundCustomCompare(int item, int expected)
        {
            int position = BinarySearch.Search(sortedTestArray, item, 0, sortedTestArray.Length, new SearchCustomComparer());
            Assert.Equal(position, expected);
        }

        [Theory]
        [InlineData(257, 257)]
        public void Search_SortedArrayDefaultCompareStressTest(int item, int expected)
        {
            var list = Enumerable.Range(0, 1000).ToArray();
            int position = BinarySearch.Search(list, item, 0, list.Length);
            Assert.Equal(position, expected);
        }

        [Theory]
        [InlineData(257, 257)]
        public void Search_SortedArrayCustomCompareStressTest(int item, int expected)
        {
            var list = Enumerable.Range(0, 1000).ToArray();
            int position = BinarySearch.Search(list, item, 0, list.Length, new SearchCustomComparer());
            Assert.Equal(position, expected);
        }
    }
}
