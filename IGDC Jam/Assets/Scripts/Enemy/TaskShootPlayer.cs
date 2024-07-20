using BehaviourTrees;
using UnityEngine;

namespace Enemy
{
    public class TaskShootPlayer : TaskNode
    {
        public TaskShootPlayer(ITreeData treeData) : base(treeData)
        {
        }

        protected override NodeState OnEvaluate()
        {
            Debug.Log("Pew Pew");
            State = NodeState.Success;
            return State;
        }
    }
}