using System;
using System.Collections.Generic;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// A Shell Sort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    public class ShellSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Private Variables
        // Default gap sequence by Marcin Ciura https://oeis.org/A102549
        private static readonly int[] _DefaultGapSequence = { 1750, 701, 301, 132, 57, 23, 10, 4, 1 };
        private IList<int> _gapSequence;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="ShellSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.
        /// Defaults the Gap Sequence to the one by Marcin Ciura.
        /// </remarks>
        public ShellSort() : this(_DefaultGapSequence)
        {
        }

        /// <summary>
        /// Initializes a <see cref="ShellSort{T}"/> class.
        /// </summary>
        /// <param name="gapSequence">The gap sequence to use.</param>
        public ShellSort(IList<int> gapSequence) : base()
        {
            _gapSequence = gapSequence ?? _DefaultGapSequence;
        }

        /// <summary>
        /// Initializes a <see cref="ShellSort{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public ShellSort(IComparer<T> comparer) : base(comparer)
        {
            _gapSequence = _DefaultGapSequence;
        }

        /// <summary>
        /// Initializes a <see cref="ShellSort{T}"/> class.
        /// </summary>
        /// <param name="gapSequence">The gap sequence to use.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        public ShellSort(IList<int> gapSequence, IComparer<T> comparer) : base(comparer)
        {
            _gapSequence = gapSequence ?? _DefaultGapSequence;
        }
        #endregion

        /// <summary>
        /// Gets and sets the gap sequence to use for the sorting algorithm.
        /// </summary>
        /// <remarks>If null it sets it to the default gap sequence.</remarks>
        public IList<int> GapSequence
        {
            get => _gapSequence;
            set
            {
                _gapSequence = value ?? _DefaultGapSequence;
            }
        }

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Shell Sort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public override IList<T> Sort(IList<T> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            foreach (var gap in _gapSequence)
            {
                int gapStart = gap + start;
                int j;
                for (int i = gapStart; i < start + count; i++)
                {
                    var temp = list[i];
                    for (j = i; j >= gap && _comparer.Compare(list[j - gapStart], temp) > 0; j -= gapStart)
                    {
                        list[j] = list[j - gap];
                    }

                    list[j] = temp;
                }
            }

            return list;
        }
    }
}
