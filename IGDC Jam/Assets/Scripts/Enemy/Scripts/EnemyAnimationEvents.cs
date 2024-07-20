using UnityEngine;

namespace Enemy
{
    public class EnemyAnimationEvents : MonoBehaviour
    {
        [SerializeField]
        private EnemyRunner _enemyRunner;
        private void OnFire()
        {
            _enemyRunner.Fire();
        }
    }
}
