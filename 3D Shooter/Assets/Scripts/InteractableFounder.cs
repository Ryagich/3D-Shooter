using UnityEngine;
using TMPro;

public class InteractableFounder : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _cameraTrans;
    [SerializeField, Range(0.0f, 100.0f)] private float _distance = 1.0f;
    [SerializeField] private LayerMask _targetLayers;

    private Transform lastGameObject;
    private Interactable lastIntractable;

    private void FixedUpdate()
    {
        var ray = new Ray(_cameraTrans.position, _cameraTrans.forward);

        if (Physics.Raycast(ray, out var hit, _distance, _targetLayers))
        {
            var gameObj = hit.transform;
            if (lastGameObject != gameObj)
            {
                lastGameObject = gameObj;
                lastIntractable = gameObj.GetComponent<Interactable>();
            }
            if (!_text.gameObject.activeSelf)
                _text.gameObject.SetActive(true);

            if (lastIntractable && Input.GetKeyDown(KeyCode.F))
                lastIntractable.Press(gameObject);
        }
        else
            _text.gameObject.SetActive(false);
    }
}
