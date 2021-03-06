﻿using System;
using System.Collections.Generic;
using System.Linq;
using Keeg.SharpCollectionsLib.Sorting;
using Xunit;

namespace Keeg.SharpCollectionsLib.Tests.Sorting
{
    public class SelectionSortTests
    {
        internal static int[] testArray = { 1, 9, 2, 8, 3, 7, 4, 6, 5, 0 };
        internal static ISortAlgorithm<int> defaultSort = new SelectionSort<int>();
        internal static ISortAlgorithm<int> customSort = new SelectionSort<int>(new SortCustomComparer());

        internal class SortCustomComparer : Comparer<int>
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
