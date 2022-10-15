using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InteractableFounder : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _cameraTrans;
    [SerializeField, Range(0.0f, 100.0f)] private float _distance = 1.0f;

    private void FixedUpdate()
    {
        var ray = new Ray(_cameraTrans.position, _cameraTrans.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _distance))
        {
            var interactable = hit.transform.gameObject.GetComponent<Interactable>();

            _text.gameObject.SetActive(interactable != null);
            if (interactable != null && Input.GetKeyDown(KeyCode.F))
                interactable.Press(gameObject);
        }
        else
            _text.gameObject.SetActive(false);
    } 
}
