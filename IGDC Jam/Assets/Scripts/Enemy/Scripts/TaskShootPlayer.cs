using BehaviourTrees;
using UnityEngine;

namespace Enemy
{
    public class TaskShootPlayer : TaskNode
    {
        private Weapon _weapon;
        private Transform _target;
        private Transform _weaponTip;
        public TaskShootPlayer(ITreeData treeData) : base(treeData)
        {
        }

        protected override void OnStart()
        {
            _weapon = TreeData.GetSharedData("weapon") as Weapon;
            _weaponTip = TreeData.GetSharedData("weaponTip") as Transform;
            _target = TreeData.GetSharedData("target") as Transform;
        }

        protected override NodeState OnEvaluate()
        {
            _weaponTip.LookAt(_target);
            _weapon.WeaponLogic();
            _weapon.onFireContinuous?.OnFireInputPressed();
            State = NodeState.Success;
            return State;
        }
    }
}