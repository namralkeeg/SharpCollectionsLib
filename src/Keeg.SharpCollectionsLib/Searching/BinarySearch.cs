using System;
using System.Collections.Generic;

namespace Keeg.SharpCollectionsLib.Searching
{
    public static class BinarySearch
    {
        private const string _InvalidCompareReturnMessage = "Invalid comparison return value. Must be 1, 0, or -1.";

        /// <summary>
        /// Performs a Binary Search on the entire list.
        /// </summary>
        /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> of pre-sorted objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        public static int Search<T>(IList<T> list, T item) where T : IComparable<T>
        {
            return Search(list, item, 0, list.Count);
        }

        /// <summary>
        /// Performs a Binary Search on the entire list.
        /// </summary>
        /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> of pre-sorted objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="start">The index to start the search from in the list.</param>
        /// <param name="count">The number of objects to search.</param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown if list is null.</exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static int Search<T>(IList<T> list, T item, int start, int count) where T : IComparable<T>
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((start < 0) || (start > list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (list.Count - start < count)
            {
                throw new ArgumentException($"{nameof(start)} plus {nameof(count)} is out of list range.");
            }

            int right = count - 1;
            int left = start;

            while (left <= right)
            {
                int middle = left + ((right - left) >> 1);    // same as (left + right) / 2
                switch (list[middle].CompareTo(item))
                {
                    case -1:
                        left = middle + 1;
                        break;
                    case 0:
                        return middle;
                    case 1:
                        right = middle - 1;
                        break;
                    default:
                        throw new InvalidOperationException(_InvalidCompareReturnMessage);
                }
            }

            return -1;
        }

        /// <summary>
        /// Performs a Binary Search on the entire list.
        /// </summary>
        /// <typeparam name="T">The type of objects to sort. Must implement <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="list">The <see cref="IList{T}"/> of pre-sorted objects to be searched.</param>
        /// <param name="item">The object to search for in the list.</param>
        /// <param name="start">The index to start the search from in the list.</param>
        /// <param name="count">The number of objects to search.</param>
        /// <param name="comparer"></param>
        /// <returns>The position of the object in the list if found, otherwise -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown if list or comparer is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if start is less than 0 or greater than list length.</exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static int Search<T>(IList<T> list, T item, int start, int count, IComparer<T> comparer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            if ((start < 0) || (start > list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (list.Count - start < count)
            {
                throw new ArgumentException($"{nameof(start)} plus {nameof(count)} is out of list range.");
            }

            int right = count - 1;
            int left = start;

            while (left <= right)
            {
                int middle = left + ((right - left) >> 1);    // same as (left + right) / 2
                switch (comparer.Compare(list[middle], item))
                {
                    case -1:
                        left = middle + 1;
                        break;
                    case 0:
                        return middle;
                    case 1:
                        right = middle - 1;
                        break;
                    default:
                        throw new InvalidOperationException(_InvalidCompareReturnMessage);
                }
            }

            return -1;
        }
    }
}
