namespace Keeg.SharpCollectionsLib.Collections.Trees.Common
{
    /// <summary>
    /// An abstract binary node base class.
    /// </summary>
    /// <typeparam name="TClass">
    /// The class type of the class deriving from this abstract base class to support circular
    /// generic references.
    /// </typeparam>
    /// <typeparam name="TData">The type of object stored in the <see cref="Value"/> property.</typeparam>
    internal abstract class BinaryNode<TClass, TData>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="BinaryNode{TClass, TData}"/> class.
        /// </summary>
        internal BinaryNode()
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinaryNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryNode(TData value)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a <see cref="BinaryNode{TClass, TData}"/> class.
        /// </summary>
        /// <param name="left">The left node.</param>
        /// <param name="right">The right node.</param>
        /// <param name="value">The value to store in the node.</param>
        internal BinaryNode(TClass left, TClass right, TData value)
        {
            Left = left;
            Right = right;
            Value = value;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets and sets reference to the left node.
        /// </summary>
        internal virtual TClass Left { get; set; }

        /// <summary>
        /// Gets and sets reference to the right node.
        /// </summary>
        internal virtual TClass Right { get; set; }

        /// <summary>
        /// Gets and sets the value to store in the node.
        /// </summary>
        internal virtual TData Value { get; set; }

        #endregion Properties

        /// <summary>
        /// Resets everything to their default values.
        /// </summary>
        internal virtual void Invalidate()
        {
            Left = default(TClass);
            Right = default(TClass);
            Value = default(TData);
        }
    }
}