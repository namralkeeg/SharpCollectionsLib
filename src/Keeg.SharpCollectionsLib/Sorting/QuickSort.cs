using System;
using System.Collections.Generic;
using static Keeg.SharpCollectionsLib.Common.Utils;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// A Quicksort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public class QuickSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Constants
        private const string _RecursiveCuttoffMessage = "Value must be greater than zero.";
        private const string _ThresholdMessage = "Threshold must be greater than 2.";
        private const int _DefaultThreshold = 16;
        #endregion

        #region Instance Fields
        private int _threshold = _DefaultThreshold;
        private ISortAlgorithm<T> _thresholdSort;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="QuickSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// Defaults the alternate sorting algorithm to <see cref="InsertionSort{T}"/>.
        /// </remarks>
        public QuickSort() : base()
        {
            _thresholdSort = new InsertionSort<T>(_comparer);
        }

        /// <summary>
        /// Initializes a <see cref="QuickSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        /// <remarks>Defaults the alternate sorting algorithm to <see cref="InsertionSort{T}"/>.</remarks>
        public QuickSort(IComparer<T> comparer) : base(comparer)
        {
            _thresholdSort = new InsertionSort<T>(_comparer);
        }

        /// <summary>
        /// Initializes a <see cref="QuickSort{T}"/> class.
        /// </summary>
        /// <param name="sortAlgorithm">The alternate sorting algorithm to use when the count threshold is reached.</param>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public QuickSort(ISortAlgorithm<T> sortAlgorithm) : base()
        {
            _thresholdSort = sortAlgorithm ?? new InsertionSort<T>(_comparer);
        }

        /// <summary>
        /// Initializes a <see cref="QuickSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        /// <param name="sortAlgorithm">The alternate sorting algorithm to use when the count threshold is reached.</param>
        public QuickSort(IComparer<T> comparer, ISortAlgorithm<T> sortAlgorithm) : base(comparer)
        {
            _thresholdSort = sortAlgorithm ?? new InsertionSort<T>(_comparer);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets and sets the threshold count for when to use the alternate sorting algorithm.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when value is less than 2.</exception>
        public int Threshold
        {
            get => _threshold;
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), _ThresholdMessage);
                }

                _threshold = value;
            }
        }

        /// <summary>
        /// Gets and sets the alternate sorting algorithm to use when the <see cref="Threshold"/> is reached.
        /// </summary>
        /// <remarks>On null it defaults to <see cref="InsertionSort{T}"/>.</remarks>
        public ISortAlgorithm<T> ThresholdSort
        {
            get => _thresholdSort;
            set
            {
                _thresholdSort = value ?? new InsertionSort<T>(_comparer);
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        /// <returns>The index to the first 'leftmost' and to the last 'rightmost' item of the middle partition.</returns>
        protected virtual int Partition(IList<T> list, int low, int high)
        {
            // Begin median-of-three pivot algorithm. 
            // https://en.wikipedia.org/wiki/Quicksort#Choice_of_pivot
            int middle = low + ((high - low) >> 1); // Same as (low + high) / 2, but prevents an integer overflow.

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

            // Begin partitioning.
            var pivotValue = list[middle];

            // Place the pivot at end - 1.
            Swap(list, middle, high - 1);

            // Low and High are already partitioned relative to the pivot in median-of-three.
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
        /// Internal Quicksort recursive sorting function.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        protected virtual void QuickSortInternal(IList<T> list, int low, int high)
        {
            // If the number of items left is below the threshold, use the alternate sort.
            // Default is Insertion Sort.
            if (high - low + 1 <= Threshold)
            {
                ThresholdSort.Sort(list, low, high - low + 1);
            }
            else
            {
                int partitionIndex = Partition(list, low, high);
                QuickSortInternal(list, low, partitionIndex - 1);
                QuickSortInternal(list, partitionIndex + 1, high);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Quicksort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        /// <remarks>
        /// Optimized with a small list size alternate sort.
        /// Uses median-of-three pivot algorithm.
        /// </remarks>
        public override IList<T> Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            // If the number of items is below the threshold, then use the alternate sorting algorithm.
            // Defaults to Insertion Sort.
            if (count <= Threshold)
            {
                ThresholdSort.Sort(list, start, count);
            }

            QuickSortInternal(list, start, start + count - 1);
            return list;
        }
        #endregion
    }
}
