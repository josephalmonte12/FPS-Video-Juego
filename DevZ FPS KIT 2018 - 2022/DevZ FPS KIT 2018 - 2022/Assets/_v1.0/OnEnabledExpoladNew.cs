using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnabledExpoladNew : MonoBehaviour {

	public GameObject explosion;
	public GameObject explosion2;
	public float destryAfter;
	public AudioClip explSound;
	public Rigidbody[] rigidb;

	public IEnumerator Enable()
	{
		PlayAudioClip(explSound, transform.position, 1f);
		Instantiate(explosion, transform.position + new Vector3(0, 5, 0), transform.rotation);
		yield return new WaitForSeconds(0.1f);
		int i = 0;
		while (i < rigidb.Length)
		{
			rigidb[i].drag = 0.1f;
			i++;
		}
		Instantiate(explosion2, transform.position, transform.rotation);
		Destroy(gameObject, destryAfter);
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.maxDistance = 1000f;
		source.volume = volume;
		source.pitch = Time.timeScale;
		source.Play();
		Destroy(go, clip.length);
		return source;
	}

}
