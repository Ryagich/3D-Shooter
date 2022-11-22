using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField, Range(0.0f, 100.0f)] private float _distance = 10.0f;
    [SerializeField] Transform _player, _target, _cameraTrans;

    private float mouseX, mouseY, yRotation;

    private void Awake()
    {
        InputHandler.OnMouseX += RotateHero;   
        InputHandler.OnMouseY += RotateCamera;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void RotateHero(float x)
    {
        mouseX = x * _sensitivity * Time.deltaTime;
        _player.Rotate(mouseX * Vector3.up);

        UpdateTargetLook();
    }

    private void RotateCamera(float y)
    {
        mouseY = y * _sensitivity * Time.deltaTime;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -45, 45);
        _cameraTrans.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        UpdateTargetLook();
    }

    private void UpdateTargetLook()
    {
        var ray = new Ray(_cameraTrans.position, _cameraTrans.forward * _distance);
        RaycastHit hit;

        _target.position = Physics.Raycast(ray, out hit)
            ? Vector3.Lerp(_target.position, hit.point, Time.deltaTime * 5)
            : Vector3.Lerp(_target.position, _target.transform.forward * 200, Time.deltaTime * 5);
    }
}
