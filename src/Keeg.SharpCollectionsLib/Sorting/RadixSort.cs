using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// A LSD Radix Sort implementation of the <see cref="ISortAlgorithm{int}"/> interface.
    /// </summary>
    public sealed class RadixSort : SortAlgorithm<int>
    {
        #region Instance Variables
        private const int _DefaultNumericBase = 10;
        private int _numericBase;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="RadixSort"/> class.
        /// </summary>
        /// <remarks>Defaults the numeric base to 10.</remarks>
        public RadixSort() : this(null, _DefaultNumericBase)
        {
        }

        /// <summary>
        /// Initializes a <see cref="RadixSort"/> class.
        /// </summary>
        /// <param name="numericBase">The numeric base of the numbers to sort.</param>
        public RadixSort(int numericBase) : this(null, numericBase)
        {
        }

        /// <summary>
        /// Initializes a <see cref="RadixSort"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{int}"/> to use for all sorting comparisons.</param>
        public RadixSort(IComparer<int> comparer) : this(comparer, _DefaultNumericBase)
        {
        }

        /// <summary>
        /// Initializes a <see cref="RadixSort"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{int}"/> to use for all sorting comparisons.</param>
        /// <param name="numericBase">The numeric base of the numbers to sort.</param>
        public RadixSort(IComparer<int> comparer, int numericBase) : base(comparer)
        {
            _numericBase = numericBase;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The numeric base of the integers being sorted.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if tyring to set a value less than 2.</exception>
        public int NumericBase
        {
            get => _numericBase;
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Numeric base must be 2 or greater.");
                }

                _numericBase = value;
            }
        }
        #endregion

        /// <summary>
        /// Finds the largest number in the list.
        /// </summary>
        /// <param name="list">The <see cref="IList{int}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>The largest integer in the list.</returns>
        /// <exception cref="InvalidOperationException">Thrown if any integers are negative.</exception>
        private int GetMax(IList<int> list, int start, int count)
        {
            int maxValue = list[start];
            for (int i = start; i < count; i++)
            {
                if (list[i] < 0)
                {
                    throw new InvalidOperationException("Negative numbers are not supported.");
                }

                if (list[i] > maxValue)
                {
                    maxValue = list[i];
                }
            }

            return maxValue;
        }

        /// <summary>
        /// Sorts the entire <see cref="IList{int}"/> in place using the LSD Radix Sort algorithm.
        /// </summary>
        /// <param name="list">The <see cref="IList{int}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public override IList<int> Sort(IList<int> list, int start, int count)
        {
            SortValidationCheck(list, start, count);

            // Get the largest value and check for negative numbers.
            int maxValue = GetMax(list, start, count);

            // Create the buckets for each digit.
            var buckets = new List<int>[NumericBase];
            int index = 0;

            // Iterate, sorting the list by each base - digit
            while ((int)Math.Pow(NumericBase, index) <= maxValue)
            {
                for (int i = start; i < start + count; i++)
                {
                    // Find the base digit from the number.
                    var digit = (list[i] / (int)Math.Pow(NumericBase, index)) % NumericBase;

                    if (buckets[digit] == null)
                    {
                        buckets[digit] = new List<int>();
                    }

                    // Add the number to the correct bucket.
                    buckets[digit].Add(list[i]);
                }

                int startIndex = start;
                // Update the list with what's in the buckets.
                for (int i = 0; i < buckets.Length; i++)
                {
                    if (buckets[i] == null)
                    {
                        continue;
                    }

                    for (int j = 0; j < buckets[i].Count; j++)
                    {
                        list[startIndex++] = buckets[i][j];
                    }
                }

                // Clean out the buckets.
                for (int i = 0; i < buckets.Length; i++)
                {
                    if (buckets[i] != null)
                    {
                        buckets[i].Clear();
                    }
                }

                index++;
            }

            return list;
        }
    }
}
