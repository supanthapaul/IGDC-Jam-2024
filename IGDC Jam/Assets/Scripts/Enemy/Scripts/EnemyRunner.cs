using BehaviourTrees;
using Health_System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyRunner : BehaviourTreeRunner, IHealth
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private LookAtTarget _lookAtTarget;

        [SerializeField]
        private Transform _weaponTip;
        
        [SerializeField]
        private Weapon _weapon;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private int _totalHealth;
        
        private int _health;
        protected override void Start()
        {
            _health = _totalHealth;
            base.Start();
            BehaviourTree.SetSharedData("transform", transform);
            BehaviourTree.SetSharedData("target", _target);
            BehaviourTree.SetSharedData("agent", GetComponent<NavMeshAgent>());
            BehaviourTree.SetSharedData("weapon", _weapon);
            BehaviourTree.SetSharedData("weaponTip", _weaponTip);
            BehaviourTree.SetSharedData("animator", _animator);
            BehaviourTree.SetTreeData();
            
            _lookAtTarget.LookAt(_target);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 15f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 20f);
        }

        public int currentHealth => _health;
        public int totalHealth => _totalHealth;
        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }

        protected override void Update()
        {
            base.Update();
            _weapon.WeaponLogic();
        }

        public void Fire()
        {
            _weapon.onFireStart?.OnFireInputStart();
            _weapon.onFireReleased?.OnFireInputReleased();
        }
    }
}
