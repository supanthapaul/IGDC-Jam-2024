using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private EnemyRunner _enemyRunner;

        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private float _range;
        private int _currentEnemyCount;
        public event Action OnAllEnemiesDeathEvent;

        public void SpawnEnemies(int amount)
        {
            var target = GameManager.Instance.playerController.transform;
            for (int i = 0; i < amount; i++)
            {
                var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                var spawnPointPosition = spawnPoint.position;
                Vector3 offset = new Vector3(Random.Range(0, _range), spawnPointPosition.y, Random.Range(0, _range));
                var enemy = Instantiate(_enemyRunner, spawnPointPosition + offset, spawnPoint.rotation);
                enemy.OnDeathEvent += OnEnemyDeath;
                enemy.SetTarget(target);
            }

            _currentEnemyCount = amount;
        }

        private void OnEnemyDeath()
        {
            _currentEnemyCount--;
            if (_currentEnemyCount <= 0)
            {
                OnAllEnemiesDeathEvent?.Invoke();
            }
        }
    }
}