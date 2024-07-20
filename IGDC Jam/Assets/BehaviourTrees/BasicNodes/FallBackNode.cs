using System.Collections.Generic;

namespace BehaviourTrees
{
    public class FallBackNode : CompositeNode
    {
        private int _currentChildIndex;
        public FallBackNode(ITreeData treeData, List<Node> children) : base(treeData, children)
        {
        }

        protected override void OnStart()
        {
            _currentChildIndex = 0;
        }

        protected override NodeState OnEvaluate()
        {
            var state = Children[_currentChildIndex].Evaluate();
            if (state == NodeState.Running)
            {
                State = NodeState.Running;
                return State;
            }
            if(state == NodeState.Success)
            {
                State = NodeState.Success;
                return State;
            }
            _currentChildIndex++;
            if (_currentChildIndex >= Children.Count)
            {
                State = NodeState.Failure;
                return State;
            }

            State = NodeState.Running;
            return State;
        }
    }
}