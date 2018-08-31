namespace Keeg.SharpCollectionsLib.Collections.Trees.Common
{
    /// <summary>
    /// An abstract node for use in a binary tree.
    /// </summary>
    /// <typeparam name="TClass">
    /// The class type of the class deriving from this abstract base class to support circular
    /// generic references.
    /// </typeparam>
    /// <typeparam name="TData">The type of object stored in the <see cref="Value"/> property.</typeparam>
    internal abstract class BinaryTreeNode<TClass, TData> : BinaryNode<TClass, TData>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        internal BinaryTreeNode()
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryTreeNode(TData value) : base(value)
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="parent">The parent node.</param>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryTreeNode(TClass parent, TData value) : base(value)
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="left">The left child node.</param>
        /// <param name="right">The right child node.</param>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryTreeNode(TClass left, TClass right, TData value) : base(left, right, value)
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryTreeNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="parent">The parent node.</param>
        /// <param name="left">The left child node.</param>
        /// <param name="right">The right child node.</param>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryTreeNode(TClass parent, TClass left, TClass right, TData value) : base(left, right, value)
        {
            Parent = parent;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets if this node is a leaf node.
        /// </summary>
        internal virtual bool IsLeaf => (Left == null) && (Right == null);

        /// <summary>
        /// Gets and sets the parent node.
        /// </summary>
        internal virtual TClass Parent { get; set; }

        /// <summary>
        /// Resets everything to their default values.
        /// </summary>
        internal override void Invalidate()
        {
            base.Invalidate();
            Parent = default(TClass);
        }

        #endregion Properties
    }
}