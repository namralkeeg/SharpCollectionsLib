﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Keeg.SharpCollectionsLib.Collections.Trees.Common;

namespace Keeg.SharpCollectionsLib.Collections.Trees
{
    /// <summary>
    /// A Binary Search Tree implemenatation of the <see cref="IBinarySearchTree{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of objects to store in the binary search tree.</typeparam>
    public class BinarySearchTree<T> : IBinarySearchTree<T> where T : IComparable<T>
    {
        #region Constants

        private const string _AddDuplicateMessage = "Attempted to add duplicate item.";
        private const string _ArrayNonZeroLBMessage = "Array has a non-zero lower bound.";
        private const string _DestArraySizeTooSmallMessage = "Destination array size is too small.";
        private const string _InvalidArrayTypeMessage = "Invalid array type.";
        private const string _OnlySingleDimArraysMessage = "Only single dimensional arrays are supported.";
        private const string _StartIndexZeroGreaterMessage = "Starting index must be 0 or greater.";

        #endregion Constants

        #region Instance Fields

        private IComparer<T> _comparer;
        private int _count;
        private BinarySearchTreeNode _root;
        private object _syncRoot;
        private long _version;

        #endregion Instance Fields

        #region Constructors

        /// <summary>
        /// Initializes a <see cref="BinarySearchTree{T}"/> class.
        /// </summary>
        public BinarySearchTree() : this(Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinarySearchTree{T}"/> class.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all comparisons.</param>
        public BinarySearchTree(IComparer<T> comparer)
        {
            _count = 0;
            _version = 0;
            _comparer = comparer ?? Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a <see cref="BinarySearchTree{T}"/> class.
        /// </summary>
        /// <param name="collection">An <see cref="IEnumerable{T}"/> collection of objects to initialize the tree with.</param>
        public BinarySearchTree(IEnumerable<T> collection) : this(collection, null)
        {
        }

        /// <summary>
        /// Initializes a <see cref="BinarySearchTree{T}"/> class.
        /// </summary>
        /// <param name="collection">An <see cref="IEnumerable{T}"/> collection of objects to initialize the tree with.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to use for all comparisons.</param>
        public BinarySearchTree(IEnumerable<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            _comparer = comparer ?? Comparer<T>.Default;
            _count = 0;
            _version = 0;

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets and sets the <see cref="IComparer{T}"/> used for comparing elements in a binary tree.
        /// </summary>
        /// <value>
        /// An <see cref="IComparer{T}"/> used for comparing elements in a binary tree.
        /// Defaults to <see cref="Comparer{T}.Default"/>
        /// </value>
        public IComparer<T> Comparer { get => _comparer; set => _comparer = value ?? Comparer<T>.Default; }

        /// <summary>
        /// A read-only property that gets the number of items in the tree.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Gets the height of a binary tree.
        /// </summary>
        /// <value>The height of a binary tree is the number of edges between the tree's root and its furthest leaf.</value>
        public int Height => GetHeight(_root);

        /// <summary>
        /// A read-only property that tells if this collection is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// A read-only property that tells if the collection is synchronized.
        /// </summary>
        public bool IsSynchronized => false;

        /// <summary>
        /// Gets the count of leaf nodes in the binary tree.
        /// </summary>
        /// <value>The total number of leaf nodes in the tree.</value>
        public int LeafCount => GetLeafCount(_root);

        /// <summary>
        /// Gets the synchronization root for this object.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
                }

                return _syncRoot;
            }
        }

        /// <summary>
        /// Gets the maximum width of a binary tree.
        /// </summary>
        /// <value>The maximum width of the tree at all levels.</value>
        public int Width => GetWidth();

        #endregion Properties

        #region ICollection<T> Implementation

        /// <summary>
        /// Adds an item to the Binary Search Tree.
        /// </summary>
        /// <param name="item">The object to add to the tree.</param>
        /// <exception cref="Exception">Thrown when the item already exists in the tree.</exception>
        public void Add(T item)
        {
            // If the tree is empty add it to the root.
            if (_root == null)
            {
                _root = new BinarySearchTreeNode(item);
                _count++;
                _version++;
                return;
            }

            int compareResult;
            var current = _root;
            while (true)
            {
                compareResult = _comparer.Compare(item, current.Value);
                // The item already exists in the tree, so throw an exception.
                if (compareResult == 0)
                {
                    throw new Exception(_AddDuplicateMessage);
                }
                else if (compareResult < 0)
                {
                    // If less than the current node and there's no left child add it.
                    if (current.Left == null)
                    {
                        current.Left = new BinarySearchTreeNode(current, item);
                        _count++;
                        break;
                    }

                    // If less than and there's a left child keep going left until an emtpy left node is found.
                    current = current.Left;
                }
                else // if (compareResult > 0)
                {
                    // If greater than the current node and there's no right child add it.
                    if (current.Right == null)
                    {
                        current.Right = new BinarySearchTreeNode(current, item);
                        _count++;
                        break;
                    }

                    // If greater than and there's a right child keep going right until an emtpy right node is found.
                    current = current.Right;
                }
            }
        }

