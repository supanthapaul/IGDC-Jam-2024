using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BehaviourTrees
{
    [CreateAssetMenu(fileName = "BehaviourTree", menuName = "BehaviourTrees/BehaviourTree")]
    public class BehaviourTree : ScriptableObject, ITreeData
    {
        public Node root;
        private readonly Dictionary<string, object> _sharedData = new();
        public List<Node> nodes = new();

        public NodeState Update()
        {
            return root.Evaluate();
        }
        
        public void SetSharedData(string key, object value)
        {
            _sharedData[key] = value;
        }

        public object GetSharedData(string key)
        {
            return _sharedData.GetValueOrDefault(key);
        }

        public Node GetRootNode()
        {
            return root;
        }

        private void Traverse(Node node, Action<Node> visitor)
        {
            if (node)
            {
                visitor.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n)=>Traverse(n, visitor));
            }
        }
        public BehaviourTree Clone()
        {
            var tree = Instantiate(this);
            tree.root = tree.root.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.root, node =>
            {
                tree.nodes.Add(node);
            });
            return tree;
        }
        
        public void SetTreeData()
        {
            root.SetTreeData(this);
        }
        
        #if UNITY_EDITOR
        public Node CreateNode(System.Type type)
        {
            Debug.Assert(type != null, nameof(type) + " != null");
            var node = CreateInstance(type) as Node;
            Debug.Assert(node != null, nameof(node) + " != null");
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            node.TreeData = this;
            Undo.RecordObject(this, "Behaviour Tree (Create Node)");
            nodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node, this);
            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (Create Node)");
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Behaviour Tree (Delete Node)");
            nodes.Remove(node);
            // AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child)
        {
            if(parent is DecoratorNode decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (Add Child)");
                child.parent = decorator;
                decorator.child = child;
                EditorUtility.SetDirty(decorator);
            }
            else if (parent is CompositeNode composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (Add Child)");
                child.parent = composite;
                composite.Children ??= new List<Node>();
                composite.Children.Add(child);
                EditorUtility.SetDirty(composite);
            }
            else if (parent is RootNode rootNode)
            {
                Undo.RecordObject(rootNode, "Behaviour Tree (Add Child)");
                child.parent = rootNode;
                rootNode.child = child;
                EditorUtility.SetDirty(rootNode);
            }
        }
        public void RemoveChild(Node parent, Node child)
        {
            if(parent is DecoratorNode decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (Remove Child)");
                child.parent = null;
                decorator.child = null;
                EditorUtility.SetDirty(decorator);
            }
            else if (parent is CompositeNode composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (Remove Child)");
                child.parent = null;
                composite.Children.Remove(child);
                EditorUtility.SetDirty(composite);
            }
            else if (parent is RootNode rootNode)
            {
                Undo.RecordObject(rootNode, "Behaviour Tree (Remove Child)");
                child.parent = null;
                rootNode.child = null;
                EditorUtility.SetDirty(rootNode);
            }
        }

        #endif
        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();
            if(parent is DecoratorNode decorator && decorator.child!=null)
            {
                children.Add(decorator.child);
            }
            else if (parent is CompositeNode composite)
            {
                children.AddRange(composite.Children);
            }
            else if (parent is RootNode rootNode)
            {
                children.Add(rootNode.child);
            }
            
            return children;
        }
    }
}