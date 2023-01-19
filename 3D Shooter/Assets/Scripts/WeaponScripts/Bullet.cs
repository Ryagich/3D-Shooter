using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public event Action OnEnemyHit;

    [SerializeField] private List<Material> _materials = new List<Material>();
    [SerializeField] private List<GameObject> _effectPrefabs = new List<GameObject>();
    [SerializeField] private GameObject _defHitEffect;
    [SerializeField] private LayerMask _targetMask;

    private float damage;
    private float speed;
    private Vector3 lastPos;

    public void ResetBullet()
    {
        StopAllCoroutines();
        lastPos = transform.position;
        StartCoroutine(Return());
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(5.0f);
        BulletPool.Pool.Return(gameObject);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Physics.Linecast(lastPos, transform.position, out var hit, _targetMask))
        {
            var hp = hit.transform.GetComponent<HpController>();
            if (hp)
            {
                hp.ChangeHp(-damage);
                Crosshair.Instance.ShowSignalCrosshair();
                OnEnemyHit?.Invoke();
            }

            var renderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
            if (renderer)
            {
                var material = renderer.sharedMaterial;
                var index = _materials.FindIndex(m => m == material);
                if (index >= 0)
                    SpawnDecal(hit, _effectPrefabs[index]);
                else
                    SpawnDecal(hit, _defHitEffect);
            }
            Debug.DrawLine(lastPos, transform.position, Color.red, 10f);
            BulletPool.Pool.Return(gameObject);
            return;
        }
        Debug.DrawLine(lastPos, transform.position, Color.green,10f);
        lastPos = transform.position;
    }

    private void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        var decal = Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
        decal.transform.SetParent(hit.collider.transform);
        Destroy(decal, 10.0f);
    }

    public void SetValues(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
    }
}
