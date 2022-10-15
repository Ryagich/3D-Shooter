using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    [SerializeField, Range(0.0f, 10.0f)] private float _speed;
    [SerializeField, Range(0.0f, 10.0f)] private float _gravity;

    private Vector3 velocity = Vector3.zero;
    private CharacterController characterC;

    private void Awake()
    {
        characterC = GetComponent<CharacterController>();
    }

    private void Update()
    {
        velocity = transform.rotation
                 * new Vector3(Input.GetAxis("Horizontal"), 0,
                               Input.GetAxis("Vertical"));

        velocity.y -= _gravity * Time.deltaTime;
        characterC.Move(velocity * Time.deltaTime);
    }
}
