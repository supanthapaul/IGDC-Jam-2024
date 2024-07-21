using UnityEngine;

namespace LockAndDoor
{
    public class Lock : MonoBehaviour
    {
        [SerializeField, SerializeReference]
        private IOpener _door;
        
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
        private int glowMatIndex;
        [SerializeField]
        private Gradient _keepOpentransitionGradient;
        [SerializeField]
        private Gradient _transitionGradient;
        [SerializeField]
        private float _emissionIntensity = 4f;
        

        private bool _lock;
   
        private float _decayTimer = 0f;
        private Material _material;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private void Start()
        {
            _material = _meshRenderer.materials[glowMatIndex];
        }

        public void Fill()
        {
            _unlock = Mathf.Clamp01(_unlock + Time.deltaTime/_totalTime);
            
            _decayTimer = 0f; // Reset decay timer when Fill is called
            if (_keepOpen)
            {
                _material.color = _keepOpentransitionGradient.Evaluate(_unlock);
                _material.SetColor(EmissionColor, _keepOpentransitionGradient.Evaluate(_unlock) * _emissionIntensity);
            }
            else
            {
                _material.color = _transitionGradient.Evaluate(_unlock);
                _material.SetColor(EmissionColor, _transitionGradient.Evaluate(_unlock) * _emissionIntensity);
            }
            if (!(_unlock >= 1f)) return;
            _lock = _keepOpen;
            _door.SetOpen(true);
        }

        private void Update()
        {
            if (_lock) return;
            _decayTimer += Time.deltaTime;

            if (_decayTimer > _decayThreshold)
            {
                // Apply decay
                float decayAmount = _decayRate * Time.deltaTime;
                _unlock = Mathf.Max(0f, _unlock - decayAmount);
                _door.SetOpen(false);
                if (_keepOpen)
                {
                    _material.color = _keepOpentransitionGradient.Evaluate(_unlock);
                    _material.SetColor(EmissionColor, _keepOpentransitionGradient.Evaluate(_unlock) * _emissionIntensity);
                }
                else
                {
                    _material.color = _transitionGradient.Evaluate(_unlock);
                    _material.SetColor(EmissionColor, _transitionGradient.Evaluate(_unlock) * _emissionIntensity);
                }
            }
        }
    }
}
