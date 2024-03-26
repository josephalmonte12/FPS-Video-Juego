using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotorHitNew : MonoBehaviour {

	public GameObject hitPrefab;	
	public HelicopterHealthNew hhp;

	public void OnTriggerEnter ()
	{
	
		//hit effect
		Instantiate( hitPrefab, transform.position, transform.rotation );
		if(hhp.hitPoints > 100){
			hhp.ApplyDamage(100);
		}	
	}
}
