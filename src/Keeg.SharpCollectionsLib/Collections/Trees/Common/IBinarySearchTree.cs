using System;

namespace Keeg.SharpCollectionsLib.Collections.Trees.Common
{
    /// <summary>
    /// Defines methods to manipulate a binary search tree.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the binary tree.</typeparam>
    interface IBinarySearchTree<T> : IBinaryTree<T>
    {
        /// <summary>
        /// Traverses a binary tree in order and performs a callback <see cref="Action{T}"/> on each node.
        /// </summary>
        /// <param name="action"></param>
        void TraverseBinaryTreeInOrder(Action<T> action);

        /// <summary>
        /// Traverses a binary tree in reverse order and performs a callback <see cref="Action{T}"/> on each node.
        /// </summary>
        /// <param name="action"></param>
        void TraverseBinaryTreeInReverseOrder(Action<T> action);
    }
}
