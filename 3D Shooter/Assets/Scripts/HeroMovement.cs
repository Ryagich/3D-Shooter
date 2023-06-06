using System.Linq;
using Bar;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(StaminaController))]
public class HeroMovement : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 50.0f)] private float _maxStepSpeed = 4.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float _stepAcceleration = 8.0f;
    [SerializeField] [Range(0.0f, 0.3f)] private float _expDragXZ = .015f;
    [SerializeField] [Range(0.0f, 0.3f)] private float _linearDragXZ = .05f;
    [SerializeField] [Range(0.0f, 20.0f)] private float _gravity = 10.0f;
    [SerializeField] [Range(0.0f, 10.0f)] private float _jumpPower = 4.5f;

    [Header("Sitting")]
    [SerializeField] [Range(0, 3)] private float _sittingHeight = 1;
    [SerializeField] [Range(0, 1)] private float _sittingSpeedMultiplier = 0.5f;

    [Header("Sprinting")]
    [SerializeField] [Range(0.0f, 50f)] private float _sprintStaminaDrain;
    [SerializeField] [Range(0.0f, 10f)] private float _sprintSpeedMultiplier;

    [SerializeField] private Vector3 velocity = Vector3.zero; // Debug SerializeField

    private CharacterController characterC;
    private StaminaController staminaController;

    private float standingHeight;
    private bool standUpRequested;
    private bool isSitting;
    private readonly Collider[] standUpCheckOverlapResult = new Collider[2];

    private void Awake()
    {
        characterC = GetComponent<CharacterController>();
        staminaController = GetComponent<StaminaController>();
        InputHandler.SpacePressed += Jump;
        InputHandler.CDowned += SitDown;
        InputHandler.CUpded += StandUp;
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

        var maxSpeed = _maxStepSpeed;

        if (TrySprint())
            maxSpeed *= _sprintSpeedMultiplier;

        if (isSitting)
            maxSpeed *= _sittingSpeedMultiplier;

        velocity += deltaV;
        var vy = velocity.y;
        velocity = Vector3.ClampMagnitude(velocity.WithY(0), maxSpeed).WithY(vy);

        characterC.Move(velocity * Time.fixedDeltaTime);

        TryStandUp();
    }

    private void OnDestroy() => InputHandler.SpacePressed -= Jump;

    private void OnDrawGizmos() => Gizmos.DrawSphere(transform.position, 0.1f);

    private void StandUp() => standUpRequested = true;

    private void SitDown()
    {
        if (standUpRequested)
            return;

        standUpRequested = false;
        isSitting = true;

        standingHeight = characterC.height;
        characterC.height = _sittingHeight;

        var offsetY = (standingHeight - _sittingHeight) / 2;

        characterC.center = new Vector3(0, offsetY, 0);
    }

    private bool TryStandUp()
    {
        if (!standUpRequested)
            return false;

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
        
        transform.position = new Vector3(0, heroYOffset, 0) + transform.position;

        standUpRequested = false;
        isSitting = false;

        return true;
    }

    private void Jump()
    {
        if (characterC.isGrounded)
            velocity.y = _jumpPower;
    }

    private bool TrySprint()
    {
        if (!characterC.isGrounded || isSitting)
            return false;

        var staminaDrainDelta = _sprintStaminaDrain * Time.fixedDeltaTime;

        if (InputHandler.IsShift && staminaController.Stamina > staminaDrainDelta)
        {
            staminaController.ChangeAmount(-staminaDrainDelta);
            return true;
        }

        return false;
    }
}