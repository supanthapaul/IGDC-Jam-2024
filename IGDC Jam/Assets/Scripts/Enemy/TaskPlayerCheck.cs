using BehaviourTrees;
using UnityEngine;

namespace Enemy
{
    public class TaskPlayerCheck : TaskNode
    {
        [SerializeField]
        private bool _shouldBeInside;

        [SerializeField]
        private float _range;

        private Transform _transform;
        private Transform _target;
       
        public TaskPlayerCheck(ITreeData treeData) : base(treeData)
        {
        }

        protected override void OnStart()
        {
            _transform ??= TreeData.GetSharedData("transform") as Transform;
            _target ??= TreeData.GetSharedData("target") as Transform;
        }

        protected override NodeState OnEvaluate()
        {
            if(Vector3.Distance(_transform.position, _target.position) > _range)
            {
                State = _shouldBeInside ? NodeState.Failure : NodeState.Success;
                return State;
            }
            State = _shouldBeInside ? NodeState.Success : NodeState.Failure;
            return State;
        }
    }
}