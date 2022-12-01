using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformCloner : MonoBehaviour
{
    public Transform TransformToClone;

    private void FixedUpdate()
    {
        if (TransformToClone)
            transform.SetPositionAndRotation(TransformToClone.position, TransformToClone.rotation);
    }
}
