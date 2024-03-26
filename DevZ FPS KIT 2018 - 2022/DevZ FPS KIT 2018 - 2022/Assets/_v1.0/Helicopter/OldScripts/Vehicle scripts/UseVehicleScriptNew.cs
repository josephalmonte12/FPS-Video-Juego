using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class UseVehicleScriptNew : MonoBehaviour {

	public float maxRayDistance = 2.0f;
	public LayerMask layerMask;
	public bool showGui = false;
	private HUDManager HUD;

	void Start()
	{
		if (HUD == null && FindObjectOfType<HUDManager>())
			HUD = FindObjectOfType<HUDManager>();
	}

	void Update()
	{
		var direction = gameObject.transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		var position = transform.position;
		if (Physics.Raycast(position, direction, out hit, maxRayDistance, layerMask.value))
		{
			HUD.ShowInfo = true;
			if (Input.GetKeyDown("e"))
			{
				var target = hit.collider.gameObject;
				target.BroadcastMessage("Action", 1);
			}
		}
		else
		{
			HUD.ShowInfo = false;
		}
	}

	/*void OnGUI()
	{
		GUI.skin = mySkin;
		if (showGui)
		{
			GUI.Label(new Rect(Screen.width - (Screen.width / 1.7f), Screen.height - (Screen.height / 1.4f), 800, 100), "Press key >>E<< to Use");
		}
	}*/
}
