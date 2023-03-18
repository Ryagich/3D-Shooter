using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeroMovement : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 50.0f)] private float _maxStepSpeed = 4.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float _stepAcceleration = 8.0f;
    [SerializeField] [Range(0.0f, 0.3f)] private float _expDragXZ = .015f;
    [SerializeField] [Range(0.0f, 0.3f)] private float _linearDragXZ = .05f;
    [SerializeField] [Range(0.0f, 20.0f)] private float _gravity = 10.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float _jumpPower = 4.5f;
    [SerializeField] [Range(0, 5)] private float _sittingHeight = 1;

    [SerializeField] private Vector3 velocity = Vector3.zero; // Debug SerializeField
    private readonly Collider[] standUpCheckOverlapResult = new Collider[2];

    private CharacterController characterC;

    private float standingHeight;
    private bool standUpRequest;

    private void Awake()
    {
        characterC = GetComponent<CharacterController>();
        InputHandler.SpacePressed += Jump;
        InputHandler.LeftCtrlDowned += SitDown;
        InputHandler.LeftCtrlUped += StandUp;
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

        if (standUpRequest && TryStandUp())
            standUpRequest = false;
    }

    private void OnDestroy()
    {
        InputHandler.SpacePressed -= Jump;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
    }

    private void StandUp()
    {
        standUpRequest = true;
    }

    private void SitDown()
    {
        if (standUpRequest)
            return;

        standingHeight = characterC.height;
        characterC.height = _sittingHeight;
        var offsetY = (standingHeight - _sittingHeight) / 2;
        characterC.center = new Vector3(0, offsetY, 0);
        standUpRequest = false;
    }

    private bool TryStandUp()
    {
        var heroYOffset = characterC.center.y + characterC.height / 2;
        var heroTopPoint = transform.position + new Vector3(0, heroYOffset, 0);

        var heroStandingYOffset = standingHeight - _sittingHeight;
        var heroStandingTopPoint = heroTopPoint + new Vector3(0, heroStandingYOffset, 0);

        Physics.OverlapCapsuleNonAlloc(heroTopPoint, heroStandingTopPoint, characterC.radius,
            standUpCheckOverlapResult);

        if (standUpCheckOverlapResult.Where(x => x != null).Any(other => other.gameObject != gameObject))
            return false;

        var characterCStandingCenter = Vector3.zero;

        characterC.center = characterCStandingCenter;
        characterC.height = standingHeight;

        return true;
    }

    private void Jump()
    {
        if (characterC.isGrounded)
            velocity.y = _jumpPower;
    }
}