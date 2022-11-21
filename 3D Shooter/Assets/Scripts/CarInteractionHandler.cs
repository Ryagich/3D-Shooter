using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteractionHandler : MonoBehaviour
{
    [SerializeField] private float _interactionDelay;

    [SerializeField] private GameObject carCamera;

    private float curTimer;
    public void GetIn(GameObject player, GameObject car)
    {
        var carController = car.GetComponent<CarControl>();
        if (curTimer != 0) return;
        player.GetComponent<HeroMovement>().enabled = false;
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<MeshRenderer>().enabled = false;
        player.transform.SetParent(car.transform);
        carController.enabled = true;
        carController._ui.SetActive(true);
        curTimer = _interactionDelay;
        player.transform.position = carController.seatPos.position;
        player.transform.rotation = carController.seatPos.rotation;
        carCamera.GetComponent<CarCameraController>().InitCar(car);
    }

    public void GetOut(GameObject player, GameObject car)
    {
        var carController = car.GetComponent<CarControl>();
        if (curTimer != 0) return;
        player.transform.position = carController.enterPos.position;
        player.transform.rotation = carController.enterPos.rotation;
        player.GetComponent<HeroMovement>().enabled = true;
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<MeshRenderer>().enabled = true;
        player.transform.SetParent(null);
        carController.enabled = false;
        carController._ui.SetActive(false);
        curTimer = _interactionDelay;
        carCamera.GetComponent<CarCameraController>().DeinitCar();
        carCamera.GetComponent<Camera>().depth = -1;
    }

    private void Update()
    {
        curTimer = Mathf.Clamp(curTimer - Time.deltaTime, 0, _interactionDelay);
    }
}
