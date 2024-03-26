using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHelicopterScript : MonoBehaviour {

	private GameObject controller;
	public bool inCar;
	public GameObject vehicleScript;
	public GameObject vehicle;
	public GameObject fpCamera;
	//var body : GameObject;
	public bool waitTime;
	public Transform getOutPosition;
	public AudioClip startEngine;
	public AudioClip handbrake;
	public string TargetName = "PlayerController";
	public void Start()
	{
		controller = GameObject.Find(TargetName);
	}

	public void Update()
	{
		if (!inCar)
		{
			return;
		}
		if (Input.GetKey("e"))
		{
			StartCoroutine(Action(2));
		}
	}

	public virtual IEnumerator Action(int useVehicle)
	{
		if (waitTime)
		{
			yield break;
		}
		waitTime = true;
		HelicopterControllerNew vScript = vehicleScript.GetComponent<HelicopterControllerNew>();
		//Get in car
		if (useVehicle == 1)
		{
			controller.SetActive(false);
			vehicle.SetActive(true);
			fpCamera.SetActive(true);
			//body.SetActive(true);
			inCar = true;
			controller.transform.eulerAngles = new Vector3(0f, vehicleScript.transform.rotation.eulerAngles.y, 0f);
			controller.transform.position = transform.position + (Vector3.right * 2f);
			controller.transform.parent = vehicle.transform;
			yield return new WaitForSeconds(0.4f);
			vScript.controlsEnabled = true;
			vScript.playerInside = true;
			waitTime = false;
		}
		//Get out
		if (useVehicle == 2)
		{
			fpCamera.SetActive(false);
			//body.SetActive(false);
			inCar = false;
			vScript.controlsEnabled = false;
			vScript.playerInside = false;
			controller.transform.parent = null;
			controller.transform.eulerAngles = new Vector3(0f, vehicleScript.transform.rotation.eulerAngles.y, 0f);
			controller.transform.position = getOutPosition.position;
			controller.SetActive(true);
			yield return new WaitForSeconds(0.3f);
			waitTime = false;
		}
	}

}
