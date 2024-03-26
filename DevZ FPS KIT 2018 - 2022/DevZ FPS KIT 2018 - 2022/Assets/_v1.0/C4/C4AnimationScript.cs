using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4AnimationScript : MonoBehaviour {

	public string drawAnim = "";         	// animation of taking out weapon.
	public string putDown = "";		// putting back weapon (playing draw animation backwards)
	public string idleAnim = "";
	public string dropC4 = "";
	public string activateC4 = "";
	public float drawAnimSpeed = 1.0f;
	public float drawReturnSpeed = 2.0f;
	public GameObject soundGO;
	public AudioClip dropSound;
	public AudioClip drawSound;
	public AudioClip activateSound;

	void Start()
	{
		if (idleAnim != "") GetComponent<Animation>()[idleAnim].wrapMode = WrapMode.Loop;
	}

	public void Update()
	{
		if (idleAnim != "")
		{
			if (!GetComponent<Animation>().isPlaying)
			{
				GetComponent<Animation>().CrossFadeQueued(idleAnim, 0.3f, QueueMode.PlayNow);
			}
		}
	}

	public void ActivateExplosives()
	{
		soundGO.GetComponent<AudioSource>().clip = activateSound;
		soundGO.GetComponent<AudioSource>().volume = 0.4f;
		soundGO.GetComponent<AudioSource>().Play();
		GetComponent<Animation>().CrossFade(activateC4);
	}

	public void DropExplosives()
	{
		soundGO.GetComponent<AudioSource>().clip = dropSound;
		soundGO.GetComponent<AudioSource>().volume = 0.7f;
		soundGO.GetComponent<AudioSource>().Play();
		GetComponent<Animation>().CrossFade(dropC4);
	}

	public void DrawWeapon()
	{

		if (drawAnim != "")
		{
			soundGO.GetComponent<AudioSource>().clip = drawSound;
			soundGO.GetComponent<AudioSource>().volume = 0.7f;
			soundGO.GetComponent<AudioSource>().Play();
			GetComponent<Animation>().Stop(drawAnim);
			GetComponent<Animation>()[drawAnim].speed = drawAnimSpeed;
			GetComponent<Animation>().Play(drawAnim);
		}
	}

	public void PutDownWeapon()
	{
		if (putDown != "")
		{
			soundGO.GetComponent<AudioSource>().clip = drawSound;
			soundGO.GetComponent<AudioSource>().volume = 0.7f;
			soundGO.GetComponent<AudioSource>().Play();
			GetComponent<Animation>()[putDown].speed = drawReturnSpeed;
			GetComponent<Animation>().Play(putDown);
		}
	}

	public void QuickPutDownWeapon()
	{
		if (putDown != "")
		{
			GetComponent<Animation>()[putDown].speed = drawReturnSpeed * 2;
			GetComponent<Animation>().CrossFade(putDown);
		}
	}
}
