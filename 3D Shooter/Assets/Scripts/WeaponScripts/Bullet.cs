using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private float speed;
    private Vector3 lastPos;

    private void Start()
    {
        lastPos = transform.position;
        Destroy(gameObject, 10.0f);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        RaycastHit hit;

        if (Physics.Linecast(lastPos, transform.position, out hit))
        {
            var hp = hit.transform.GetComponent<Hp>();
            if (hp)
                hp.TakeDamage(damage);
            Destroy(gameObject);
        }
        lastPos = transform.position;
    }

    public void SetValues(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
    }
}
