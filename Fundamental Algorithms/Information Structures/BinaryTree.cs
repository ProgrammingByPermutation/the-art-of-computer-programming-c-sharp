using System;
using System.Collections;
using System.Collections.Generic;

namespace Fundamental_Algorithms.Information_Structures
{
    /// <summary>
    /// Represents a binary tree
    /// </summary>
    /// <typeparam name="T">The type of value to save in the nodes of the tree.</typeparam>
    public class BinaryTree<T>
    {
        /// <summary>
        /// The root of the tree
        /// </summary>
        public BinaryTreeNode<T> Root { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="root">The root of the new binary tree</param>
        public BinaryTree(BinaryTreeNode<T> root) {
            Root = root;
        }

        /// <summary>
        /// Performs an Inorder traversal of the tree. An inorder traversal is performed
        /// as follows:
        /// Traverse the left subtree
        /// Visit the root
        /// Traverse the right subtree
        /// </summary>
        /// <param name="action">The action to perform that currently visited node. It is assumed that
        /// action with neither delete the current node nor any of its ancestors from the tree.</param>
        public void TraverseInorder(Action<BinaryTreeNode<T>> action) {
            if (null == action) {
                return;
            }

            // Let Tree be a pointer to a binary tree; this algorithm visits all nodes of the binary tree inorder,
            // making use of an auxiliary stack A.
            
            // T1. [Initialize] Set stack A empty, and set the link variable P <- Tree
            Stack<BinaryTreeNode<T>> A = new Stack<BinaryTreeNode<T>>();
            BinaryTreeNode<T> P = Root;

            while (null != P || A.Count > 0) {
                // T2. [P = null?] If P = null, go to step T4.
                while (null != P) {
                    // T3. (Now P points to a nonempty binary tree that is to be traversed.) Set A <= P; that is, push
                    // the value of P onto stack A. Then set P <- LLINK(P) and return to step T2.
                    A.Push(P);
                    P = P.Left;
                }

                // T4. [P <= Stack.] If stack A is empty, the algorithm terminates; otherwise set P <= A.
                if (A.Count <= 0) {
                    return;
                }

                // T5. Visit NODE(P). Then set P <- RLINK(P) and return to step T2.
                P = A.Pop();
                action(P);
                P = P.Right;
            }
        }

        /// <summary>
        /// Performs a preorder traversal of the tree. A preorder traversal is performed
        /// as follows:
        /// Visit the root
        /// Traverse the left subtree
        /// Traverse the right subtree
        /// </summary>
        /// <param name="action">The action to perform that currently visited node. It is assumed that
        /// action with neither delete the current node nor any of its ancestors from the tree.</param>
        public void TraversePreorder(Action<BinaryTreeNode<T>> action) {
            if (null == action) {
                return;
            }

            // Let Tree be a pointer to a binary tree; this algorithm visits all nodes of the binary tree preorder,
            // making use of an auxiliary stack A.
            
            // T1. [Initialize] Set stack A empty, and set the link variable P <- Tree
            Stack<BinaryTreeNode<T>> A = new Stack<BinaryTreeNode<T>>();
            BinaryTreeNode<T> P = Root;

            while (null != P || A.Count > 0) {
                // T2. [P = null?] If P = null, go to step T4.
                while (null != P) {
                    // MODIFICATION STEP: Visit NODE(P).
                    // T3. (Now P points to a nonempty binary tree that is to be traversed.) Set A <= P; that is, push
                    // the value of P onto stack A. Then set P <- LLINK(P) and return to step T2.
                    action(P);
                    A.Push(P);
                    P = P.Left;
                }

                // T4. [P <= Stack.] If stack A is empty, the algorithm terminates; otherwise set P <= A.
                if (A.Count <= 0) {
                    return;
                }

                // T5. Then set P <- RLINK(P) and return to step T2.
                P = A.Pop();
                P = P.Right;
            }
        }

        /// <summary>
        /// Performs a postorder traversal of the tree. A postorder traversal is performed
        /// as follows:
        /// Traverse the left subtree
        /// Traverse the right subtree
        /// Visit the root
        /// </summary>
        /// <param name="action">The action to perform that currently visited node. It is assumed that
        /// action with neither delete the current node nor any of its ancestors from the tree.</param>
        public void TraversePostorder(Action<BinaryTreeNode<T>> action) {
            if (null == action) {
                return;
            }

            // Let Tree be a pointer to a binary tree; this algorithm visits all nodes of the binary tree postorder,
            // making use of an auxiliary stack A.
            
            // Create two stacks:
            // toCheckChildren = The nodes that need to be checked for children
            // toVisit = The nodes, in order, that need to be visited
            Stack<BinaryTreeNode<T>> toCheckChildren = new Stack<BinaryTreeNode<T>>();
            Stack<BinaryTreeNode<T>> toVisit = new Stack<BinaryTreeNode<T>>();

            // Push root onto the stack
            toCheckChildren.Push(Root);

            // While the stack has content
            while (toCheckChildren.Count > 0) {
                BinaryTreeNode<T> node = toCheckChildren.Pop();
                toVisit.Push(node);

                // Push the left child if it exists
                if (null != node.Left) {
                    toCheckChildren.Push(node.Left);
                }

                // Push the right child if it exists
                if (null != node.Right) {
                    toCheckChildren.Push(node.Right);
                }
            }

            // While there are nodes to visit call the action delegate
            while (toVisit.Count > 0) {
                action(toVisit.Pop());
            }
        }
    }
    
    /// <summary>
    /// Represents a node of the binary tree
    /// </summary>
    /// <typeparam name="T">The type of value to save in the node of the tree.</typeparam>
    public class BinaryTreeNode<T>
    {
        /// <summary>
        /// The left child
        /// </summary>
        public BinaryTreeNode<T> Left { get; set; }
        /// <summary>
        /// The right child
        /// </summary>
        public BinaryTreeNode<T> Right { get; set; }
        /// <summary>
        /// The value of the this node
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The value to store in the node.</param>
        /// <param name="left">The left child of the node.</param>
        /// <param name="right">The right child of the node.</param>
        public BinaryTreeNode(T value, BinaryTreeNode<T> left = null, BinaryTreeNode<T> right = null) {
            Value = value;
            Left = left;
            Right = right;
        }
    }
}
