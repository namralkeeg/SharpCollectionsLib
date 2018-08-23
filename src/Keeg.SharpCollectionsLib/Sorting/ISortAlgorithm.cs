using System;
using System.Collections.Generic;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// Represents a set functions for sorting a generic <see cref="IList{T}"/> of objects.
    /// </summary>
    /// <typeparam name="T">The type of objects in the <see cref="IList{T}"/> to sort. 
    /// Objects must also implement the <see cref="IComparable{T}"/> interface.</typeparam>
    public interface ISortAlgorithm<T> where T : IComparable<T>
    {
        /// <summary>
        /// Gets or sets the <see cref="IComparer{T}"/> used for sorting the objects.
        /// </summary>
        /// <returns>The <see cref="IComparer{T}"/> the sorting function will use.</returns>
        IComparer<T> Comparer { get; set; }

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the <see cref="Comparer"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        IList<T> Sort(IList<T> list);

        /// <summary>
        /// Sorts the <see cref="IList{T}"/> in place using the <see cref="Comparer"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to sort.</param>
        /// <param name="start">The inclusive index into the <see cref="IList{T}"/> to start.</param>
        /// <param name="count">The number of objects to sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        IList<T> Sort(IList<T> list, int start, int count);
    }
}