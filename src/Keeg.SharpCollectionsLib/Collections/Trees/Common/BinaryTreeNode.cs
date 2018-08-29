using System;

namespace Keeg.SharpCollectionsLib.Collections.Trees.Common
{
    /// <summary>
    /// A node for use in a binary tree.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the objects in the binary tree node. Must implement the <see cref="IComparable{T}"/> interface.
    /// </typeparam>
    internal class BinaryTreeNode<T> where T : IComparable<T>
    {
        #region Constructors
        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{T}"/> class.
        /// </summary>
        internal BinaryTreeNode()
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{T}"/> class.
        /// </summary>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryTreeNode(T value) : this(null, value)
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{T}"/> class.
        /// </summary>
        /// <param name="parent">The parent node to associate with this node.</param>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryTreeNode(BinaryTreeNode<T> parent, T value)
        {
            Parent = parent;
            Value = value;
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{T}"/> class.
        /// </summary>
        /// <param name="parent">The parent node to associate with this node.</param>
        /// <param name="left">The left child node.</param>
        /// <param name="right">The right child node.</param>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryTreeNode(BinaryTreeNode<T> parent, BinaryTreeNode<T> left, BinaryTreeNode<T> right, T value)
        {
            Parent = parent;
            Left = left;
            Right = right;
            Value = value;
        }
        #endregion

        #region Auto Properties
        /// <summary>
        /// Gets and sets a reference to the parent node.
        /// </summary>
        internal virtual BinaryTreeNode<T> Parent { get; set; }

        /// <summary>
        /// Gets and sets reference to the left child node.
        /// </summary>
        internal virtual BinaryTreeNode<T> Left { get; set; }

        /// <summary>
        /// Gets and sets reference to the right child node.
        /// </summary>
        internal virtual BinaryTreeNode<T> Right { get; set; }

        /// <summary>
        /// Gets and sets the value to store in the node.
        /// </summary>
        internal T Value { get; set; }
        #endregion

        #region Properties
        /// <summary>
        /// Gets if the node is a leaf node.
        /// </summary>
        internal virtual bool IsLeaf => (Left == null) && (Right == null);

        /// <summary>
        /// Gets if the node is a left child.
        /// </summary>
        internal virtual bool IsLeftChild => (Parent == null) ? false : Parent.Left == this;

        /// <summary>
        /// Gets if the node is a right child.
        /// </summary>
        internal virtual bool IsRightChild => (Parent == null) ? false : Parent.Right == this;
        #endregion

        /// <summary>
        /// Nulls out all the references.
        /// </summary>
        internal virtual void Invalidate()
        {
            Parent = null;
            Left = null;
            Right = null;
            Value = default(T);
        }
    }
}
