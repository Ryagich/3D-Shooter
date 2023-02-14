using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCloner : MonoBehaviour
{
    public Transform TransformToClone;

    private void Update()
    {
        if (TransformToClone)
        transform.SetPositionAndRotation(TransformToClone.position, TransformToClone.rotation);
        //{
        //    transform.rotation = TransformToClone.rotation;
        //    transform.position = Vector3.Lerp(transform.position, TransformToClone.position, 0.99f);
        //}
    }
}
