using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    [SerializeField, Range(0.0f, 100.0f)] private float _distance = 10.0f;
    [SerializeField] Transform _player, _target, _cameraTrans;

    private float mouseX, mouseY, yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -45, 45);

        _player.Rotate(mouseX * Vector3.up);
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
