using System;
using UnityEngine;

namespace LockAndDoor
{
    public class Lock : MonoBehaviour
    {
        [SerializeField]
        private Door _door;
        
        [SerializeField]
        private float _totalTime;
        
        [Tooltip("Rate at which the value decays per second")]
        [SerializeField]
        private float _decayRate = 0.5f; // 
        
        [Tooltip("Time in seconds before decay starts")]
        [SerializeField]
        private float _decayThreshold = 2f; 

        [SerializeField, Range(0, 1)]
        private float _unlock;

        [SerializeField]
        private bool _keepOpen;

        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private Gradient _transitionGradient;

        private bool _lock;
   
        private float _decayTimer = 0f;
        private Material _material;

        private void Start()
        {
            _material = _meshRenderer.material;
        }

        public void Fill()
        {
            _unlock = Mathf.Clamp01(_unlock + Time.deltaTime/_totalTime);
            _material.color = _transitionGradient.Evaluate(_unlock);
            _decayTimer = 0f; // Reset decay timer when Fill is called
            
            if (!(_unlock >= 1f)) return;
            _lock = _keepOpen;
            _door.SetDoor(true);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Fill();
            }
            if (_lock) return;
            _decayTimer += Time.deltaTime;

            if (_decayTimer > _decayThreshold)
            {
                // Apply decay
                float decayAmount = _decayRate * Time.deltaTime;
                _unlock = Mathf.Max(0f, _unlock - decayAmount);
                _material.color = _transitionGradient.Evaluate(_unlock);
                _door.SetDoor(false);
            }
        }
    }
}
