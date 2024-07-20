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
        private BoxCollider _boxCollider;
        
        [SerializeField]
        private int _totalHealth;
        private bool _isAlive = true;
        private int _health;
        private static readonly int Death = Animator.StringToHash("death");

        public void SetTarget(Transform target)
        {
            _target = target;
        }
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

        public int currentHealth => _health;
        public int totalHealth => _totalHealth;

        public bool isAlive { get => _isAlive; set => _isAlive = value; }

        public void TakeDamage(int damage)
        {
            if (!isAlive)
                return;
            _health -= damage;
            
            if (_health > 0) return;

            _boxCollider.enabled = false;
            _animator.SetTrigger(Death);
            isAlive = false;
        }

        protected override void Update()
        {
            if (!isAlive) return;
            base.Update();
            _weapon.WeaponLogic();
        }

        public void Fire()
        {
            _weapon.onFireStart?.OnFireInputStart();
            _weapon.onFireReleased?.OnFireInputReleased();
        }

        public void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
