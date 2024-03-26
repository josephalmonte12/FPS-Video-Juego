using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpeedNew : MonoBehaviour {

	public float speed;
	private Vector3 lastPosition;
	public AudioClip crashSound;
	public BuggyDamageNew bDamage;
	public BuggyAdjustRotationNew camRot;
	public UseVehicleNew uw;

	public void FixedUpdate()
	{
		speed = (transform.position - lastPosition).magnitude;
		lastPosition = transform.position;
	}

	public void OnTriggerEnter(Collider other)
	{
		GetComponent<AudioSource>().clip = crashSound;
		if (speed > 0.1f)
		{
			if ((other.gameObject.name == "DamageReceiver") && (other.tag != "Player"))
			{
				other.gameObject.SendMessageUpwards("ApplyDamage", speed * 3000, SendMessageOptions.DontRequireReceiver);
			}
			if (uw.inCar)
			{
				camRot.Crash();
			}
			bDamage.ApplyDamage(speed * 1000);
			GetComponent<AudioSource>().volume = 1f;
			GetComponent<AudioSource>().Play();
		}
	}

}
