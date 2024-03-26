using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassScriptNew : MonoBehaviour {
//#pragma strict
	public GameObject normalGlass;
	public GameObject statteredGlass;
	public bool shatter;
	public float removeGlassAfter;
	public float hitPoints;
	public AudioClip destructionSound;
	public Vector3 vel;
	public Vector3 previous;
	public Vector3 explPos;

	public void ApplyDamage(int damage)
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
			StartCoroutine(ShatterGlass());
		}
	}

	public virtual IEnumerator ShatterGlass()
	{
		if (!shatter)
		{
			shatter = true;
			//normalGlass.GetComponent<Rigidbody>().useGravity = true;
			yield return new WaitForSeconds(0.1f);
			statteredGlass.SetActive(true);
			Destroy(normalGlass);
			if (explPos != Vector3.zero)
			{
				var gos = gameObject.GetComponentsInChildren(typeof(Rigidbody));
				foreach (Rigidbody go in gos)
				{
					go.isKinematic = false;
					go.useGravity = true;
					go.GetComponent<Rigidbody>().velocity = (transform.position - explPos) * 0.5f;
				}
			}
			//yield WaitForSeconds(0.1);
			GetComponent<AudioSource>().clip = destructionSound;
			GetComponent<AudioSource>().Play();
			explPos = Vector3.zero;
		}
		yield return new WaitForSeconds(removeGlassAfter);
		Destroy(statteredGlass);
		enabled = false;
	}

	public void ShatterGlassHit()
	{
		Destroy(normalGlass);
		shatter = true;
		statteredGlass.SetActive(true);
		GetComponent<AudioSource>().clip = destructionSound;
		GetComponent<AudioSource>().Play();
	}
}
