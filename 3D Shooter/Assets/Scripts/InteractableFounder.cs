using UnityEngine;
using TMPro;

public class InteractableFounder : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _cameraTrans;
    [SerializeField, Range(0.0f, 100.0f)] private float _distance = 1.5f;
    [SerializeField] private LayerMask _targetLayers;

    private Interactable lastIntractable;

    private void Awake() { InputHandler.OnFDown += () => lastIntractable?.Press(gameObject); }

    private void FixedUpdate()
    {
        var ray = new Ray(_cameraTrans.position, _cameraTrans.forward);

        if (Physics.Raycast(ray, out var hit, _distance, _targetLayers))
        {
            var interactable = hit.transform.GetComponent<Interactable>();
            if (interactable != lastIntractable)
                lastIntractable = interactable;
        }
        else
            lastIntractable = null;
        _text.gameObject.SetActive(lastIntractable);
    }
}
