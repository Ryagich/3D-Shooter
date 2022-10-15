using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
            Destroy(gameObject);
            Debug.Log("FixedUpdate");
        }
        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Debug.Log("OnTriggerEnter");
    }

    public void SetValues(float speed)
    {
        this.speed = speed;
    }
}
