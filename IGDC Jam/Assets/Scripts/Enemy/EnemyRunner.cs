using BehaviourTrees;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyRunner : BehaviourTreeRunner
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private LookAtTarget _lookAtTarget;
        protected override void Start()
        {
            base.Start();
            BehaviourTree.SetSharedData("transform", transform);
            BehaviourTree.SetSharedData("target", _target);
            BehaviourTree.SetSharedData("agent", GetComponent<NavMeshAgent>());
            BehaviourTree.SetTreeData();
            
            _lookAtTarget.LookAt(_target);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 5);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 10);
        }
    }
}
