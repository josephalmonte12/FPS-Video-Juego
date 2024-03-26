using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuggyDamageNew : MonoBehaviour {

	public CarController cc;
	public int hitPoints;
	public UseVehicleNew vScript;
	public GameObject deadReplacement;
	public GameObject[] unparentWheels;
	public GameObject explosion;
	public GameObject explosion2;
	public Transform pos;
	public int minDamage;
	public ParticleSystem dmageEffect;
	public void Start()
	{
		cc.hitPoints = hitPoints;
	}

	public void ApplyDamage(float damage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		// Apply damage
		hitPoints = hitPoints - (int)damage;
		cc.hitPoints = hitPoints;
		StartCoroutine(DamageEffect());
		// Are we dead?
		if (hitPoints <= 0f)
		{
			StartCoroutine(Detonate());
		}
	}

	public IEnumerator DamageEffect()
	{
		dmageEffect.Play();// = true;
		yield return new WaitForSeconds(4f);
		if (hitPoints > minDamage)
		{
			dmageEffect.Stop();// = false;
		}
	}

	public IEnumerator Detonate()
	{
		if (vScript.inCar)
		{
			StartCoroutine(vScript.Action(3)); //unparrent player before explosion
		}
		yield return new WaitForSeconds(0.1f);
		deadReplacement.SetActive(true);
		deadReplacement.transform.parent = null;
		//deadReplacement.BroadcastMessage("Enable");
		for (int i = 0; i < unparentWheels.Length; i++){
			unparentWheels[i].transform.parent = null;
			unparentWheels[i].AddComponent<MeshCollider>();
			unparentWheels[i].transform.GetComponent<MeshCollider>().convex = true;
			unparentWheels[i].AddComponent<Rigidbody>();
			unparentWheels[i].transform.GetComponent<Rigidbody>().mass = 40;
			var unparentWheelsPos = unparentWheels[i].transform.position;
			unparentWheelsPos.y += 1;
			unparentWheels[i].transform.position = unparentWheelsPos;
			unparentWheels[i].GetComponent<Rigidbody>().AddForce(Vector3.up * 40000);

		}
		Instantiate(explosion, pos.transform.position, Quaternion.identity);
		Instantiate(explosion2, pos.transform.position, pos.transform.rotation);
		Destroy(transform.parent.gameObject);
	}
}
