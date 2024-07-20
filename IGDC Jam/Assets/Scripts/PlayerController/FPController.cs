using Health_System;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PlayerPrefStatics;

public enum FPState
{
    Walking,
    InAir,
    Crouching,
    Sliding,
    WallRunning,
    Dashing
}

[RequireComponent(typeof(Rigidbody))]
public class FPController : AbilityUpdate, IHealth
{
    [Header("Restrictions")]
    public bool hasForward;
    public bool hasStrafe;
    public bool hasJump;
    public bool hasWallrun;
    public bool hasDash;
    public bool hasCrouch;
    public bool hasSlide;

    [Header("Health")]
    public int maxHealth;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float wallRunSpeed;
    [SerializeField] private float dashSpeed;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchScale = 0.5f;

    [Header("Dashing")]
    [SerializeField] private bool canDash = true;  //different from has dash, checks if dash is on cooldown
    [SerializeField] private float dashForce;
    [SerializeField] private float dashUpwardForce;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashPeriod;
    private float _remainingDashPeriod;
    private float _currentDashCooldown;

    [Header("Key Binds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode dashKey = KeyCode.E;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    bool _isGrounded;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float slopeSpeedIncreaseMultiplier = 1.5f;
    private RaycastHit _slopeHit;

    [SerializeField] private Transform orientation;
    public WeaponsHolder weaponsHolder;

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
    private float _lastDesiredMoveSpeed;
    private bool dashing;

    public int currentHealth { get; private set; }

    public int totalHealth { get; set; }

    public bool isAlive {get; set;}

    private void Start()
    {
        SetUpRestrictions();
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _isReadyToJump = true;
        _initialScale = transform.localScale.y;

        isAlive = true;
        currentHealth = totalHealth = maxHealth;
        GameManager.Instance.playerController = this;
    }


    [ContextMenu("Take Away All Translation Abilities")]
    private void SetAllRestrictionsToFalse()
    {
        PlayerPrefs.SetInt(ForwardRestriction, 0);
        PlayerPrefs.SetInt(StrafeRestriction, 0);
        PlayerPrefs.SetInt(JumpRestriction, 0);
        PlayerPrefs.SetInt(CrouchRestriction, 0);
        PlayerPrefs.SetInt(DashRestriction, 0);
        PlayerPrefs.SetInt(WallRunRestriction, 0);
        PlayerPrefs.SetInt(SlideRestriction, 0);

        SetUpRestrictions();
    }


    [ContextMenu("Give All Translation Abilities")]
    private void SetAllRestrictionsToTrue()
    {
        PlayerPrefs.SetInt(ForwardRestriction, 1);
        PlayerPrefs.SetInt(StrafeRestriction, 1);
        PlayerPrefs.SetInt(JumpRestriction, 1);
        PlayerPrefs.SetInt(CrouchRestriction, 1);
        PlayerPrefs.SetInt(DashRestriction,  1);
        PlayerPrefs.SetInt(WallRunRestriction, 1);
        PlayerPrefs.SetInt(SlideRestriction, 1);

        SetUpRestrictions();
    }

    public override void SetUpRestrictions()
    {
        hasForward =        PlayerPrefs.GetInt(ForwardRestriction, 0) == 1;
        hasStrafe =         PlayerPrefs.GetInt(StrafeRestriction, 0) == 1;
        hasJump =           PlayerPrefs.GetInt(JumpRestriction, 0) == 1;
        hasCrouch =         PlayerPrefs.GetInt(CrouchRestriction, 0) == 1;
        hasSlide =        PlayerPrefs.GetInt(SlideRestriction, 0) == 1;
        hasWallrun =        PlayerPrefs.GetInt(WallRunRestriction, 0) == 1;
        hasDash =           PlayerPrefs.GetInt(DashRestriction, 0) == 1;
    }

    private void Update()
    {
        if (!isAlive) return;
        // ground check
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        HandleInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (_isGrounded&&!dashing)
            _rb.drag = groundDrag;
        else
            _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if(!isAlive) return;

        MovePlayer();
    }

    private void HandleInput()
    {
        if(hasStrafe)
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
        }

        if(hasForward)
        {
            _verticalInput = Input.GetAxisRaw("Vertical");
        }
        

        if(hasJump)
        {
            if(Input.GetKey(jumpKey) && _isReadyToJump && _isGrounded)
            {
                _isReadyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }

        if(hasCrouch)
        {
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

        if(hasDash)
        {
            if(!canDash)
            {
                _currentDashCooldown -= Time.deltaTime;
                if(_currentDashCooldown < 0f)
                {
                    canDash = true;                    
                }
            }

            if(dashing)
            {
                _remainingDashPeriod -= Time.deltaTime;
                if(_remainingDashPeriod < 0f)
                {
                    dashing = false;
                }
            }

            if(Input.GetKeyDown(dashKey) && canDash)
            {
                Debug.Log("Dashed");
                Dash();
            }
        }
    }

    private void StateHandler()
    {
        if (wallRunning)
        {
            state = FPState.WallRunning;
            _desiredMoveSpeed = wallRunSpeed;
        }
        else if (dashing)
        {
            state = FPState.Dashing;
            _desiredMoveSpeed = dashSpeed;
        }
        else if (sliding)
        {
            state = FPState.Sliding;
            if (IsOnSlope() && _rb.velocity.y < 0.1f)
                _desiredMoveSpeed = slideSpeed;

        }
        else if (Input.GetKey(crouchKey))
        {
            state = FPState.Crouching;
            _desiredMoveSpeed = crouchSpeed;
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
            _rb.AddForce(_moveSpeed * 20f * GetSlopeMoveDirection(_moveDirection), ForceMode.Force);
            if(_rb.velocity.y > 0) 
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if(_isGrounded)
            _rb.AddForce(_moveSpeed * 10f * _moveDirection.normalized, ForceMode.Force);
        else if(!_isGrounded)
            _rb.AddForce(_moveSpeed * 10f * airMultiplier * _moveDirection.normalized, ForceMode.Force);
        
        _rb.useGravity = !IsOnSlope();
    }

    private void SpeedControl()
    {
        if (dashing) return;

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

    private void Dash()
    {
        dashing = true;
        canDash = false;
        _currentDashCooldown = dashCooldown;
        _remainingDashPeriod = dashPeriod;
        _rb.AddForce(_moveDirection*dashForce + orientation.up*dashUpwardForce, ForceMode.VelocityChange);
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

    public void JumpPadLogic(float upwardForce)
    {
        _rb.velocity.Set(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            isAlive = false;
            GameManager.Instance.PlayerDeath();
        }
    }
}
