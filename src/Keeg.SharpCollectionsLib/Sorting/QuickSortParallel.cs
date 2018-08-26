using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// A Quicksort with parallel sorting implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public sealed class QuickSortParallel<T> : QuickSort<T> where T : IComparable<T>
    {
        #region Static Fields
        private static readonly int _maxParallelTasks = Environment.ProcessorCount * 2;
        #endregion

        #region Instance Fields
        private volatile int _parallelInvokeCalls = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="QuickSortParallel{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// Defaults the alternate sorting algorithm to <see cref="InsertionSort{T}"/>.
        /// </remarks>
        public QuickSortParallel() : base()
        {
        }

        /// <summary>
        /// Initializes a <see cref="QuickSortParallel{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        /// <remarks>Defaults the alternate sorting algorithm to <see cref="InsertionSort{T}"/>.</remarks>
        public QuickSortParallel(IComparer<T> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a <see cref="QuickSortParallel{T}"/> class.
        /// </summary>
        /// <param name="sortAlgorithm">The alternate sorting algorithm to use when the count threshold is reached.</param>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// </remarks>
        public QuickSortParallel(ISortAlgorithm<T> sortAlgorithm) : base(sortAlgorithm)
        {
        }

        /// <summary>
        /// Initializes a <see cref="QuickSortParallel{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        /// <param name="sortAlgorithm">The alternate sorting algorithm to use when the count threshold is reached.</param>
        public QuickSortParallel(IComparer<T> comparer, ISortAlgorithm<T> sortAlgorithm) : base(comparer, sortAlgorithm)
        {
        }
        #endregion

        /// <summary>
        /// Internal Quicksort Parallel recursive sorting function.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="low">Starting inclusive index of the list to work with.</param>
        /// <param name="high">Ending inclusive index of the list to work with.</param>
        /// <remarks>
        /// If the max number of parallel tasks hasn't been reached, execute in parallel.
        /// Otherwise run sequentially like normal Quick Sort.
        /// </remarks>
        protected override void QuickSortInternal(IList<T> list, int low, int high)
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
                if (_parallelInvokeCalls < _maxParallelTasks)
                {
                    Interlocked.Increment(ref _parallelInvokeCalls);
                    Parallel.Invoke(
                        () => QuickSortInternal(list, low, partitionIndex - 1),
                        () => QuickSortInternal(list, partitionIndex + 1, high));
                    Interlocked.Decrement(ref _parallelInvokeCalls);
                }
                else
                {
                    QuickSortInternal(list, low, partitionIndex - 1);
                    QuickSortInternal(list, partitionIndex + 1, high);
                }
            }
        }
    }
}
