using System.Collections.Generic;

namespace BehaviourTrees
{
    public class Sequence : CompositeNode
    {
        private int _currentChild;
        public Sequence(ITreeData treeData, List<Node> children) : base(treeData, children)
        {
        }

        protected override void OnStart()
        {
            _currentChild = 0;
        }

        protected override NodeState OnEvaluate()
        {
            if (_currentChild >= Children.Count)
            {
                State = NodeState.Success;
                return State;
            }

            var state = Children[_currentChild].Evaluate();
            if (state == NodeState.Failure)
            {
                State = NodeState.Failure;
                return State;
            }
            if(state == NodeState.Success)
            {
                _currentChild++;
            }
            State = NodeState.Running;
            return State;
        }

    }
}