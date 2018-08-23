using System;
using System.Linq;
using Keeg.SharpCollectionsLib.Sorting;
using Xunit;

namespace Keeg.SharpCollectionsLib.Tests.Sorting
{
    public class CountingSortTests
    {
        internal static int[] testArray = { 1, 9, 2, 8, 3, 7, 4, 6, 5, 0 };
        internal static Func<int, int> getKey = (int k) => k.GetHashCode();
        internal static ISortAlgorithm<int> defaultSort = new CountingSort<int>();
        internal static ISortAlgorithm<int> customSort = new CountingSort<int>(getKey);

        [Fact]
        public void Sort_SortEntireListDefaultCompare()
        {
            var tempArray = new int[10];
            testArray.CopyTo(tempArray, 0);
            defaultSort.Sort(tempArray);

            for (int i = 0; i < tempArray.Length; i++)
            {
                Assert.Equal(i, tempArray[i]);
            }
        }

        [Fact]
        public void Sort_SortEntireListCustomCompare()
        {
            var tempArray = new int[10];
            testArray.CopyTo(tempArray, 0);
            customSort.Sort(tempArray);

            for (int i = 0; i < tempArray.Length; i++)
            {
                Assert.Equal(i, tempArray[i]);
            }
        }

        [Fact]
        public void Sort_StressTestDefaultCompare()
        {
            var random = new Random();
            int nodes = 1000;
            var randomList = Enumerable.Range(0, nodes)
                .OrderBy(x => random.Next())
                .ToArray();

            defaultSort.Sort(randomList);
            for (int i = 0; i < nodes; i++)
            {
                Assert.Equal(i, randomList[i]);
            }
        }

        [Fact]
        public void Sort_StressTestCustomCompare()
        {
            var random = new Random();
            int nodes = 1000;
            var randomList = Enumerable.Range(0, nodes)
                .OrderBy(x => random.Next())
                .ToArray();

            customSort.Sort(randomList);
            for (int i = 0; i < nodes; i++)
            {
                Assert.Equal(i, randomList[i]);
            }
        }
    }
}

