using System;
using System.Collections.Generic;

namespace BehaviourTrees
{
    public class InterruptSequence : CompositeNode
    {
        public InterruptSequence(ITreeData treeData, List<Node> children) : base(treeData, children)
        {
        }

        protected override NodeState OnEvaluate()
        {
            foreach (var child in Children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Success:
                        continue;
                    case NodeState.Failure:
                        State = NodeState.Failure;
                        return State;
                    case NodeState.Running:
                        State = NodeState.Running;
                        return State;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            State = NodeState.Success;
            return NodeState.Success;
        }
    }
}