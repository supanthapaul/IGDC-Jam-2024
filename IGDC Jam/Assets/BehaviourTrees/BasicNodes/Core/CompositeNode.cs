using System.Collections.Generic;

namespace BehaviourTrees
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children;
        
        protected CompositeNode(ITreeData treeData, List<Node> children) : base(treeData)
        {
            Children = new();
            foreach (var child in children)
            {
                Attach(child);
            }
        }
        private void Attach(Node node)
        {
            node.parent = this;
            Children.Add(node);
        }
        public override Node Clone()
        {
            var node = Instantiate(this);
            node.Children = new List<Node>();
            foreach (var child in Children)
            {
                node.Children.Add(child.Clone());
            }
            return node;
        }

        public override void SetTreeData(ITreeData treeData)
        {
            base.SetTreeData(treeData);
            foreach (var child in Children)
            {
                child.SetTreeData(treeData);
            }
        }
    }
}