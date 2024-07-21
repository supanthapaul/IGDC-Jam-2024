using System.Collections.Generic;

namespace BehaviourTrees
{
    public class Selector : CompositeNode
    {
        protected Selector(ITreeData treeData, List<Node> children) : base(treeData, children)
        {
        }

        protected override NodeState OnEvaluate()
        {
            foreach (Node child in Children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        State = NodeState.Success;
                        return State;
                    case NodeState.Running:
                        State = NodeState.Running;
                        return State;
                    default:
                        State = NodeState.Success;
                        return State;
                }
            }

            State = NodeState.Failure;
            return State;
        }
    }
}