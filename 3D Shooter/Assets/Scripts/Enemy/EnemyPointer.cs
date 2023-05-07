using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyPointer : MonoBehaviour
{
    private void Start()
    {
        PointerManager.Instance.AddToList(this);
        GetComponent<HpController>().OnDeath.AddListener(Dead);
    }

    private void Dead()
    {
        PointerManager.Instance.RemoveFromList(this);
    }
}
