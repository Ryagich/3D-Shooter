using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    [SerializeField, Range(0.0f, 50.0f)] private float _maxStepSpeed = 4.0f;
    [SerializeField, Range(0.0f, 10.0f)] private float _stepAcceleration = 8.0f;
    [SerializeField, Range(0.0f, 0.3f)] private float _expDragXZ = .015f;
    [SerializeField, Range(0.0f, 0.3f)] private float _linearDragXZ = .05f;
    [SerializeField, Range(0.0f, 20.0f)] private float _gravity = 10.0f;
    [SerializeField, Range(0.0f, 10.0f)] private float _jumpPower = 4.5f;

    [SerializeField] private Vector3 velocity = Vector3.zero; // Debug SerializeField

    private CharacterController characterC;

    private void Awake()
    {
        characterC = GetComponent<CharacterController>();
        InputHandler.OnPressSpace += Jump;
    }

    private void OnDestroy()
    {
        InputHandler.OnPressSpace -= Jump;
    }

    private void FixedUpdate()
    {
        if (!characterC.isGrounded)
            velocity.y -= _gravity * Time.fixedDeltaTime;

        var deltaV = transform.rotation * (InputHandler.Movement.normalized * Time.fixedDeltaTime * _stepAcceleration);

        if (deltaV.sqrMagnitude < 0.01f)
        {
            var scale = Vector3.one - new Vector3(_expDragXZ, 0, _expDragXZ);
            velocity = Vector3.Scale(velocity, scale);
            velocity = Vector3.MoveTowards(velocity, Vector3.zero, _linearDragXZ);
        }
        velocity += deltaV;
        var vy = velocity.y;
        velocity = Vector3.ClampMagnitude(velocity.WithY(0), _maxStepSpeed).WithY(vy);

        characterC.Move(velocity * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        if (characterC.isGrounded)
            velocity.y = _jumpPower;
    }
}
