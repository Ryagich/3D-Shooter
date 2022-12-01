using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private List<Hp> hpList = new List<Hp>();
    [SerializeField, Min(0.0f)] private float _damage = 10f;

    public void Attack()
    {
        foreach (var hp in hpList)
            hp.ChangeHp(-_damage);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var hp = other.gameObject.GetComponent<Hp>();
        if (other.gameObject != gameObject && hp && !hpList.Contains(hp))
            hpList.Add(hp);
    }

    private void OnTriggerExit(Collider other)
    {
        var hp = other.gameObject.GetComponent<Hp>();
        if (hp && hpList.Contains(hp))
            hpList.Remove(hp);
    }
}
