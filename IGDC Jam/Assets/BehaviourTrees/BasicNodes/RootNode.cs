namespace BehaviourTrees
{
    public class RootNode : Node
    {
        public Node child;
        public RootNode(ITreeData treeData) : base(treeData)
        {
        }

        protected override NodeState OnEvaluate()
        {
            return child.Evaluate();
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