using UnityEngine;

namespace Traps
{
    public class DeathLaser : MonoBehaviour
    {
        private enum LaserState
        {
            Stationary,
            Moving,
        }

        private enum MovementType
        {
            Single,
            BounceBack,
            Loop,
            Repeating,
        }

        [SerializeField]
        private Transform _laser;
        
        [SerializeField]
        private Transform[] _wayPoints;

        [SerializeField]
        private LaserState _laserState;
        [SerializeField]
        private MovementType _movementType;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private bool _autoStart;
    
        private bool _canMove;
        private bool _isRunning;
        private int _currentWayPoint;
        private int _incrementor = 1;
        private Transform _target;

        private void Start()
        {
            if (_laserState == LaserState.Stationary)
            {
                _canMove = false;
                return;
            }

            if (!_autoStart) return;
            _canMove = true;
            _currentWayPoint = 0;
            _target = _wayPoints[_currentWayPoint];
        }

        public void StartMove()
        {
            if (_laserState == LaserState.Stationary)
                return;
            _canMove = true;
        }
        public void StopMove()
        {
            _canMove = false;
        }

        private void Update()
        {
            if (!_canMove) return;
            if (Vector3.Distance(_target.position, _laser.position) < 0.1f)
            {
                _laser.position = _target.position;
                CheckAndUpdateTarget();
            }
            else
            {
                _laser.position = Vector3.MoveTowards(_laser.position, _target.position, _speed * Time.deltaTime);
            }
        }

        private void CheckAndUpdateTarget()
        {
            switch (_movementType)
            {
                case MovementType.Single:
                    _canMove = false;
                    return;
                case MovementType.BounceBack:
                    if(_currentWayPoint == 0 && _incrementor == -1)
                        _incrementor = 1;
                    else if(_currentWayPoint == _wayPoints.Length - 1 && _incrementor == 1)
                        _incrementor = -1;
                    _currentWayPoint += _incrementor;
                    break;
                case MovementType.Loop:
                    _currentWayPoint = (_currentWayPoint + 1) % _wayPoints.Length;
                    break;
                case MovementType.Repeating:
                    if (_currentWayPoint == _wayPoints.Length - 1)
                    {
                        _laser.position = _wayPoints[0].position;
                        _currentWayPoint = 0;
                    }
                    _currentWayPoint++;
                    break;
            }
            _target = _wayPoints[_currentWayPoint];
        }
    }
}
