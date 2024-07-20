using UnityEngine;

namespace BehaviourTrees
{
    public enum NodeState
    {
        Success,
        Failure,
        Running
    }
    public abstract class Node : ScriptableObject
    {
        public Node parent;
        [HideInInspector]
        public string guid;
        [HideInInspector]
        public Vector2 position;

        protected NodeState State;

        public ITreeData TreeData;
        
        private bool _hasStarted;
        public string description;
        public NodeState NodeState => State;
        public bool Started => _hasStarted;

        protected Node(ITreeData treeData)
        {
            parent = null;
            TreeData = treeData;
        }
        
        public  NodeState Evaluate()
        {
            if (!_hasStarted)
            {
                OnStart();
                _hasStarted = true;
            }

            State = OnEvaluate();

            if (State == NodeState.Success || State == NodeState.Failure)
            {
                _hasStarted = false;
                OnComplete();
            }

            return State;
        }

        public virtual Node Clone()
        {
            var node = Instantiate(this);
            return node;
        }

        public virtual void SetTreeData(ITreeData treeData)
        {
            TreeData = treeData;
        }

        protected virtual void OnStart(){}
        protected virtual NodeState OnEvaluate() => NodeState.Failure;
        protected virtual void OnComplete(){}
    }
}
