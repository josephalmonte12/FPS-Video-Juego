using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterControllerNew : MonoBehaviour {

	public GameObject main_Rotor_GameObject; // gameObject to be animated
	public GameObject tail_Rotor_GameObject; // gameObject to be animated
	public float max_Rotor_Force; // newtons
	public float max_Rotor_Velocity; // degrees per second
	public float rotor_Velocity; // value between 0 and 1
	private float rotor_Rotation; // degrees... used for animating rotors
	public float max_tail_Rotor_Force; // newtons
	public float max_Tail_Rotor_Velocity; // degrees per second
	private float tail_Rotor_Velocity; // value between 0 and 1
	private float tail_Rotor_Rotation; // degrees... used for animating rotors
	public float forward_Rotor_Torque_Multiplier; // multiplier for control input
	public float sideways_Rotor_Torque_Multiplier; // multiplier for control input
	public bool main_Rotor_Active; // boolean for determining if a prop is active
	public bool tail_Rotor_Active; // boolean for determining if a prop is active
	public float fuel;
	public GameObject noFuelSoundGO;
	public AudioClip rotatorSound;
	public GameObject rotatorTextureGO1;
	public GameObject rotatorTextureGO2;
	public GameObject bladesTextureGO;
	public float alpha;
	public GUISkin mySkin;
	public bool grounded;
	//@HideInInspector
	public bool damaged;
	//[HideInInspector]
	public bool controlsEnabled;
	public RaycastHit hit;
	public float rayDistance;
	public float forwardAccel;
	public float sidewaysAccel;
	public float sensitivity;
	public float adjust;
	public float stabilizacijasAtrums;
	public float verticalAccel;
	public ParticleSystem dustEffect;
	public LayerMask layerMask;
	public bool playerInside;
	public HelicopterHealthNew HP;
	//private Vector3 torqueValue;
	//gaining momentum and rotates around
	//private Vector3 controlTorque;

	public void Awake()
	{
		rotatorTextureGO1.GetComponent<Renderer>().enabled = false;
		rotatorTextureGO2.GetComponent<Renderer>().enabled = false;
	}

	public void Start()
	{
		dustEffect.Stop();
		GetComponent<AudioSource>().clip = rotatorSound;
		GetComponent<AudioSource>().loop = true;
		GetComponent<AudioSource>().Play();
	}

	public void FixedUpdate()
	{
		float moveForward = Input.GetAxis("Mouse Y");
		float moveSideways = Input.GetAxis("Mouse X");

		if (rotor_Velocity>0.3f)
		{
			if (moveForward>0.0f)
			{
				forwardAccel += Time.deltaTime * Mathf.Abs(moveForward) * sensitivity;
			}
			else if (moveForward < 0.0f)
			{
				forwardAccel -= Time.deltaTime * Mathf.Abs(moveForward) * sensitivity;
			}
			forwardAccel = Mathf.Clamp(forwardAccel, -1.0f, 1.0f);


			if (moveSideways>0.0f)
			{ //-->
				sidewaysAccel += Time.deltaTime * Mathf.Abs(moveSideways) * sensitivity;
			}
			else if (moveSideways < 0.0f)
			{ // <--
				sidewaysAccel -= Time.deltaTime * Mathf.Abs(moveSideways) * sensitivity;
			}
			sidewaysAccel = Mathf.Clamp(sidewaysAccel, -1.0f, 1.0f);
		}
		else
		{
			forwardAccel = 0.0f;
			sidewaysAccel = 0.0f;
		}

		adjust = rotor_Velocity - Mathf.Abs(moveForward) / 2;


		if (controlsEnabled)
		{
			Vector3 torqueValue = Vector3.zero;
			Vector3 controlTorque = new Vector3(forwardAccel * forward_Rotor_Torque_Multiplier, 1.0f, -sidewaysAccel * sideways_Rotor_Torque_Multiplier); // * sideways_Rotor_Torque_Multiplier

			// Now check if the main rotor is active, if it is, then add it's torque to the "Torque Value", and apply the forces to the body of the 
			// helicopter.
			if (main_Rotor_Active == true)
			{
				torqueValue += (controlTorque * max_Rotor_Force * rotor_Velocity);

				// Now the force of the prop is applied. The main rotor applies a force direclty related to the maximum force of the prop and the 
				// prop velocity (a value from 0 to 1)
				GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * max_Rotor_Force * rotor_Velocity / (3 - forwardAccel / 2));
				//paatrinaajums uz priekshu
				GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * forwardAccel * 28000 * rotor_Velocity);
				GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * sidewaysAccel * 10000 * rotor_Velocity);
				//rigidbody.AddRelativeForce ( Vector3.up * verticalAccel * 20000 * rotor_Velocity);

				if (Input.GetKey(KeyCode.S))
				{
					GetComponent<Rigidbody>().AddRelativeForce(-Vector3.up * 10000);
				}
				// This is simple code to help stabilize the helicopter. It essentially pulls the body back towards neutral when it is at an angle to
				// prevent it from tumbling in the air.
				if (Vector3.Angle(Vector3.up, transform.up) < 70)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0), Time.deltaTime * rotor_Velocity * stabilizacijasAtrums);
				}
			}

			// Now we check to make sure the tail rotor is active, if it is, we add it's force to the "Torque Value"
			if (tail_Rotor_Active == true)
			{
				torqueValue -= (Vector3.up * max_tail_Rotor_Force * tail_Rotor_Velocity);
			}

			// And finally, apply the torques to the body of the helicopter.
			GetComponent<Rigidbody>().AddRelativeTorque(torqueValue);
			grounded = false;
		}
	}

	public void OnCollisionStay(Collision col)
	{
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance))
		{
			grounded = true;
		}
	}

	public void Update()
	{

		// This line simply changes the pitch of the attached audio emitter to match the speed of the main rotor.
		if (controlsEnabled)
		{
			GetComponent<AudioSource>().volume = 1.0f;
		}
		else
		{
			if (rotor_Velocity < 0.03f)
				GetComponent<AudioSource>().volume -= Time.deltaTime / 3;
		}
		var adjustPitch = rotor_Velocity + 0.3f;
		adjustPitch = Mathf.Clamp(adjustPitch, 0.0f, 1.0f);
		GetComponent<AudioSource>().pitch = adjustPitch;



		alpha = 1 - rotor_Velocity;
		bladesTextureGO.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
		if (fuel>0 && rotor_Velocity>0.2)
		{
			fuel -= rotor_Velocity / 3 * Time.deltaTime;
		}
		if (alpha>0.8)
		{
			rotatorTextureGO1.GetComponent<Renderer>().enabled = false;
			rotatorTextureGO2.GetComponent<Renderer>().enabled = false;

		}
		else
		{
			rotatorTextureGO1.GetComponent<Renderer>().enabled = true;
			rotatorTextureGO2.GetComponent<Renderer>().enabled = true;
		}

		// Now we animate the rotors, simply by setting their rotation to an increasing value multiplied by the helicopter body's rotation.
		if (main_Rotor_Active == true)
		{
			main_Rotor_GameObject.transform.rotation = transform.rotation * Quaternion.Euler(0, rotor_Rotation / 2, 0);
		}
		if (tail_Rotor_Active == true)
		{
			tail_Rotor_GameObject.transform.rotation = transform.rotation * Quaternion.Euler(tail_Rotor_Rotation, 0, 0);
		}

		// this just increases the rotation value for the animation of the rotors.
		rotor_Rotation += max_Rotor_Velocity * rotor_Velocity * Time.deltaTime;
		tail_Rotor_Rotation += max_Tail_Rotor_Velocity * rotor_Velocity * Time.deltaTime;

		// here we find the velocity required to keep the helicopter level. With the rotors at this speed, all forces on the helicopter cancel 
		// each other out and it should hover as-is.
		var hover_Rotor_Velocity = (GetComponent<Rigidbody>().mass * Mathf.Abs((Physics.gravity.y) / max_Rotor_Force));
		var hover_Tail_Rotor_Velocity = (max_Rotor_Force * rotor_Velocity) / max_tail_Rotor_Force;

		if (damaged)
		{
			controlsEnabled = false;
		}

		if (controlsEnabled)
		{
			if (fuel>0.3f)
			{
				if (Input.GetAxis("Vertical")>0.2f)
				{
					rotor_Velocity += Input.GetAxis("Vertical") * (0.01f + forwardAccel / 400);

				}
				else if (Input.GetAxis("Vertical") == 0)
				{
					rotor_Velocity = Mathf.Lerp(rotor_Velocity, 0.4f, Time.deltaTime);
				}
			}

			if (Input.GetAxis("Vertical") < -0.2f)
			{
				rotor_Velocity = Mathf.Lerp(rotor_Velocity, hover_Rotor_Velocity, Time.deltaTime / 3);
			}

			if (rotor_Velocity>0.2)
			{
				tail_Rotor_Velocity = hover_Tail_Rotor_Velocity - Input.GetAxis("Horizontal");
			}
			else
			{
				tail_Rotor_Velocity = hover_Tail_Rotor_Velocity;
			}

		}
		else
		{
			if (!damaged)
			{
				rotor_Velocity = Mathf.Lerp(rotor_Velocity, 0.0f, Time.deltaTime / 10);
				GetComponent<Rigidbody>().AddRelativeForce(-Vector3.up * 10000 * rotor_Velocity);
				tail_Rotor_Velocity = 0;
			}
			else
			{
				tail_Rotor_Velocity = hover_Tail_Rotor_Velocity - Input.GetAxis("Horizontal");
			}
		}

		if (!grounded && !playerInside)
		{
			tail_Rotor_Velocity = hover_Tail_Rotor_Velocity - Input.GetAxis("Horizontal");
		}

		if (fuel < 0.3)
		{
			rotor_Velocity = Mathf.Lerp(rotor_Velocity, 0, Time.deltaTime / 15);
			noFuelSoundGO.GetComponent<AudioSource>().enabled = true;
		}
		if (grounded && rotor_Velocity < 0.1 && fuel <= 0.0)
		{
			noFuelSoundGO.GetComponent<AudioSource>().enabled = false;
		}


		// now we set velocity limits. The multiplier for rotor velocity is fixed to a range between 0 and 1. You can limit the tail rotor velocity 
		// too, but this makes it more difficult to balance the helicopter variables so that the helicopter will fly well.
		rotor_Velocity = Mathf.Clamp(rotor_Velocity, 0.0f, 1.0f);

		if (fuel < 0.1f)
		{
			fuel = 0f;
		}

		if (rotor_Velocity>0.2f)
		{

			var direction = gameObject.transform.TransformDirection(new Vector3(0, -1, 0));
			RaycastHit hit;
			var position = transform.position;

			if (Physics.Raycast(position, direction, out hit, 15, layerMask.value))
			{

				dustEffect.Play();
				var contact = hit.point;
				var rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
				dustEffect.transform.position = contact;
				dustEffect.transform.rotation = rotation;
			}
			else
			{
				dustEffect.Stop();
			}

		}
		else
		{
			dustEffect.Stop();
		}

	}

	/*public void OnGUI()
	{
		GUI.skin = mySkin;
		var style1 = mySkin.customStyles[1];

		if (controlsEnabled)
		{

			GUI.Label(new Rect(Screen.width - 90, Screen.height - 50, 200, 80), "" + fuel.ToString("f2"), style1);
			GUI.Label(new Rect(Screen.width - 160, Screen.height - 50, 200, 80), " Fuel: ");

		}
	}*/
}
