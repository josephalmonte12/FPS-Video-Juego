using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobLaddersProne : MonoBehaviour {

	private float timer = 0.0f; 
	private float timerY = 0.0f;
	private float bobbingSpeed; 
	private float bobbingSpeedY;
	private float bobbingAmount;
	private float bobbingAmountY; 
	public PlayerController codcontroller;
	public CharacterController controller;

	public AudioClip[] climbSounds;
	public AudioClip[] proneSounds;
	private float waveslice = 0.0f;
	private float wavesliceY = 0.0f;
	private float totalAxes;

	private Vector3 targetPos;

	public void Update()
	{
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		if (codcontroller.state == 2)
		{
			bobbingAmount = 0.05f;
			bobbingSpeed = 5.0f;
			bobbingAmountY = 0.12f;
		}
		else if (codcontroller.onLadder && !codcontroller.grounded)
		{
			bobbingSpeed = 4.5f;
			bobbingAmount = 0.07f;
			bobbingAmountY = 0.15f;
		}
		else if (!codcontroller.onLadder && codcontroller.grounded)
		{
			bobbingSpeed = 0;
			bobbingAmount = 0;
			bobbingAmountY = 0;
			transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 1f);
		}

		bobbingSpeedY = bobbingSpeed * 2;

		if (Mathf.Abs(vertical) == 0 && Mathf.Abs(horizontal) == 0)
		{
			timer = timerY = 0.0f;
		}
		else
		{
			waveslice = Mathf.Sin(timer);
			wavesliceY = Mathf.Sin(timerY);
			timer = timer + bobbingSpeed * Time.deltaTime;
			timerY = timerY + bobbingSpeedY * Time.deltaTime;
			if (timer > Mathf.PI * 2) timer = timer - (Mathf.PI * 2);

			if (timerY > Mathf.PI * 2)
			{
				timerY = timerY - (Mathf.PI * 2);

				if (codcontroller.ladderState == 3)
				{
					PlayClimbSounds(climbSounds[Random.Range(0, climbSounds.Length)], transform.position, .6f);
				}
				if (codcontroller.state == 2 && codcontroller.grounded && codcontroller.velMagnitude > 0.5)
				{
					PlayClimbSounds(proneSounds[Random.Range(0, proneSounds.Length)], transform.position, .6f);
				}
			}
		}

		if (waveslice != 0)
		{
			var translateChange = waveslice * bobbingAmount;
			var translateChangeY = wavesliceY * bobbingAmountY;

			totalAxes = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
			totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);

			translateChange = totalAxes * translateChange;
			translateChangeY = totalAxes * translateChangeY;
			if (controller.velocity.magnitude > 0.3 && !codcontroller.grounded && codcontroller.onLadder)
			{
				targetPos = new Vector3(translateChange, translateChangeY, translateChangeY / 4);

				transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 5);
			}
			else if (codcontroller.state == 2 && !codcontroller.onLadder)
			{
				if (Mathf.Abs(vertical) > 0.0)
				{
					targetPos = new Vector3(translateChange, translateChangeY / 2, translateChangeY);
				}
				if (Mathf.Abs(horizontal) > 0.0)
				{
					targetPos = new Vector3(translateChange, translateChangeY / 2, transform.localPosition.z);
				}

				transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 5);
			}
		}
	}

	public AudioSource PlayClimbSounds(AudioClip clip, Vector3 position, float volume)
	{
		var go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
