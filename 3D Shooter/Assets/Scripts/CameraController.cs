using Settings;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(0.0f, 500.0f)] private float _targetDistance = 200.0f;
    [SerializeField] private Transform _player, _target, _cameraTrans;
    [SerializeField] private LayerMask _layers;
    [SerializeField, Range(0.0f, 100.0f)] private float _targetSpeed = 5.0f;

    private float mouseX, mouseY, yRotation;

    private void Awake()
    {
        InputHandler.OnMouseX += RotateX;
        InputHandler.OnMouseY += RotateY;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RotateX(float x)
    {
        mouseX = x * UserSettings.CameraSensitivity * Time.deltaTime;
        _player.Rotate(mouseX * Vector3.up);

        UpdateTargetLook();
    }

    public void RotateY(float y)
    {
        mouseY = y * UserSettings.CameraSensitivity * Time.deltaTime;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -45, 70);
        _cameraTrans.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        UpdateTargetLook();
    }

    private void UpdateTargetLook()
    {
        var ray = new Ray(_cameraTrans.position, _cameraTrans.forward);

        var lerpTarget = Physics.Raycast(ray, out var hit, _targetDistance, _layers)
            ? hit.point
            : _cameraTrans.position + _cameraTrans.forward * _targetDistance;
        _target.position = Vector3.Lerp(_target.position, lerpTarget, Time.fixedDeltaTime * _targetSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Camera.main.transform.position, _target.position);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(_cameraTrans.position, _cameraTrans.forward * _targetDistance * 200);
    }

    public Transform GetTargetLook() => _target;

    private void OnDestroy()
    {
        InputHandler.OnMouseX -= RotateX;
        InputHandler.OnMouseY -= RotateY;
    }
}