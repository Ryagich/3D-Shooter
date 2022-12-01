using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumders : MonoBehaviour
{
    [SerializeField] private float _gravity = 9.8f, _explosion = 1.0f, _fadeSpeed = 2.0f;
    [SerializeField] private Vector2 velocity;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        velocity = new Vector2(Random.Range(-_explosion, _explosion), _explosion);
        Destroy(gameObject, 1.0f);
    }

    private void FixedUpdate()
    {
        transform.Translate(velocity * Time.fixedDeltaTime, Space.Self);
        velocity += Vector2.down * _gravity * Time.fixedDeltaTime;

        _text.color = _text.color.WithA(_text.color.a - Time.fixedDeltaTime * _fadeSpeed);
    }
}
