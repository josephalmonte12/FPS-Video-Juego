using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScripotExplosion : MonoBehaviour {

	public GameObject expl;

	public void Activate(){
		Instantiate(expl, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