        /// <summary>
        /// Removes all items from the tree and resets the count.
        /// </summary>
        public void Clear()
        {
            _root = null;
            _count = 0;
            _version++;
        }

        /// <summary>
        /// Checks if an item is in the tree.
        /// </summary>
        /// <param name="item">The item to check for in the tree.</param>
        /// <returns>True if the item is found, false otherwise.</returns>
        public bool Contains(T item)
        {
            var current = _root;
            int compareResult;

            while (current != null)
            {
                compareResult = _comparer.Compare(item, current.Value);
                // If smaller go left, larger go right.
                if (compareResult < 0)
                {
                    current = current.Left;
                }
                else if (compareResult > 0)
                {
                    current = current.Right;
                }
                else // if (compareResult == 0)
                {
                    // The item was found in the tree.
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the elements of the <see cref="BinarySearchTree{T}"/> to an <see cref="Array"/>,
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from
        /// <see cref="BinarySearchTree{T}"/>. The <see cref="Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            if (_count == 0)
            {
                return;
            }

            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), _StartIndexZeroGreaterMessage);
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException(_OnlySingleDimArraysMessage, nameof(array));
            }

            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException(_ArrayNonZeroLBMessage, nameof(array));
            }

            if (array.Length - index < _count)
            {
                throw new ArgumentOutOfRangeException(nameof(array), _DestArraySizeTooSmallMessage);
            }

            if (array is T[] testArray)
            {
                CopyTo(testArray, index);
            }
            else
            {
                Type targetType = array.GetType().GetElementType();
                Type sourceType = typeof(T);
                if (!(targetType.IsAssignableFrom(sourceType) || sourceType.IsAssignableFrom(targetType)))
                {
                    throw new ArgumentException(_InvalidArrayTypeMessage, nameof(array));
                }

                if (!(array is object[] objects))
                {
                    throw new ArgumentException(_InvalidArrayTypeMessage, nameof(array));
                }

                try
                {
                    int i = index;
                    using (var treeEnumerator = GetEnumerator())
                    {
                        while (treeEnumerator.MoveNext())
                        {
                            objects[i++] = treeEnumerator.Current;
                        }
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException(_InvalidArrayTypeMessage, nameof(array));
                }
            }
        }

