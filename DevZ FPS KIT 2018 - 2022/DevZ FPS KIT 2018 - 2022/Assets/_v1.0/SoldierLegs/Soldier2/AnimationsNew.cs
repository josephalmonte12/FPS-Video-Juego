using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

//TODO:
//Fix turn "Y" axis;

public class AnimationsNew : MonoBehaviour {

	public CharacterController controller;
	public Animation anim;
	public string idleAnim = "";
	public string crouchIdle = "";
	public string proneIdle = "";

	//Walk
	public string walkForward = "";
	public string walkBackwards = "";
	public string strafeLeft = "";
	public string strafeRight = "";
	//Run
	public string runForward = "";

	//Crouch
	public string crouchWalkForward = "";
	public string crouchWalkBackwards = "";
	public string crouchWalkLeft = "";
	public string crouchWalkRight = "";

	//Jump
	public string runJump = "";
	public string standingJump = "";
	public string idleAim = "";


	public Transform rootBone;
	public Transform upperBodyBone;
	private float lowerBodyDeltaAngle = 0.0f;
	private float lowerBodyDeltaAngleTarget;

	public bool grounded = true;
	private int controllerState = 1;
	public PlayerController con;


	public Transform palyer;
	private Transform tr;
	private Vector3 lastPosition = Vector3.zero;
	private Vector3 velocity = Vector3.zero;
	private Vector3 localVelocity = Vector3.zero;

	private float angle = 0f;

		public Vector3 standPos;
		public Vector3 crouchPos;
		public Vector3 pronePos;
		private Vector3 tempPos;
	
		private float lastYRotation;
		public float turnSpeed;
		public string turnAnim;

	public void Awake()
	{
		tr = palyer.transform;
		lastPosition = tr.position;
	}

	public void Start()
	{
		anim[walkForward].speed = 1.3f;
		anim[walkBackwards].speed = 1.3f;
		anim[strafeLeft].speed = 1.3f;
		anim[strafeRight].speed = 1.3f;
		anim[runForward].speed = 1.2f;
		anim[turnAnim].speed = 1.5f;
	}

