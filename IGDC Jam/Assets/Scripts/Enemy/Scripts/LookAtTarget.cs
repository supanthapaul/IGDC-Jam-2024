using UnityEngine;

namespace Enemy
{
    public class LookAtTarget : MonoBehaviour
    {
        private Transform _target;
        private bool _hasTarget;
        private Vector3 _lookVector;
        public void LookAt(Transform target)
        {
            _target = target;
            _hasTarget = true;
        }

        private void Update()
        {
            if (!_hasTarget) return;
            SetLookVector();
            transform.LookAt(_lookVector);
        }

        private void SetLookVector()
        {
            _lookVector = _target.position;
            _lookVector.y = transform.position.y;
        }
    }
}
