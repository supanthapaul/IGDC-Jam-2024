using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum FPState
{
    Walking,
    Sprinting,
    InAir,
    Crouching,
    Sliding,
    Dashing,
    WallRunning
}

[RequireComponent(typeof(Rigidbody))]
public class FPController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private  float sprintSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float wallRunSpeed;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchScale = 0.5f;
    
    [Header("Key Binds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool _isGrounded;
    
    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float slopeSpeedIncreaseMultiplier = 1.5f;
    private RaycastHit _slopeHit;

    [SerializeField] private Transform orientation;

    public FPState state;
    [HideInInspector] public bool sliding;
    [HideInInspector] public bool wallRunning;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    private bool _isReadyToJump;
    private float _moveSpeed;
    private float _initialScale;
    private bool _exitingSlope;
    private float _desiredMoveSpeed;
    private float  _lastDesiredMoveSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _isReadyToJump = true;
        _initialScale = transform.localScale.y;
    }

    private void Update()
    {
        
        
        // ground check
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        HandleInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (_isGrounded)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && _isReadyToJump && _isGrounded)
        {
            _isReadyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, _initialScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        if (wallRunning)
        {
            state = FPState.WallRunning;
            _desiredMoveSpeed = wallRunSpeed;
        }
        else if (sliding)
        {
            state = FPState.Sliding;
            if (IsOnSlope() && _rb.velocity.y < 0.1f)
                _desiredMoveSpeed = slideSpeed;
            else
                _desiredMoveSpeed = sprintSpeed;

        }
        else if (Input.GetKey(crouchKey))
        {
            state = FPState.Crouching;
            _desiredMoveSpeed = crouchSpeed;
        }
        else if (_isGrounded && Input.GetKey(sprintKey))
        {
            state = FPState.Sprinting;
            _desiredMoveSpeed = sprintSpeed;
        }
        else if (_isGrounded)
        {
            state = FPState.Walking;
            _desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = FPState.InAir;
        }

        if (Mathf.Abs(_desiredMoveSpeed - _lastDesiredMoveSpeed) > 4f && _moveSpeed != 0f)
        {
            StopAllCoroutines();
            StartCoroutine(LerpMoveSpeed());
        }
        else
        {
            _moveSpeed = _desiredMoveSpeed;
            
        }
        _lastDesiredMoveSpeed = _desiredMoveSpeed;
    }

    private IEnumerator LerpMoveSpeed()
    {
        float time = 0;
        float diff = Mathf.Abs(_desiredMoveSpeed - _moveSpeed);
        float startValue = _moveSpeed;

        while (time < diff)
        {
            _moveSpeed = Mathf.Lerp(startValue, _desiredMoveSpeed, time / diff);
            time += Time.deltaTime * slopeSpeedIncreaseMultiplier;
            yield return null;
        }

        _moveSpeed = _desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (IsOnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection(_moveDirection) * _moveSpeed * 20f, ForceMode.Force);
            if(_rb.velocity.y > 0) 
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if(_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        else if(!_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f * airMultiplier, ForceMode.Force);
        
        _rb.useGravity = !IsOnSlope();
    }

    private void SpeedControl()
    {
        if (IsOnSlope() && !_exitingSlope)
        {
            if(_rb.velocity.magnitude > _moveSpeed)
                _rb.velocity = _rb.velocity.normalized * _moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            // velocity clamp
            if(flatVel.magnitude > _moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _moveSpeed;
                _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
            }
        }
        
    }

    private void Jump()
    {
        _exitingSlope = true;
        // reset y velocity
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        _isReadyToJump = true;
        _exitingSlope = false;
    }

    public bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0f;
        }

        return false;
    }
    
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }
}
