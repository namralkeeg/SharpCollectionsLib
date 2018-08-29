using System;
using System.Collections;
using System.Collections.Generic;

namespace Keeg.SharpCollectionsLib.Collections.Trees.Common
{
    /// <summary>
    /// Defines methods to manipulate a generic binary tree.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the binary tree.</typeparam>
    interface IBinaryTree<T> : ICollection<T>, ICollection, IReadOnlyCollection<T>
    {
        /// <summary>
        /// Gets and sets the <see cref="IComparer{T}"/> used for comparing elements in a binary tree.
        /// </summary>
        /// <value>
        /// An <see cref="IComparer{T}"/> used for comparing elements in a binary tree.
        /// </value>
        IComparer<T> Comparer { get; set; }

        /// <summary>
        /// Gets the height of a binary tree.
        /// </summary>
        /// <value>The height of a binary tree is the number of edges between the tree's root and its furthest leaf.</value>
        int Height { get; }

        /// <summary>
        /// Gets the count of leaf nodes in the binary tree.
        /// </summary>
        /// <value>The total number of leaf nodes in the tree.</value>
        int LeafCount { get; }

        /// <summary>
        /// Gets the maximum width of a binary tree.
        /// </summary>
        /// <value>The maximum width of the tree at all levels.</value>
        int Width { get; }

        /// <summary>
        /// Traverses a binary tree Pre-Order (NLR) and performs a callback <see cref="Action{T}"/> on each node.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each node.</param>
        void TraverseTreePreOrder(Action<T> action);

        /// <summary>
        /// Traverses a binary tree Post-Order (LRN) and performs a callback <see cref="Action{T}"/> on each node.
        /// </summary>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each node.</param>
        void TraverseTreePostOrder(Action<T> action);
    }
}
