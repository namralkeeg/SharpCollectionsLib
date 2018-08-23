using System;
using System.Collections.Generic;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// An Counting Sort implementation of the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <remarks>
    /// Counting Sort isn't a comparison type sort. So it requires a function that returns an integer key instead.
    /// </remarks>
    public class CountingSort<T> : SortAlgorithm<T> where T : IComparable<T>
    {
        #region Instance Fields
        private const string _NonNegMessage = "Key values cannot be negative.";
        private Func<T, int> _getKey;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="CountingSort{T}"/> class.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="Object.GetHashCode"/> for key generation.
        /// </remarks>
        public CountingSort() : this((T k) => k.GetHashCode())
        {
        }

        /// <summary>
        /// Initializes a <see cref="CountingSort{T}"/> class.
        /// </summary>
        /// <param name="getKey">A function <see cref="Func{T, TResult}"/> that returns an int key based on the value of T.</param>
        /// <remarks>
        /// Defaults to <see cref="Object.GetHashCode"/> for key generation if null.
        /// </remarks>
        public CountingSort(Func<T, int> getKey)
        {
            _getKey = getKey ?? ((T k) => k.GetHashCode());
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="Func{T, TResult}"/> to use for generating keys.
        /// </summary>
        /// <remarks>
        /// Defaults to <see cref="Object.GetHashCode"/> for key generation if null.
        /// </remarks>
        public Func<T, int> GetKey
        {
            get => _getKey;
            set
            {
                _getKey = value ?? ((T k) => k.GetHashCode());
            }
        }
        #endregion

        /// <summary>
        /// Gets the maximum key for all elements in the list.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>The max int key for all elements in the list.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a key is less than zero.</exception>
        private int GetMax(IList<T> list, int start, int count)
        {
            int maxValue = 0;
            for (int i = start; i < count; i++)
            {
                var key = _getKey(list[i]);
                if (key < 0)
                {
                    throw new InvalidOperationException(_NonNegMessage);
                }

                if (key > maxValue)
                {
                    maxValue = key;
                }

            }

            return maxValue;
        }

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the Counting Sort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public override IList<T> Sort(IList<T> list, int start, int count)
        {
            int maxValue = GetMax(list, start, count);
            var countBucket = new int[maxValue + 1];

            // Store the count for each key.
            for (int i = start; i < count; i++)
            {
                countBucket[_getKey(list[i])]++;
            }

            // Change countBucket[i] so that it contains the actual position of the item
            // in the output array.
            for (int i = 1; i <= maxValue; i++)
            {
                countBucket[i] += countBucket[i - 1];
            }

            // Array to build the sorted list.
            var sortedArray = new T[count];

            // Build the output array
            for (int i = start; i < count; i++)
            {
                sortedArray[countBucket[_getKey(list[i])] - 1] = list[i];
                --countBucket[_getKey(list[i])];
            }

            int sortedIndex = 0;
            // Copy the sorted array back to the original.
            for (int i = start; i < count; i++)
            {
                list[i] = sortedArray[sortedIndex++];
            }

            return list;
        }
    }
}
