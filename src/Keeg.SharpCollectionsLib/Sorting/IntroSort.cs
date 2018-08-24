using System;
using System.Collections.Generic;
using static Keeg.SharpCollectionsLib.Common.Utils;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// An Introsort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <remarks>
    /// See https://en.wikipedia.org/wiki/Introsort for a description of Introsort.
    /// </remarks>
    public class IntroSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Constants
        private const int _DefaultCuttoff = 16;
        private const string _CuttoffThresholdMessage = "Cuttoff threshold must be greater than 2.";
        #endregion

        #region Instance Fields
        private readonly InsertionSort<T> _insertionSort;
        private readonly HeapSort<T> _heapSort;
        private int _cuttoffThreshold;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="IntroSort{T}"/> class.
        /// </summary>
        public IntroSort() : base()
        {
            _insertionSort = new InsertionSort<T>(_comparer);
            _heapSort = new HeapSort<T>(_comparer);
            _cuttoffThreshold = _DefaultCuttoff;
        }

        /// <summary>
        /// Initializes a <see cref="IntroSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public IntroSort(IComparer<T> comparer) : base(comparer)
        {
            _insertionSort = new InsertionSort<T>(_comparer);
            _heapSort = new HeapSort<T>(_comparer);
            _cuttoffThreshold = _DefaultCuttoff;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets the cuttoff threshold for when to switch to Insertion Sort for small lists.
        /// </summary>
        public int CuttoffThreshold
        {
            get => _cuttoffThreshold;
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), _CuttoffThresholdMessage);
                }

                _cuttoffThreshold = value;
            }
        }
        #endregion

        private int Partition(IList<T> list, int low, int high)
        {
            // Begin median-of-three pivot algorithm. 
            // https://en.wikipedia.org/wiki/Quicksort#Choice_of_pivot

            // Same as (low + high) / 2, but prevents an integer overflow.
            int middle = low + ((high - low) >> 1);

            if (Comparer.Compare(list[middle], list[low]) < 0)
            {
                Swap(list, low, middle);
            }

            if (Comparer.Compare(list[high], list[low]) < 0)
            {
                Swap(list, low, high);
            }

            if (Comparer.Compare(list[high], list[middle]) < 0)
            {
                Swap(list, middle, high);
            }

            // Choose the middle as the pivot value.
            var pivotValue = list[middle];

            // Place the pivot at end - 1.
            Swap(list, middle, high - 1);

            // Low, High and the pivot are already partitioned relative to the pivot in median-of-three.
            int i = low + 1;
            int j = high - 2;

            while (true)
            {
                while (Comparer.Compare(list[i], pivotValue) < 0)
                {
                    i++;
                }

                while (Comparer.Compare(list[j], pivotValue) > 0)
                {
                    j--;
                }

                if (i >= j)
                {
                    // Return the pivot back to the middle.
                    // Everything is now less than the pivot on the left and greater than on the right.
                    Swap(list, i, high - 1);
                    return i;
                }

                Swap(list, i, j);
            }
        }

        /// <summary>
        /// Internal recursive function that performs the core Introsort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        /// <param name="depthLimit"></param>
        private void IntroSortInternal(IList<T> list, int low, int high, int depthLimit)
        {
            int count = high - low + 1;

            // If the number of items is at the threshold, then use insertion sort.
            if (count <= CuttoffThreshold)
            {
                _insertionSort.Sort(list, low, count);
                return;
            }

            // If the recursion depth limit is reached do a heap sort instead.
            if (depthLimit == 0)
            {
                _heapSort.Sort(list, low, count);
                return;
            }

            int partitionIndex = Partition(list, low, high);
            IntroSortInternal(list, low, partitionIndex - 1, depthLimit - 1);
            IntroSortInternal(list, partitionIndex + 1, high, depthLimit - 1);
        }

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Introsort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public override IList<T> Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            if (count <= CuttoffThreshold)
            {
                _insertionSort.Sort(list, start, count);
                return list;
            }

            int depthLimit = 2 * (int)Math.Log(count);
            IntroSortInternal(list, start, start + count - 1, depthLimit);
            return list;
        }
    }
}