        /// <summary>
        /// Copies the <see cref="BinarySearchTree{T}"/> elements to an existing one-dimensional <see cref="Array"/>,
        /// starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied
        /// from <see cref="BinarySearchTree{T}"/>. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (_count == 0)
            {
                return;
            }

            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), _StartIndexZeroGreaterMessage);
            }

            if (array.Length - arrayIndex < _count)
            {
                throw new ArgumentOutOfRangeException(nameof(array), _DestArraySizeTooSmallMessage);
            }

            int i = arrayIndex;
            using (var treeEnumerator = GetEnumerator())
            {
                while (treeEnumerator.MoveNext())
                {
                    array[i++] = treeEnumerator.Current;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new InOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new InOrderEnumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection in reverse order.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection in reverse.</returns>
        public IEnumerator<T> GetReverseEnumerator()
        {
            return new ReverseOrderEnumerator(this);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="BinarySearchTree{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="BinarySearchTree{T}"/>.</param>
        /// <returns>
        /// <see cref="true"/> if item was successfully removed from the ICollection<T>; otherwise, <see cref="false"/>.
        /// This method also returns <see cref="false"/> if item is not found in the original <see cref="BinarySearchTree{T}"/>.
        /// </returns>
        public bool Remove(T item)
        {
            // The tree is empty.
            if (_root == null)
            {
                return false;
            }

            // The item was found, now remove it.
            if (Contains(item))
            {
                // Call internal recursive delete, and return the rebuilt tree with the item removed.
                _root = RemoveNode(_root, item);
                _count--;
                _version++;
                return true;
            }
            else
            {
                // The item isn't in the tree.
                return false;
            }
        }

        private T GetSmallestValue(BinarySearchTreeNode node)
        {
            var current = node;
            while (current.Left != null)
            {
                current = current.Left;
            }

            return current.Value;
        }

        private BinarySearchTreeNode RemoveNode(BinarySearchTreeNode node, T item)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            int compareResult = _comparer.Compare(item, node.Value);
            // If smaller than the current node recurse left, greater recurse right.
            if (compareResult < 0)
            {
                node.Left = RemoveNode(node.Left, item);
            }
            else if (compareResult > 0)
            {
                node.Right = RemoveNode(node.Right, item);
            }
            else // (compareResult == 0)
            {
                // If it's a leaf node then remove it.
                if (node.IsLeaf)
                {
                    return null;
                }
                else if (node.Left == null)
                {
                    // Node with no left child.
                    return node.Right;
                }
                else if (node.Right == null)
                {
                    // Node with no right child.
                    return node.Left;
                }
                else // A node with two children.
                {
                    // The smallest value in the right sub-tree.
                    var smallestValue = GetSmallestValue(node.Right);
                    node.Value = smallestValue;
                    // Remove the leaf node that had the smallest value.
                    node.Right = RemoveNode(node.Right, smallestValue);
                }
            }

            return node;
        }

        #endregion ICollection<T> Implementation

        #region Binary Tree Specific Functions

        /// <summary>
        /// Traverses the <see cref="BinarySearchTree{T}"/> in order and applies the delegate action to each node.
        /// </summary>
        /// <param name="action">The delegate action to apply to each node.</param>
        public void TraverseBinaryTreeInOrder(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (_root == null)
            {
                return;
            }

            TraverseBinaryTreeInOrder(action, _root);
        }

        /// <summary>
        /// Traverses the <see cref="BinarySearchTree{T}"/> in reverse order and applies the delegate action to each node.
        /// </summary>
        /// <param name="action">The delegate action to apply to each node.</param>
        public void TraverseBinaryTreeInReverseOrder(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (_root == null)
            {
                return;
            }

            TraverseBinaryTreeInReverseOrder(action, _root);
        }

        /// <summary>
        /// Traverses the <see cref="BinarySearchTree{T}"/> in post-order (LRN) and applies the delegate action to each node.
        /// </summary>
        /// <param name="action">The delegate action to apply to each node.</param>
        public void TraverseTreePostOrder(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (_root == null)
            {
                return;
            }

            TraverseTreePostOrder(action, _root);
        }

        /// <summary>
        /// Traverses the <see cref="BinarySearchTree{T}"/> in pre-order (NLR) and applies the delegate action to each node.
        /// </summary>
        /// <param name="action">The delegate action to apply to each node.</param>
        public void TraverseTreePreOrder(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (_root == null)
            {
                return;
            }

            TraverseTreePreOrder(action, _root);
        }

        private int GetHeight(BinarySearchTreeNode node)
        {
            if (node == null)
            {
                return -1;
            }

            return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

        private int GetLeafCount(BinarySearchTreeNode node)
        {
            if (node == null)
            {
                return 0;
            }
            else if (node.IsLeaf)
            {
                return 1;
            }
            else
            {
                return GetLeafCount(node.Left) + GetLeafCount(node.Right);
            }
        }

        private int GetWidth()
        {
            int maxWidth = 0;
            for (int i = 0; i < Height; i++)
            {
                int tempWidth = GetWidth(_root, i);
                if (tempWidth > maxWidth)
                {
                    maxWidth = tempWidth;
                }
            }

            return maxWidth;
        }

        private int GetWidth(BinarySearchTreeNode node, int depth)
        {
            if (node == null)
            {
                return 0;
            }

            if (depth <= 0)
            {
                return 1;
            }

            return GetWidth(node.Left, depth - 1) + GetWidth(node.Right, depth - 1);
        }

        private void TraverseBinaryTreeInOrder(Action<T> action, BinarySearchTreeNode node)
        {
            if (node == null)
            {
                return;
            }

            TraverseBinaryTreeInOrder(action, node.Left);
            action(node.Value);
            TraverseBinaryTreeInOrder(action, node.Right);
        }

        private void TraverseBinaryTreeInReverseOrder(Action<T> action, BinarySearchTreeNode node)
        {
            if (node == null)
            {
                return;
            }

            TraverseBinaryTreeInReverseOrder(action, node.Right);
            action(node.Value);
            TraverseBinaryTreeInReverseOrder(action, node.Left);
        }

        private void TraverseTreePostOrder(Action<T> action, BinarySearchTreeNode node)
        {
            if (node == null)
            {
                return;
            }

            TraverseTreePostOrder(action, node.Left);
            TraverseTreePostOrder(action, node.Right);
            action(node.Value);
        }

        private void TraverseTreePreOrder(Action<T> action, BinarySearchTreeNode node)
        {
            if (node == null)
            {
                return;
            }

            action(node.Value);
            TraverseTreePreOrder(action, node.Left);
            TraverseTreePreOrder(action, node.Right);
        }

        #endregion Binary Tree Specific Functions

        #region Internal Support Classes

        public struct InOrderEnumerator : IEnumerator<T>
        {
            #region Instance Fields

            private readonly Stack<BinarySearchTreeNode> _nodeStack;
            private readonly BinarySearchTreeNode _root;
            private readonly BinarySearchTree<T> _tree;
            private readonly long _version;
            private T _current;

            #endregion Instance Fields

            internal InOrderEnumerator(BinarySearchTree<T> tree)
            {
                _tree = tree ?? throw new ArgumentNullException(nameof(tree));
                _root = tree._root;
                _nodeStack = new Stack<BinarySearchTreeNode>();
                _version = _tree._version;
                _current = default(T);
                StackLeftMostNodesFirst(_root);
            }

            public T Current => _current;
            object IEnumerator.Current => _current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_version != _tree._version)
                {
                    throw new InvalidOperationException("Collection changed during enumeration.");
                }

                if (_nodeStack.Count == 0)
                {
                    return false;
                }

                var topNode = _nodeStack.Pop();
                _current = topNode.Value;
                StackLeftMostNodesFirst(topNode.Right);

                return true;
            }

            public void Reset()
            {
                _nodeStack.Clear();
                StackLeftMostNodesFirst(_root);
            }

            private void StackLeftMostNodesFirst(BinarySearchTreeNode node)
            {
                var current = node;
                while (current != null)
                {
                    _nodeStack.Push(current);
                    current = current.Left;
                }
            }
        }

        public struct ReverseOrderEnumerator : IEnumerator<T>
        {
            #region Instance Fields

            private readonly Stack<BinarySearchTreeNode> _nodeStack;
            private readonly BinarySearchTreeNode _root;
            private readonly BinarySearchTree<T> _tree;
            private readonly long _version;
            private T _current;

            #endregion Instance Fields

            internal ReverseOrderEnumerator(BinarySearchTree<T> tree)
            {
                _tree = tree ?? throw new ArgumentNullException(nameof(tree));
                _root = _tree._root;
                _nodeStack = new Stack<BinarySearchTreeNode>();
                _version = _tree._version;
                _current = default(T);
                StackRightMostNodesFirst(_root);
            }

            public T Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_version != _tree._version)
                {
                    throw new InvalidOperationException("Collection changed during enumeration.");
                }

                if (_nodeStack.Count == 0)
                {
                    return false;
                }

                var topNode = _nodeStack.Pop();
                _current = topNode.Value;
                StackRightMostNodesFirst(topNode.Left);

                return true;
            }

            public void Reset()
            {
                _nodeStack.Clear();
                StackRightMostNodesFirst(_root);
            }

            private void StackRightMostNodesFirst(BinarySearchTreeNode node)
            {
                var current = node;
                while (current != null)
                {
                    _nodeStack.Push(current);
                    current = current.Right;
                }
            }
        }

        internal class BinarySearchTreeNode : BinaryTreeNode<BinarySearchTreeNode, T>
        {
            #region Constructors

            public BinarySearchTreeNode()
            {
            }

            public BinarySearchTreeNode(T value) : base(value)
            {
            }

            public BinarySearchTreeNode(BinarySearchTreeNode parent, T value) : base(parent, value)
            {
            }

            public BinarySearchTreeNode(BinarySearchTreeNode left, BinarySearchTreeNode right, T value) : base(left, right, value)
            {
            }

            public BinarySearchTreeNode(BinarySearchTreeNode parent, BinarySearchTreeNode left, BinarySearchTreeNode right, T value) : base(parent, left, right, value)
            {
            }

            #endregion Constructors

            /// <summary>
            /// Gets if the node is a left child.
            /// </summary>
            internal virtual bool IsLeftChild => (Parent == null) ? false : Parent.Left == this;

            /// <summary>
            /// Gets if the node is a right child.
            /// </summary>
            internal virtual bool IsRightChild => (Parent == null) ? false : Parent.Right == this;
        }

        #endregion Internal Support Classes
    }
}