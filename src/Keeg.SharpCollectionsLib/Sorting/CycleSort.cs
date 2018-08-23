using System;
using System.Collections.Generic;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// A Cycle Sort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public class CycleSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a <see cref="CycleSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public CycleSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="CycleSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public CycleSort(IComparer<T> comparer) : base(comparer)
        {
        }
        #endregion

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Cycle Sort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public override IList<T> Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            for (int cyclePosition = start; cyclePosition < count - 1; cyclePosition++)
            {
                var item = list[cyclePosition];
                int position = cyclePosition;
                for (int i = cyclePosition + 1; i < count; i++)
                {
                    if (Comparer.Compare(list[i], item) < 0)
                    {
                        position++;
                    }
                }

                if (position == cyclePosition)
                {
                    continue;
                }

                while (Comparer.Compare(item, list[position]) == 0)
                {
                    position++;
                }

                if (position != cyclePosition)
                {
                    var temp = item;
                    item = list[position];
                    list[position] = temp;
                }

                while (position != cyclePosition)
                {
                    position = cyclePosition;
                    for (int i = cyclePosition + 1; i < count; i++)
                    {
                        if (Comparer.Compare(list[i], item) < 0)
                        {
                            position++;
                        }
                    }

                    while (Comparer.Compare(item, list[position]) == 0)
                    {
                        position++;
                    }

                    if (Comparer.Compare(item, list[position]) != 0)
                    {
                        var temp = item;
                        item = list[position];
                        list[position] = temp;
                    }
                }
            }

            return list;
        }
    }
}
