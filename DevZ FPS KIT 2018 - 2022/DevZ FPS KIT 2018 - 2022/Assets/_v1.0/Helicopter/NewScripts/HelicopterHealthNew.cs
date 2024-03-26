using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterHealthNew : MonoBehaviour {

	public float maximumHitPoints;
	public float hitPoints;
	public GameObject deadReplacement;
	private float gotHitTimer;
	public Transform explosion;
	public GameObject particle;
	public HelicopterControllerNew controllScript;
	public GUISkin mySkin;
	private bool callFunction;
	public UseHelicopterScript vScript;
	public void Start()
	{
		HelicopterControllerNew controllScript = GetComponent<HelicopterControllerNew>();
	}

	public void Update()
	{
		if (hitPoints <= 200)
		{
			particle.GetComponent<ParticleSystem>().Play();
			particle.GetComponent<AudioSource>().enabled = true;
			controllScript.damaged = true;
			hitPoints = hitPoints - (Time.deltaTime * 20);
			if (hitPoints <= 0f)
			{
				Detonate();
			}
		}
	}

	public void ApplyDamage(float damage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		// Apply damage
		hitPoints = hitPoints - damage;
		// Are we dead?
		if (hitPoints <= 0f)
		{
			Detonate();
		}
	}

	public void Detonate()
	{
		if (callFunction)
		{
			return;
		}
		callFunction = true;
		if (vScript.inCar)
		{
			StartCoroutine(vScript.Action(2)); //unparrent player before explosion
		}
		// If we have a dead barrel then replace ourselves with it!
		if (deadReplacement)
		{
			deadReplacement.SetActive(true);
			deadReplacement.transform.parent = null;
			deadReplacement.BroadcastMessage("Enable");
		}
		Destroy(transform.parent.gameObject);
	}

	/*public void OnGUI()
	{
		GUI.skin = mySkin;
		GUIStyle style1 = mySkin.customStyles[0];
		if (controllScript.controlsEnabled)
		{
			GUI.Label(new Rect(120, Screen.height - 50, 150, 80), "" + hitPoints, style1);
			GUI.Label(new Rect(20, Screen.height - 50, 150, 80), " HP: ");
		}
	}*/

}
