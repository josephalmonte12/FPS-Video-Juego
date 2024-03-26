using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCameraNew : MonoBehaviour {

	public Transform target;
	private Transform myTransform;
	public float targetHeight;
	public float targetRight;
	public float distance;
	public bool prevButtonRight;
	public float maxDistance;
	public float minDistance;
	public float xSpeed;
	public float ySpeed;
	public float yMinLimit;
	public float yMaxLimit;
	public float zoomRate;
	public float rotationDampening;
	public float theta2;
	private float x;
	private float y;
	private Vector3 fwd;
	private Vector3 rightVector;
	private Vector3 upVector;
	private Vector3 movingVector;
	private Vector3 collisionVector;
	private bool isColliding;
	private float distmod;

	public void Start()
	{
		myTransform = transform;
		Vector3 angles = myTransform.eulerAngles;
		x = angles.y;
		y = angles.x;
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	public void LateUpdate()
	{
		if (!target)
		{
			return;
		}
		if (Input.GetMouseButtonUp(0))
		{
			prevButtonRight = false;
		}
		if (Input.GetMouseButtonUp(1))
		{
			prevButtonRight = true;
		}
		// If either mouse buttons are down, let them govern camera position
		if (Input.GetMouseButton(2))//|| Input.GetMouseButton(1))
		{
			x = x + ((Input.GetAxis("Mouse X") * xSpeed) * 0.02f);
			y = y - ((Input.GetAxis("Mouse Y") * ySpeed) * 0.02f);
		}
		else
		{
			// otherwise, ease behind the target if any of the directional keys are pressed
			if (prevButtonRight)
			{
				float targetRotationAngle = target.eulerAngles.y;
				float currentRotationAngle = myTransform.eulerAngles.y;
				x = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime);
			}
		}
		distance = distance - ((Input.GetAxis("Mouse ScrollWheel") * zoomRate) * Mathf.Abs(distance));// * Time.deltaTime
		distance = Mathf.Clamp(distance, minDistance, maxDistance);
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		Vector3 targetMod = new Vector3(0, -targetHeight, 0) - ((rotation * Vector3.right) * targetRight);
		int layerMask = 1 << 8;
		layerMask = ~layerMask;
		Vector3 position = target.position - (((rotation * Vector3.forward) * (distance - distmod)) + targetMod);
		Vector3 position2 = target.position - (((rotation * Vector3.forward) * 0.1f) + targetMod);

		//position = Vector3.Slerp(transform.position, position, Time.deltaTime * 100);   
		myTransform.rotation = rotation;
		myTransform.position = position;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)
		{
			angle = angle + 360;
		}
		if (angle > 360)
		{
			angle = angle - 360;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
