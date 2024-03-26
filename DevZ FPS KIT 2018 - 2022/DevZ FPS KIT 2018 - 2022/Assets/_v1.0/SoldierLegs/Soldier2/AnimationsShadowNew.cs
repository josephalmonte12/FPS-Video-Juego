using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsShadowNew : MonoBehaviour {

	public CharacterController controller;
	public Animation anim;
	public string idleAnim;
	//Walk
	public string walkForward;
	public string walkBackwards;
	public string strafeLeft;
	public string strafeRight;
	//Run
	public string runForward;
	//Crouch
	public string crouchIdle;
	public string crouchWalkForward;
	public string crouchWalkBackwards;
	public string crouchWalkLeft;
	public string crouchWalkRight;
	//Jump
	public string runJump;
	public string standingJump;
	public string idleAim;
	public string proneIdle;
	public Transform rootBone;
	public Transform upperBodyBone;
	private float lowerBodyDeltaAngle;
	private float lowerBodyDeltaAngleTarget;
	public bool grounded;
	public int controllerState;
	public PlayerController con;
	public Transform palyer;
	private Transform tr;
	private Vector3 lastPosition;
	private Vector3 velocity;
	private Vector3 localVelocity;
	private float angle;
	public Vector3 standPos;
	public Vector3 crouchPos;
	public Vector3 pronePos;
	private Vector3 tempPos;
	public float lastYRotation;
	public float turnSpeed;
	public string turnAnim;
	public Transform aimTarget;
	public Transform aimPivot;
	public float aimAngleY;
	public Transform spineRot;
	public Transform spineRot2;
	public Transform spineRot3;
	public float adjust;
	public float adjustX;
	public float maxAngle;
	public AnimationsNew anims;
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
		anim[runForward].layer = 5;
		anim[runForward].speed = 1.2f;
		anim[standingJump].layer = 5;
		anim[runJump].layer = 5;
		anim[idleAim].layer = 5;
		anim[idleAim].AddMixingTransform(upperBodyBone);
		anim[crouchIdle].layer = 0;
		anim[crouchWalkForward].layer = 0;
		anim[crouchWalkBackwards].layer = 0;
		anim[turnAnim].layer = 5;
		anim[turnAnim].speed = 1.5f;
		anim[proneIdle].speed = 0.1f;
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
		if (controllerState == 2) //Prone
		{
			anim.CrossFade(proneIdle, 0.4f);
		}
		if (grounded)
		{
			anim.CrossFade(idleAim);
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
			if (controllerState != 2) //Prone
			{
				lowerBodyDeltaAngleTarget = 0;
				float normalizedTime = Mathf.InverseLerp(50, -50, controller.velocity.y);
				anim[standingJump].normalizedTime = normalizedTime;
				anim.CrossFade(standingJump, 0.8f);
				anim[runJump].normalizedTime = normalizedTime;
				anim.CrossFade(runJump, 0.2f);
			}
		}
	}

	public float HorizontalAngle(Vector3 direction)
	{
		return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
	}

	public void LateUpdate()
	{
		turnSpeed = anims.turnSpeed;
		//Local Rotation 
		lowerBodyDeltaAngle = Mathf.LerpAngle(lowerBodyDeltaAngle, lowerBodyDeltaAngleTarget, Time.deltaTime * 3);
		// Aiming up/down
		if (controllerState != 2)
		{
			Vector3 aimDir = (aimTarget.position - aimPivot.position).normalized;
			float targetAngle = Mathf.Asin(aimDir.y) * Mathf.Rad2Deg;
			aimAngleY = Mathf.Lerp(aimAngleY, targetAngle, Time.deltaTime * 8);
			if (!Input.GetButton("Fire2"))
			{
				if (aimAngleY > maxAngle)
				{
					aimAngleY = maxAngle;
				}
				if (aimAngleY < -maxAngle)
				{
					aimAngleY = -maxAngle;
				}
			}

			spineRot.eulerAngles = new Vector3(spineRot.eulerAngles.x, spineRot.eulerAngles.y, (aimAngleY / 2) - adjust);
			spineRot2.eulerAngles = new Vector3(spineRot2.eulerAngles.x, spineRot2.eulerAngles.y, (aimAngleY / 2) - adjust);
			spineRot3.eulerAngles = new Vector3(spineRot3.eulerAngles.x, spineRot3.eulerAngles.y, aimAngleY - adjust);
			spineRot.localEulerAngles = new Vector3(adjustX, spineRot.localEulerAngles.y, spineRot.localEulerAngles.z);

			//offset for weapon
			spineRot3.localEulerAngles = new Vector3(spineRot3.localEulerAngles.x, -aimAngleY / 3, spineRot3.localEulerAngles.z);

			/*spineRot.eulerAngles.z = (aimAngleY / 2) - adjust;
			spineRot2.eulerAngles.z = (aimAngleY / 2) - adjust;
			spineRot3.eulerAngles.z = aimAngleY - adjust;
			spineRot.localEulerAngles.x = adjustX;
			//offset for weapon
			spineRot3.localEulerAngles.y = -aimAngleY / 3;*/
		}
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
