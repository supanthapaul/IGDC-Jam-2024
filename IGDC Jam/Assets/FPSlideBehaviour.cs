using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSlideBehaviour : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform orientation;

    [Header("Sliding")]
    [SerializeField] private float slideForce = 400f;
    [SerializeField] private float maxSlideTime = 0.4f;
    [SerializeField] private float slideScale = 0.5f;    
    
    [Header("Key Binds")]
    [SerializeField] private KeyCode slideKey = KeyCode.LeftControl;
    
    private Rigidbody _rb;
    private FPController _controller;
    private float _slideTimer;
    private float _initialScale;
    private float _horizontalInput;
    private float _verticalInput;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _controller = GetComponent<FPController>();
        _initialScale = transform.localScale.y;
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        if(Input.GetKeyDown(slideKey) && (_horizontalInput != 0f || _verticalInput != 0f))
            StartSlide();
        if(Input.GetKeyUp(slideKey) && _controller.sliding)
            StopSlide();
    }

    private void FixedUpdate()
    {
        if(_controller.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        _slideTimer = maxSlideTime;
        _controller.sliding = true;
        transform.localScale = new Vector3(transform.localScale.x, slideScale, transform.localScale.z);
        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }
    
    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (!_controller.IsOnSlope() || _rb.velocity.y > -0.1f)
        {
            _rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            _slideTimer -= Time.deltaTime;
        }
        else
        {
            _rb.AddForce(_controller.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }
        
        if (_slideTimer <= 0)
        {
            StopSlide();
        }
    }
    
    private void StopSlide()
    {
        _controller.sliding = false;
        transform.localScale = new Vector3(transform.localScale.x, _initialScale, transform.localScale.z);
    }
}
