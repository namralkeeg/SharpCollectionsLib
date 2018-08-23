using System;
using System.Collections.Generic;
using static Keeg.SharpCollectionsLib.Common.Utils;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// A Heap Sort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public class HeapSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a <see cref="HeapSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public HeapSort()
        {
        }

        /// <summary>
        /// Initializes a <see cref="HeapSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public HeapSort(IComparer<T> comparer) : base(comparer)
        {
        }
        #endregion

        /// <summary>
        /// Create a max heap out of list, rooted at index.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to heapify.</param>
        /// <param name="index">Index into the array subtree rooted with node index.</param>
        /// <param name="count">The number of objects to include in the heap.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        protected void Heapify(IList<T> list, int index, int count)
        {
            int left = (index << 1) + 1;    // index * 2 + 1
            int right = (index << 1) + 2;   // index * 2 + 2
            int largest = index;            // Initialize largest as root

            // If left child is larger than root
            if (left < count && Comparer.Compare(list[left], list[index]) > 0)
            {
                largest = left;
            }

            // If right child is larger than largest so far
            if (right < count && Comparer.Compare(list[right], list[largest]) > 0)
            {
                largest = right;
            }

            // If largest is not root
            if (largest != index)
            {
                Swap(list, index, largest);

                // Recursively heapify the affected sub-tree
                Heapify(list, largest, count);
            }
        }

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Heapsort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public override IList<T> Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            // Build a max heap.
            for (int i = (count >> 1) - 1; i >= start; i--)
            {
                Heapify(list, i, count);
            }

            // Extract an element from the heap one at a time.
            for (int j = count - 1; j >= start; j--)
            {
                // Move the current root to the end.
                Swap(list, j, start);

                // Call max heapify on a reduced heap.
                Heapify(list, start, j);
            }

            return list;
        }
    }
}
