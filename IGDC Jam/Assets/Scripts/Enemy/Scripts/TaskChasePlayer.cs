using BehaviourTrees;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class TaskChasePlayer : TaskNode
    {
        [SerializeField]
        private float _chaseTill;

        [SerializeField]
        private LayerMask _ignoreLayers;
    
        private Transform _target;
        private NavMeshAgent _agent;
    
        public TaskChasePlayer(ITreeData treeData) : base(treeData)
        {
        }

        protected override void OnStart()
        {
            _target ??= TreeData.GetSharedData("target") as Transform;
            _agent ??= TreeData.GetSharedData("agent") as NavMeshAgent;
        }

        protected override NodeState OnEvaluate()
        {
            if (Vector3.Distance(_agent.transform.position, _target.position) > _chaseTill)
            {
                return RunToPlayer();
            }

            if (!IsPlayerVisible())
            {
                return RunToPlayer();
            }

            _agent.isStopped = true;
            State = NodeState.Success;
            return State;
        }

        private NodeState RunToPlayer()
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
            State = NodeState.Running;
            return State;
        }

        private bool IsPlayerVisible()
        {
            if(Physics.Linecast(_agent.transform.position, _target.position, out RaycastHit hit, ~_ignoreLayers))
            {
                return hit.transform.CompareTag("Player");
            }

            return false;
        }
    }
}
