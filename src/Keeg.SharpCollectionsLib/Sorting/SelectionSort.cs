using System;
using System.Collections.Generic;
using static Keeg.SharpCollectionsLib.Common.Utils;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// A Selection Sort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public class SelectionSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a <see cref="SelectionSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public SelectionSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="SelectionSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public SelectionSort(IComparer<T> comparer) : base(comparer)
        {
        }
        #endregion

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Selection Sort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public override IList<T> Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            int least;
            for (int i = start; i < start + count; i++)
            {
                least = i;
                for (int j = i + 1; j < start + count; j++)
                {
                    if (Comparer.Compare(list[j], list[least]) < 0)
                    {
                        least = j;
                    }
                }

                if (least != i)
                {
                    Swap(list, i, least);
                }
            }

            return list;
        }
    }
}
