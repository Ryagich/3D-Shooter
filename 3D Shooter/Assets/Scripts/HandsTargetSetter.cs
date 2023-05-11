using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsTargetSetter : MonoBehaviour
{
    [SerializeField] private Transform _rightT, _leftT;
    //tags
    //Left Hand IK
    //Right Hand IK

    private void Awake() => SetTargets(_leftT, _rightT);

    private void OnEnable() => SetTargets(_leftT, _rightT);

    private void OnDisable() => SetTargets();

    private void OnDestroy() => SetTargets();

    private void SetTargets(Transform left = null, Transform right = null)
    {
        var leftHand = GameObject.FindGameObjectWithTag("Left Hand IK");
        if (leftHand)
            leftHand.GetComponent<TransformCloner>().TransformToClone = left;
        var rightHand = GameObject.FindGameObjectWithTag("Right Hand IK");
        if (rightHand)
            rightHand.GetComponent<TransformCloner>().TransformToClone = right;
    }
}
