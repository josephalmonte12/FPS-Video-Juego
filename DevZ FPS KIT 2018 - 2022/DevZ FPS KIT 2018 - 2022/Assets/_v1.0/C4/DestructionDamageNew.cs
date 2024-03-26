using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionDamageNew : MonoBehaviour {

	public float hitPoints;
	public GameObject replacement;
	public GameObject hide;
	public AudioClip destructionSound;
	public void ApplyDamage(float damage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		hitPoints = hitPoints - damage;
		if (hitPoints <= 0f)
		{
			Detonate();
		}
	}

	public void Detonate()
	{
		GetComponent<AudioSource>().PlayOneShot(destructionSound, 1f);
		replacement.SetActive(true);
		hide.SetActive(false);
		enabled = false;
	}
}
