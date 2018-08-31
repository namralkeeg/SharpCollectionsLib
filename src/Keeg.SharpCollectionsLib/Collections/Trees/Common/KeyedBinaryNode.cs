namespace Keeg.SharpCollectionsLib.Collections.Trees.Common
{
    /// <summary>
    /// An abstract binary node class with a key.
    /// </summary>
    /// <typeparam name="TClass">
    /// The class type of the class deriving from this abstract base class to support circular
    /// generic references.
    /// </typeparam>
    /// <typeparam name="TKey">The type of object stored in the <see cref="Key"/> property.</typeparam>
    /// <typeparam name="TData">The type of object stored in the <see cref="Value"/> property.</typeparam>
    internal abstract class KeyedBinaryNode<TClass, TKey, TData> : BinaryNode<TClass, TData>
    {
        #region Constructors

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryNode{TClass, TKey, TData}"/> class.
        /// </summary>
        internal KeyedBinaryNode()
        {
        }

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryNode{TClass, TKey, TData}"/> class.
        /// </summary>
        /// <param name="key">They key to store in the node.</param>
        internal KeyedBinaryNode(TKey key) : base(default(TData))
        {
            Key = key;
        }

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryNode{TClass, TKey, TData}"/> class.
        /// </summary>
        /// <param name="key">They key to store in the node.</param>
        /// <param name="value">The value to store in the node.</param>
        internal KeyedBinaryNode(TKey key, TData value) : base(value)
        {
            Key = key;
        }

        /// <summary>
        /// Initializes a <see cref="KeyedBinaryNode{TClass, TKey, TData}"/> class.
        /// </summary>
        /// <param name="key">They key to store in the node.</param>
        /// <param name="value">The value to store in the node.</param>
        /// <param name="left">The left child node.</param>
        /// <param name="right">The right child node.</param>
        internal KeyedBinaryNode(TKey key, TData value, TClass left, TClass right) : base(left, right, value)
        {
            Key = key;
        }

        #endregion Constructors

        /// <summary>
        /// Gets and sets the key in the node.
        /// </summary>
        internal TKey Key { get; set; }

        /// <summary>
        /// Resets everything to their default values.
        /// </summary>
        internal override void Invalidate()
        {
            base.Invalidate();
            Key = default(TKey);
        }
    }
}