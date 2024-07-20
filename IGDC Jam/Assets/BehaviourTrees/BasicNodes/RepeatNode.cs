namespace BehaviourTrees
{
    public class RepeatNode : DecoratorNode
    {
        
        public RepeatNode(ITreeData treeData, Node child) : base(treeData, child)
        {
        }

        protected override NodeState OnEvaluate()
        {
            child.Evaluate();
            State = NodeState.Running;
            return State;
        }
    }
}