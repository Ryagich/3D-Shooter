using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{
    [SerializeField] private PointerIcon _pointerPrefab;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Camera _camera;

    private Dictionary<EnemyPointer, PointerIcon> _dictionary = new Dictionary<EnemyPointer, PointerIcon>();

    public static PointerManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void AddToList(EnemyPointer enemyPointer)
    {
        PointerIcon newPointer = Instantiate(_pointerPrefab, transform);
        _dictionary.Add(enemyPointer, newPointer);
    }

    public void RemoveFromList(EnemyPointer enemyPointer)
    {
        Destroy(_dictionary[enemyPointer].gameObject);
        _dictionary.Remove(enemyPointer);
    }

    private void LateUpdate()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        foreach (var i in _dictionary)
        {
            var enemyPointer = i.Key;
            var pointerIcon = i.Value;

            var toEnemy = enemyPointer.transform.position - _playerTransform.position;
            var ray = new Ray(_playerTransform.position, toEnemy);
            Debug.DrawRay(_playerTransform.position, toEnemy);

            var minDistance = Mathf.Infinity;
            var index = 0;

            for (var p = 0; p < 4; p++)
                if (planes[p].Raycast(ray, out var distance))
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        index = p;
                    }

            minDistance = Mathf.Clamp(minDistance, 0, toEnemy.magnitude);
            var worldPosition = ray.GetPoint(minDistance);
            var position = _camera.WorldToScreenPoint(worldPosition);
            var rotation = GetIconRotation(index);

            if (toEnemy.magnitude > minDistance)
                pointerIcon.Show();
            else
                pointerIcon.Hide();

            pointerIcon.SetIconPosition(position, rotation);
        }
    }

    private Quaternion GetIconRotation(int planeIndex)
    {
        if (planeIndex == 0)
            return Quaternion.Euler(0f, 0f, 90f);
        else if (planeIndex == 1)
            return Quaternion.Euler(0f, 0f, -90f);
        else if (planeIndex == 2)
            return Quaternion.Euler(0f, 0f, 180);
        else
            return Quaternion.Euler(0f, 0f, 0f);
    }

}
