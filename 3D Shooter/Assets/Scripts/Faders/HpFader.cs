using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpFader : MonoBehaviour
{
    [SerializeField] private Fader _hpFader;

    private void Awake()
    {
        GetComponent<HpController>().HpM.OnAmountChanged += (_, _) => _hpFader.Show(true);
    }
}
