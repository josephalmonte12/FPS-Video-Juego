using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterPlaySoundNew : MonoBehaviour {

	public AudioClip[] hitSounds;

	public void OnTriggerEnter(Collider other)
	{
		GetComponent<AudioSource>().clip = hitSounds[Random.Range(0, hitSounds.Length)];
		GetComponent<AudioSource> ().Play();
	}
}
