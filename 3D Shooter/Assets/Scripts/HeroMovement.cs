using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    [SerializeField, Range(0.0f, 50.0f)] private float _maxSpeed, _acceleration;
    [SerializeField, Range(0.0f, 1.0f)] private float _linearDrag;
    [SerializeField, Range(0.0f, 50.0f)] private float _gravity;
    [SerializeField, Range(0.0f, 10.0f)] private float _jumpPower = 5.0f;

    private Vector3 velocity = Vector3.zero;
    private CharacterController characterC;

    private void Awake()
    {
        characterC = GetComponent<CharacterController>();
        InputHandler.OnMove += Move;
        InputHandler.OnPressSpace += Jump;
    }

    private void Update()
    {
        characterC.Move(velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        velocity.y -= _gravity * Time.deltaTime;
    }

    private void Move(Vector3 move)
    {
        velocity += transform.rotation * move * _acceleration * Time.deltaTime;
        velocity *= 1 - _linearDrag;
        velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);
    }

    private void Jump()
    {
        if (characterC.isGrounded)
            velocity.y = _jumpPower;
    }
}
