using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;
    public Transform cameraTransform;
    public Joystick joystick;
    
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Check if the player is grounded
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Small downward force to keep player grounded
        }

        // Get input from the player
#if UNITY_EDITOR && UNITY_STANDALONE_WIN
        var moveX = Input.GetAxis("Horizontal");
        var moveZ = Input.GetAxis("Vertical");
#elif UNITY_ANDROID
        var moveX = joystick.Horizontal;
        var moveZ = joystick.Vertical;
#endif

        // Get the camera's forward and right directions
        var forward = cameraTransform.forward;
        var right = cameraTransform.right;

        // Normalize the vectors to remove any unintended influence
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Calculate the direction the player should move
        var move = forward * moveZ + right * moveX;

        // Move the player
        _controller.Move(move * (speed * Time.deltaTime));

        // Rotate the player to face the camera's forward direction
        if (move != Vector3.zero)
        {
            transform.forward = forward;
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;

        // Apply movement
        _controller.Move(_velocity * Time.deltaTime);
    }
}