	public void Update()
	{
		velocity = (tr.position - lastPosition) / Time.deltaTime;
		localVelocity = tr.InverseTransformDirection(velocity);
		localVelocity.y = 0;
		angle = HorizontalAngle(localVelocity);
		lastPosition = tr.position;
		controllerState = con.state;
		grounded = con.grounded;
		float movementSpeed = controller.velocity.magnitude;
		//turnSpeed = Mathf.DeltaAngle(lastYRotation, transform.rotation.eulerAngles.y);
		if (controllerState == 2) //Prone
		{
			anim.CrossFade(proneIdle, 0.3f);
		}
		if (grounded)
		{
			if (movementSpeed < 1f)
			{
				if (controllerState != 2)
				{
					if (turnSpeed != 0f)
					{
						anim.Blend(turnAnim, 0.5f);
						return;
					}
				}
			}
			if (controllerState == 0) //Walk and Run
			{
				if (localVelocity.z > 1f) //Forward
				{
					if (movementSpeed < 7f)
					{
						anim.CrossFade(walkForward, 0.2f);
					}
					else
					{
						anim.CrossFade(runForward, 0.2f);
					}
					if (angle < -25)
					{
						lowerBodyDeltaAngleTarget = -45;
					}
					else
					{
						if (angle > 25)
						{
							lowerBodyDeltaAngleTarget = 45;
						}
						else
						{
							lowerBodyDeltaAngleTarget = 0;
						}
					}
				}
				else
				{
					if (localVelocity.z < -1f) //Backward
					{
						if (movementSpeed < 7f)
						{
							anim.CrossFade(walkBackwards, 0.2f);
						}
						if ((angle > 115) && (angle < 155))
						{
							lowerBodyDeltaAngleTarget = -45;
						}
						else
						{
							if ((angle < -115) && (angle > -155))
							{
								lowerBodyDeltaAngleTarget = 45;
							}
							else
							{
								lowerBodyDeltaAngleTarget = 0;
							}
						}
					}
					else
					{
						if (localVelocity.x < -1f)
						{
							if (movementSpeed < 7f)
							{
								anim.CrossFade(strafeLeft, 0.5f);
							}
							lowerBodyDeltaAngleTarget = 0;
						}
						else
						{
							if (localVelocity.x > 1f)
							{
								if (movementSpeed < 7f)
								{
									anim.CrossFade(strafeRight, 0.5f);
								}
								lowerBodyDeltaAngleTarget = 0;
							}
							else
							{
								lowerBodyDeltaAngleTarget = 0;
								anim.CrossFade(idleAnim, 0.3f);
							}
						}
					}
				}
			}
			else
			{
				if (controllerState == 1) //Crouch
				{
					if (localVelocity.z > 0.2f)
					{
						anim.CrossFade(crouchWalkForward, 0.5f);
						if (angle < -25)
						{
							lowerBodyDeltaAngleTarget = -45;
						}
						else
						{
							if (angle > 25)
							{
								lowerBodyDeltaAngleTarget = 45;
							}
							else
							{
								lowerBodyDeltaAngleTarget = 0;
							}
						}
					}
					else
					{
						if (localVelocity.z < -0.2f)
						{
							anim.CrossFade(crouchWalkBackwards, 0.4f);
							if ((angle > 115) && (angle < 155))
							{
								lowerBodyDeltaAngleTarget = -45;
							}
							else
							{
								if ((angle < -115) && (angle > -155))
								{
									lowerBodyDeltaAngleTarget = 45;
								}
								else
								{
									lowerBodyDeltaAngleTarget = 0;
								}
							}
						}
						else
						{
							if (localVelocity.x < -0.2f)
							{
								anim.CrossFade(crouchWalkLeft, 0.5f);
								lowerBodyDeltaAngleTarget = 0;
							}
							else
							{
								if (localVelocity.x > 0.2f)
								{
									anim.CrossFade(crouchWalkRight, 0.5f);
									lowerBodyDeltaAngleTarget = 0;
								}
								else
								{
									lowerBodyDeltaAngleTarget = 0;
									anim.CrossFade(crouchIdle, 0.6f);
								}
							}
						}
					}
				}
			}
		}
		else
		{
			lowerBodyDeltaAngleTarget = 0;
			float normalizedTime = Mathf.InverseLerp(50, -50, controller.velocity.y);
			anim[standingJump].normalizedTime = normalizedTime;
			anim.CrossFade(standingJump, 0.8f);
			anim[runJump].normalizedTime = normalizedTime;
			anim.CrossFade(runJump, 0.2f);
		}
	}

	public float HorizontalAngle(Vector3 direction)
	{
		return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
	}

	public void LateUpdate()
	{
		turnSpeed = Mathf.DeltaAngle(lastYRotation, transform.rotation.eulerAngles.y);
		lastYRotation = transform.rotation.eulerAngles.y;
		//Local Rotation 
		lowerBodyDeltaAngle = Mathf.LerpAngle(lowerBodyDeltaAngle, lowerBodyDeltaAngleTarget, Time.deltaTime * 3);
		Quaternion lowerBodyDeltaRotation = Quaternion.Euler(0, lowerBodyDeltaAngle, 0);
		rootBone.rotation = lowerBodyDeltaRotation * rootBone.rotation;
		upperBodyBone.rotation = Quaternion.Inverse(lowerBodyDeltaRotation) * upperBodyBone.rotation;
		if (controllerState == 0)
		{
			tempPos = standPos;
		}
		else
		{
			if (controllerState == 1)
			{
				tempPos = crouchPos;
			}
			else
			{
				if (controllerState == 2)
				{
					tempPos = pronePos;
				}
			}
		}
		if (transform.localPosition != tempPos)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, tempPos, 4 * Time.deltaTime);
		}
	}
}
