using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(FPController))]
[RequireComponent(typeof(Rigidbody))]
public class WallRunningBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    
    [Header("Wall Running")]
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float wallRunForce;
    [SerializeField] private float wallJumpUpForce;
    [SerializeField] private float wallJumpSideForce;
    [SerializeField] private float maxWallRunTime;
    
    [Header("Detection")]
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float minJumpHeight;
    
    [Header("Exiting")]
    [SerializeField] private float exitWallTime;
    
    
    [Header("Key Binds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    
    
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    private bool _wallLeft;
    private bool _wallRight;
    private float _horizontalInput;
    private float _verticalInput;
    private float _wallRunTimer;
    private FPController _controller;
    private Rigidbody _rb;
    private float _exitWallTimer;
    private bool _exitingWall;

    private void Start()
    {
        _controller = GetComponent<FPController>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(_controller.wallRunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        _wallRight = Physics.Raycast(transform.position, orientation.right, out _rightWallHit, wallCheckDistance, whatIsWall);
        _wallLeft = Physics.Raycast(transform.position, -orientation.right, out _leftWallHit, wallCheckDistance, whatIsWall);
    }
    
    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        if ((_wallLeft || _wallRight) && _verticalInput > 0f && AboveGround() && !_exitingWall)
        {
            if(!_controller.wallRunning)
                StartWallRun();

            if (Input.GetKeyDown(jumpKey))
            {
                WallJump();
            }
        } 
        else if (_exitingWall)
        {
            if(_controller.wallRunning)
                StopWallRun();

            if(_exitWallTimer > 0f)
                _exitWallTimer -= Time.deltaTime;
            if (_exitWallTimer <= 0)
                _exitingWall = false;
        }
        else
        {
            if(_controller.wallRunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        _controller.wallRunning = true;
    }

    private void WallRunningMovement()
    {
        _rb.useGravity = false;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        
        Vector3 wallNormal = _wallLeft ? _leftWallHit.normal : _rightWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        
        _rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        
        if(!(_wallLeft && _horizontalInput > 0f) && !(_wallRight && _horizontalInput < 0f)) 
            _rb.AddForce(-wallNormal * 100f, ForceMode.Force);
    }

    private void StopWallRun()
    {
        _controller.wallRunning = false;
    }

    private void WallJump()
    {
        _exitingWall = true;
        _exitWallTimer = exitWallTime;
        
        Vector3 wallNormal = _wallLeft ? _leftWallHit.normal : _rightWallHit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
