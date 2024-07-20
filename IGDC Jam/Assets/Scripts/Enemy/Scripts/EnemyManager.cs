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

        public void SpawnEnemies(int amount, Transform target)
        {
            for (int i = 0; i < amount; i++)
            {
                var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                var spawnPointPosition = spawnPoint.position;
                Vector3 offset = new Vector3(Random.Range(0, _range), spawnPointPosition.y, Random.Range(0, _range));
                var enemy = Instantiate(_enemyRunner, spawnPointPosition + offset, spawnPoint.rotation);
                enemy.SetTarget(target);
            }
        }

        [Header("Testing")]
        [SerializeField]
        private Transform _player;

        [SerializeField]
        private int _amount;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnEnemies(_amount, _player);
            }
        }
    }
}