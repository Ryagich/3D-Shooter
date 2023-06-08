using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerManager : MonoBehaviour
{
    public static PointerManager Instance;

    [SerializeField] private float a = 1000;
    [SerializeField] private PointerIcon _pointerPrefab;
    [SerializeField] private Transform _point;
    [SerializeField] private Camera _camera;

    private Dictionary<EnemyPointer, PointerIcon> _dictionary = new Dictionary<EnemyPointer, PointerIcon>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddToList(EnemyPointer enemyPointer)
    {
        var newPointer = Instantiate(_pointerPrefab, transform);
        _dictionary.Add(enemyPointer, newPointer);
    }

    public void RemoveFromList(EnemyPointer enemyPointer)
    {
        Destroy(_dictionary[enemyPointer].gameObject);
        _dictionary.Remove(enemyPointer);
    }

    private void FixedUpdate()
    {
        return;
        var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        
        foreach (var i in _dictionary)
        {
            var enemyPointer = i.Key;
            var pointerIcon = i.Value;

            var toEnemy = enemyPointer.transform.position - _point.position;
            var ray = new Ray(_point.position, toEnemy);
            Debug.DrawRay(_point.position, toEnemy);

            var index = 0;
            var minDistance = Mathf.Infinity;
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

            if (toEnemy.magnitude < minDistance)
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
        else if(planeIndex == 3)
            return Quaternion.Euler(0f, 0f, 0f);
        return Quaternion.identity;
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawWireSphere(_camera.transform.position, a);
    }
}
