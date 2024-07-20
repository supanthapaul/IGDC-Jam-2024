using BehaviourTrees;
using UnityEngine;

namespace Enemy
{
    public class TaskPlayerVisible : TaskNode
    {
        [SerializeField]
        private bool _shouldBeVisible;

        [SerializeField]
        private LayerMask _ignoreLayers;
        
        private Transform _target;
        private Transform _transform;
        public TaskPlayerVisible(ITreeData treeData) : base(treeData)
        {
        }

        protected override void OnStart()
        {
            _target ??= TreeData.GetSharedData("target") as Transform;
            _transform ??= TreeData.GetSharedData("transform") as Transform;
        }

        protected override NodeState OnEvaluate()
        {
            if(Physics.Linecast(_transform.position, _target.position, out RaycastHit hit, ~_ignoreLayers))
            {
                State = hit.transform.CompareTag("Player") == _shouldBeVisible ? NodeState.Success : NodeState.Failure;
                return State;
            }
            State = NodeState.Failure;
            return State;
        }
    }
}
