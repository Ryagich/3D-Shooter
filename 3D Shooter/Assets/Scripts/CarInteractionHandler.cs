using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteractionHandler : MonoBehaviour
{
    [SerializeField] private float _interactionDelay;

    private float curTimer;
    public void GetIn(GameObject player, GameObject car)
    {
        var carController = car.GetComponent<CarControl>();
        if (curTimer == 0)
        {
            player.GetComponent<HeroMovement>().enabled = false;
            player.GetComponent<CapsuleCollider>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.SetParent(car.transform);
            car.GetComponent<CarControl>().enabled = true;
            curTimer = _interactionDelay;
            player.transform.position = carController.seatPos.position;
            player.transform.rotation = carController.seatPos.rotation;
        }
    }

    public void GetOut(GameObject player, GameObject car)
    {
        var carController = car.GetComponent<CarControl>();
        if (curTimer == 0)
        {
            player.transform.position = carController.enterPos.position;
            player.transform.rotation = carController.enterPos.rotation;
            player.GetComponent<HeroMovement>().enabled = true;
            player.GetComponent<CapsuleCollider>().enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
            player.transform.SetParent(null);
            car.GetComponent<CarControl>().enabled = false;
            curTimer = _interactionDelay;
        }
    }

    private void Update()
    {
        curTimer = Mathf.Clamp(curTimer - Time.deltaTime, 0, _interactionDelay);
    }
}
