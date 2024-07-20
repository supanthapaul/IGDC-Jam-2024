namespace BehaviourTrees
{
    public abstract class DecoratorNode : Node
    {
        public Node child;
        protected DecoratorNode(ITreeData treeData, Node child) : base(treeData)
        {
            this.child = child;
        }

        public override Node Clone()
        {
            var node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }

        public override void SetTreeData(ITreeData treeData)
        {
            base.SetTreeData(treeData);
            child.SetTreeData(treeData);
        }
    }
}