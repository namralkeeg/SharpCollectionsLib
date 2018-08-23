using System;
using System.Collections.Generic;

namespace Keeg.SharpCollectionsLib.Sorting
{
    /// <summary>
    /// Provides a base class for implementing the <see cref="ISortAlgorithm{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to sort.</typeparam>
    public abstract class SortAlgorithm<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        #region Instance Fields
        /// <summary>
        /// The <see cref="IComparer{T}"/> to use for comparing objects while sorting.
        /// </summary>
        protected IComparer<T> _comparer;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="SortAlgorithm{T}"/> class.
        /// </summary>
        /// <remarks>Defaults the <see cref="Comparer"/> to a <see cref="Comparer{T}.Default"/>
        /// which should be the <see cref="IComparable{T}"/> implementation for T.</remarks>
        protected SortAlgorithm()
        {
            _comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a <see cref="SortAlgorithm{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all sorting comparisons.</param>
        protected SortAlgorithm(IComparer<T> comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="IComparer{T}"/> to use for sorting the objects.
        /// </summary>
        /// <remarks>Defaults to <see cref="Comparer{T}.Default"/></remarks>.
        public virtual IComparer<T> Comparer
        {
            get => _comparer;
            set => _comparer = value ?? Comparer<T>.Default;
        }
        #endregion

        /// <summary>
        /// A helper function for validating the parameters passed to the main sort function.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        protected void SortValidationCheck(IList<T> list, int start, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((start < 0) || (start > list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }

            if (start + count > list.Count)
            {
                throw new ArgumentException($"{nameof(start)} plus {nameof(count)} is out of list range.");
            }
        }

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the <see cref="Comparer"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public virtual IList<T> Sort(IList<T> list)
        {
            return Sort(list, 0, list.Count);
        }

        /// <summary>
        /// Sorts the entire <see cref="IList{T}"/> in place using the <see cref="Comparer"/>.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of objects to be sorted.</param>
        /// <param name="start">The starting index of the first object to sort.</param>
        /// <param name="count">The number of objects to include in the sort.</param>
        /// <returns>An <see cref="IList{T}"/> reference to the sorted <see cref="IList{T}"/>.</returns>
        public abstract IList<T> Sort(IList<T> list, int start, int count);
    }
}
