using UnityEngine;

namespace BehaviourTrees
{
    public class LogNode : TaskNode
    {
        public string Message;
        public LogNode(ITreeData treeData, string message) : base(treeData)
        {
            this.Message = message;
        }

        protected override NodeState OnEvaluate()
        {
            Debug.Log(Message);
            return NodeState.Success;
        }
    }
}