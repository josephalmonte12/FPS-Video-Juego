using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	public AudioClip wind;
	private AudioSource windAudio;
	public float windVolume;
	public Camera cam;
	private AudioSource carAudio;
	private int gearShiftsStarted;
	private int oneShotLimit;
	public float carSpeed;
	void Awake()
	{
		windAudio = gameObject.AddComponent<AudioSource>();
		windAudio.loop = true;
		windAudio.clip = wind;
		windAudio.volume = windVolume;
		windAudio.Play();
	}

	public void Update()
	{
		carSpeed = GetComponent<Rigidbody>().velocity.magnitude;
		float carSpeedFactor = Mathf.Clamp01(carSpeed / 60f);
		windAudio.volume = Mathf.Lerp(0, windVolume, carSpeedFactor);
		cam.fieldOfView = 60 + (carSpeed / 2.5f);
	}
}
