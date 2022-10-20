using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyPointer : MonoBehaviour
{
    private void Start()
    {
        PointerManager.Instance.AddToList(this);
    }
}
