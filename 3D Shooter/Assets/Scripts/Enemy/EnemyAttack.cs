using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private List<HpController> _hpList = new List<HpController>();
    [SerializeField, Min(0.0f)] private float _damage = 10f;

    public void Attack()
    {
        foreach (var hp in _hpList)
            hp.ChangeAmount(-_damage);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var hp = other.gameObject.GetComponent<HpController>();
        if (other.gameObject != gameObject && hp && !_hpList.Contains(hp))
            _hpList.Add(hp);
    }

    private void OnTriggerExit(Collider other)
    {
        var hp = other.gameObject.GetComponent<HpController>();
        if (hp && _hpList.Contains(hp))
            _hpList.Remove(hp);
    }
}
