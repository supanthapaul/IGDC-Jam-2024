using DG.Tweening;
using UnityEngine;

namespace LockAndDoor
{
    public class Door : IOpener
    {
        [SerializeField]
        private Transform _leftDoor;

        [SerializeField]
        private Transform _rightDoor;

        [SerializeField]
        private float _openWidth;
        
        [SerializeField]
        private MeshRenderer doorFrameRenderer;
        [SerializeField]
        private int glowMatIndex;

        private float _leftStartX;
        private float _rightStartX;
        private bool _doorState;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private void Start()
        {
            _leftStartX = _leftDoor.localPosition.x;
            _rightStartX = _rightDoor.localPosition.x;
        }

        [ContextMenu("Open Door")]
        public void OnOpen()
        {
            SetOpen(true);
        }

        [ContextMenu("Close Door")]
        public void OnClose()
        {
            SetOpen(false);
        }

        public override void SetOpen(bool isOpen)
        {
            if (_doorState == isOpen) return;

            _doorState = isOpen;
            doorFrameRenderer.materials[glowMatIndex].color = isOpen ? Color.green : Color.red;
            doorFrameRenderer.materials[glowMatIndex].SetColor(EmissionColor, (isOpen ? Color.green : Color.red) * 4f);
            _leftDoor.DOLocalMoveZ(isOpen ? _openWidth: 0 + _leftStartX, 1f).SetEase(Ease.InSine);
            _rightDoor.DOLocalMoveZ(isOpen ? -_openWidth: 0 + _rightStartX, 1f).SetEase(Ease.InSine);
        }
    }
}