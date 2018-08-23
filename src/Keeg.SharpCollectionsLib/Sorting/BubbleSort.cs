using System;
using System.Collections.Generic;
using static Keeg.SharpCollectionsLib.Common.Utils;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// A Bubble Sort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort.</typeparam>
    /// <remarks>Includes an optimization where by after every pass, all elements after the 
    /// last swap are sorted, and do not need to be checked again</remarks>
    public sealed class BubbleSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a <see cref="BubbleSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public BubbleSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="BubbleSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public BubbleSort(IComparer<T> comparer) : base(comparer)
        {
        }
        #endregion

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Bubble Sort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public override IList<T> Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            int itemsLeft = count;
            int lastSwapped;
            do
            {
                lastSwapped = 0;
                for (int i = start + 1; i < start + count; i++)
                {
                    if (Comparer.Compare(list[i - 1], list[i]) > 0)
                    {
                        Swap(list, i - 1, i);
                        lastSwapped = i;
                    }
                }

                itemsLeft = lastSwapped;

            } while (itemsLeft > 0);

            return list;
        }
    }
}
