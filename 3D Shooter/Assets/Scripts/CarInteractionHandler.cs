using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteractionHandler : MonoBehaviour
{
    [SerializeField] private float _interactionDelay;

    private float curTimer = 0;
    public void GetIn(GameObject player, GameObject car)
    {
        if (curTimer == 0)
        {
            player.SetActive(false);
            car.GetComponent<CarControl>().enabled = true;
            curTimer = _interactionDelay;
        }
    }

    public void GetOut(GameObject player, GameObject car)
    {
        if (curTimer == 0)
        {
            player.SetActive(true);
            car.GetComponent<CarControl>().enabled = false;
            player.transform.position = car.GetComponent<CarControl>()._enterPos.position;
            player.transform.rotation = car.GetComponent<CarControl>()._enterPos.rotation;
            curTimer = _interactionDelay;
        }
    }

    private void Update()
    {
        curTimer = Mathf.Clamp(curTimer - Time.deltaTime, 0, _interactionDelay);
    }
}
