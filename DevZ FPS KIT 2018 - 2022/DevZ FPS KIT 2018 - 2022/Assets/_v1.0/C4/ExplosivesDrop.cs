using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenLocker;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class ExplosivesDrop : MonoBehaviour {

	public int count = 5;
	public GameObject explosive;
	public Transform spawn;
	public GameObject[] dropped;
	public int dropCount = 0;

	private bool canDrop = true;
	private float dropTimer;
	private float dropTime = 0.7f;
	public float pressTimer = 0.5f;

	public bool activs = false;
	public WeaponsPlayer pw;
	public Sprite C4Texture;
	public Transform kickGO;
	public PlayerController ccontroller;
	public ScriptExplosives explosivesScript;
	public DynamicCrosshairNew crosshair;
	public C4AnimationScript animationScript;
	public ControllerGadget GC;
	public GameObject mainCamera;

	public bool knifeOrGrenade = false;

	public void Start()
	{
		dropped = new GameObject[count];
		dropTimer = dropTime;
		activs = false;
	}

	public void Update()
	{
		if (!ScreenLock.lockCursor) return;
		if (activs)
		{
			if (knifeOrGrenade)
			{
				ccontroller.canRun = false;
				ccontroller.run = false;
				//canFire = false;
				//canAim = false;
				pw.canSwitch = false;
			}
			else
			{
				//canFire = true;
				//canAim = true;
				pw.canSwitch = true;
				ccontroller.canRun = true;
			}
			if (!knifeOrGrenade)
			{
				if (ccontroller.running)
				{
					return;
				}
				if (Input.GetKeyDown("q"))
				{
					StartCoroutine(FastKnife());
				}
				if (Input.GetKeyDown("g") && (explosivesScript.explType != Exposives.NONE))
				{
					if (((explosivesScript.pull == false) && (explosivesScript.readyToThrow == false)) && (explosivesScript.grenadeCount > 0))
					{
						StartCoroutine(EventGrenadeThrow(1));
					}
				}
				if (Input.GetButtonDown("Fire1"))
				{
					if (canDrop)
					{
						dropTimer = dropTime;
						StartCoroutine(Drop());
					}
				}
				if (Input.GetButtonDown("Fire2"))
				{
					if (pressTimer <= 0f)
					{
						animationScript.ActivateExplosives();
						pressTimer = 0.5f;
					}
					if (dropped[0] != null)
					{
						StartCoroutine(Expload());
					}
				}
				kickGO.localRotation = Quaternion.Slerp(kickGO.localRotation, Quaternion.identity, Time.deltaTime * 7);
			}
			if (explosivesScript.readyToThrow == true)
			{
				if (Input.GetKeyUp("g"))
				{
					StartCoroutine(EventGrenadeThrow(2));
				}
				else
				{
					if (!Input.GetKey("g"))
					{
						StartCoroutine(EventGrenadeThrow(2));
					}
				}
			}
		}
		if (dropTimer > 0f)
		{
			dropTimer = dropTimer - Time.deltaTime;
		}
		if (pressTimer > 0f)
		{
			pressTimer = pressTimer - Time.deltaTime;
		}
		if (dropTimer <= 0f)
		{
			canDrop = true;
		}
		else
		{
			canDrop = false;
		}
	}

	/*public void OnGUI()
	{
		int i = 0;
		while (i < count)
		{
			GUI.DrawTexture(new Rect(150 + (i * 35), Screen.height - 75, 70, 70), C4Texture);
			i++;
		}
	}*/

	public virtual IEnumerator Drop()
	{
		if ((count > 0) && canDrop)
		{
			animationScript.DropExplosives();
			yield return new WaitForSeconds(0.17f);
			GameObject temp = Instantiate(explosive, spawn.position, spawn.rotation);
			dropped[dropCount] = temp;
			count--;
			dropCount++;
		}
	}

	public virtual IEnumerator Expload()
	{
		int i = 0;
		while (i < dropped.Length)
		{
			yield return new WaitForSeconds(0.1f);
			if (dropped[i] != null)
			{
				ScripotExplosion explScript = dropped[i].GetComponent<ScripotExplosion>();
				explScript.Activate();
			}
			i++;
		}
		dropCount = 0;
		dropTimer = dropTime;
	}

	public void ActivateGadget()
	{
		if (!activs)
		{
			GC.enabled = true;
			crosshair.showCrosshair = false;
			RendererBool(true);
			animationScript.DrawWeapon();
			activs = true;
			pw.switching = false;
			pw.canSwitch = true;
			ccontroller.canRun = true;
		}
	}

	public virtual IEnumerator Deactivate()
	{
		if (activs)
		{
			animationScript.PutDownWeapon();
			yield return new WaitForSeconds(0.5f);
			GC.enabled = false;
			activs = false;
			pw.switching = false;
			pw.canSwitch = false;
			RendererBool(false);
		}
	}

	public virtual IEnumerator EventGrenadeThrow(int status)
	{
		if (status == 1) //Prepare to throw
		{
			StartCoroutine(DisableFunctionality(0.3f));
			StartCoroutine(explosivesScript.Pull(status));
		}
		if (status == 2) //Throw Grenade
		{
			if (explosivesScript.throwing == true)
			{
				yield break;
			}
			StartCoroutine(explosivesScript.Throw(status));
			yield return new WaitForSeconds(1f);
			StartCoroutine(EnableFunctionality());
		}
	}

	public virtual IEnumerator FastKnife()
	{
		RendererBool(false); //Hide weapon
		StartCoroutine(DisableFunctionality(0.3f));
		mainCamera.SendMessage("KnifeAttack");
		yield return new WaitForSeconds(0.4f);
		StartCoroutine(EnableFunctionality());
	}

	public virtual IEnumerator DisableFunctionality(float waitTime)
	{
		//Throwing grenade, or using knife.
		knifeOrGrenade = true;
		//readyToThrow = true;
		animationScript.QuickPutDownWeapon();
		yield return new WaitForSeconds(waitTime);
		RendererBool(false);
	}

	public virtual IEnumerator EnableFunctionality()
	{
		if (ccontroller.onLadder)
		{
			yield break;
		}
		//Returning from grenade, or knife attack.
		RendererBool(true);
		animationScript.DrawWeapon();
		yield return new WaitForSeconds(1f);
		knifeOrGrenade = false;
	}

	public void RendererBool(bool status)
	{
		var gos = GetComponentsInChildren(typeof(Renderer));
		foreach (Renderer go in gos)
		{
			if (go.name != "MuzzleFlash")
			{
				go.enabled = status;
			}
		}
	}
}
