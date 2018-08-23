using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keeg.SharpCollectionsLib.Common
{
    internal static class Utils
    {
        /// <summary>
        /// Swaps the left and right values.
        /// </summary>
        /// <typeparam name="T">The element type of the items to swap.</typeparam>
        /// <param name="left">Value on the left to swap.</param>
        /// <param name="right">Value on the right to swap.</param>
        internal static void Swap<T>(ref T left, ref T right)
        {
            var temp = left;
            left = right;
            right = temp;
        }

        /// <summary>
        /// Swaps two values in the generic array at index left and index right.
        /// </summary>
        /// <typeparam name="T">The element type of the <see cref="Array"/>.</typeparam>
        /// <param name="array"><see cref="Array"/> of generic T items. </param>
        /// <param name="left">Index of the first item to swap in the array.</param>
        /// <param name="right">Index of the second item to swap in the array.</param>
        internal static void Swap<T>(T[] array, int left, int right)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (left == right)
            {
                return;
            }

            var temp = array[left];
            array[left] = array[right];
            array[right] = temp;
        }

        /// <summary>
        /// Swaps two values in the <see cref="IList{T}"/> at index left and index right.
        /// </summary>
        /// <typeparam name="T">The element type of the <see cref="IList{T}"/></typeparam>
        /// <param name="list"><see cref="IList{T}"/> of generic T items.</param>
        /// <param name="left">Index of the first item to swap in the <see cref="IList{T}"/>.</param>
        /// <param name="right">Index of the second item to swap in the <see cref="IList{T}"/>.</param>
        internal static void Swap<T>(IList<T> list, int left, int right)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (left == right)
            {
                return;
            }

            var temp = list[left];
            list[left] = list[right];
            list[right] = temp;
        }
    }
}
