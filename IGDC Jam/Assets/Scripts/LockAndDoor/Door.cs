using DG.Tweening;
using UnityEngine;

namespace LockAndDoor
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private Transform _leftDoor;

        [SerializeField]
        private Transform _rightDoor;

        [SerializeField]
        private float _openWidth;

        private float _leftStartX;
        private float _rightStartX;
        private bool _doorState;

        private void Start()
        {
            _leftStartX = _leftDoor.localPosition.x;
            _rightStartX = _rightDoor.localPosition.x;
        }

        public void SetDoor(bool isOpen)
        {
            if (_doorState == isOpen) return;

            _doorState = isOpen;
            _leftDoor.DOLocalMoveX(isOpen ? -_openWidth: 0 + _leftStartX, 1f);
            _rightDoor.DOLocalMoveX(isOpen ? _openWidth: 0 + _rightStartX, 1f);
        }

    }
}