using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class c4RaycastNew : MonoBehaviour {

	public LayerMask layerMask;
	public AudioClip soundHit;
	private Vector3 velocity;
	private Vector3 newPos;
	private Vector3 oldPos;
	private bool hasHit;
	private Vector3 direction;
	private RaycastHit hit;
	public float speed;
	public Transform adjustRotation;
	public float forceToApply;
	public float objectGravity;
	private GameObject follow;
	public Transform adjustRot;
	public Transform adjustRotY;
	public void Start()
	{
		newPos = transform.position;
		oldPos = newPos;
		velocity = speed * transform.forward;
		direction = transform.TransformDirection(new Vector3(0, 0, 1));
	}

	public void Update()
	{
		if (hasHit)
		{
			if (follow != null)
			{
				transform.position = follow.transform.position;
				transform.rotation = follow.transform.rotation;
			}
			return;
		}
		newPos = newPos + ((velocity + direction) * Time.deltaTime);
		Vector3 dir = newPos - oldPos;
		float dist = dir.magnitude;
		dir = dir / dist;
		if (dist > 0)
		{
			if (Physics.Raycast(oldPos, dir, out hit, dist, layerMask))
			{
				newPos = hit.point;
				if (hit.collider)
				{
					Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
					StartCoroutine(SetRotation(rot));
					GetComponent<AudioSource>().PlayOneShot(soundHit, 0.3f);
					if (hit.collider.tag == "Metal")
					{
						GameObject hitPoint = UnityEngine.Object.Instantiate(new GameObject(), hit.point, transform.root.rotation);
						hitPoint.transform.localPosition = hitPoint.transform.localPosition + (0.04f * hit.normal);
						hitPoint.transform.parent = hit.transform;
						follow = hitPoint;
						//collider.isTrigger = true;
						hasHit = true;
					}
					else
					{
						enabled = false;
					}
				}
			}
		}
		oldPos = transform.position;
		transform.position = newPos;
		velocity.y = velocity.y - (objectGravity * Time.deltaTime);
		adjustRotation.transform.rotation = Quaternion.LookRotation(dir);
	}

	public void OnCollisionEnter(Collision col)
	{
		GetComponent<AudioSource>().PlayOneShot(soundHit, 0.3f);
	}

	public virtual IEnumerator SetRotation(Quaternion targetRot)
	{
		float t = 0f;
		while (t < 1f)
		{
			t = t + (Time.deltaTime * 100);
			adjustRot.rotation = Quaternion.Slerp(adjustRot.rotation, targetRot, t);
			adjustRotY.localRotation = new Quaternion(0f, transform.rotation.y, 0f, 0f);
			//adjustRotY.localRotation.y = transform.rotation.y;
			yield return null;
		}
	}
}
