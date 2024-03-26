using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteScriptNew : MonoBehaviour {

	public AudioClip open;
	public AudioClip detach;

	public void Start () {
		GetComponent<AudioSource>().clip = open;
		GetComponent<AudioSource>().Play();
	}

	public void parachuteDestroy () {
		GetComponent<AudioSource>().clip = detach;
		GetComponent<AudioSource>().Play();
		Destroy(gameObject, 2.5f);
	}
}
