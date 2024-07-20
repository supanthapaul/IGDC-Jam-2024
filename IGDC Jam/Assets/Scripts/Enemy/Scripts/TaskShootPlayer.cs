using BehaviourTrees;
using UnityEngine;

namespace Enemy
{
    public class TaskShootPlayer : TaskNode
    {
        private Transform _target;
        private Transform _weaponTip;
        private Animator _animator;
        private static readonly int IsFiring = Animator.StringToHash("isFiring");

        public TaskShootPlayer(ITreeData treeData) : base(treeData)
        {
        }

        protected override void OnStart()
        {
            _weaponTip = TreeData.GetSharedData("weaponTip") as Transform;
            _target = TreeData.GetSharedData("target") as Transform;
            _animator = TreeData.GetSharedData("animator") as Animator;
        }

        protected override NodeState OnEvaluate()
        {
            _weaponTip.LookAt(_target);
            _animator.SetBool(IsFiring, true);
            State = NodeState.Success;
            return State;
        }
    }
}