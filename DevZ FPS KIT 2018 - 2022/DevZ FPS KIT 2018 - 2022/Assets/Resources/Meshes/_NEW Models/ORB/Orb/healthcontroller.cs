using UnityEngine;
using System.Collections;

public class RayAndHit {
	public Ray ray;
	public RaycastHit hit;
	public RayAndHit(Ray ray, RaycastHit hit) {
		this.ray = ray;
		this.hit = hit;
	}
}

public class HealthController : MonoBehaviour {
	
	public GameObject deathHandler;
	public float maxHitPoints = 100;
	public float hitDamage = 3;
	public float healingSpeed = 2;
	[HideInInspector]
	public float hitPoints;
	public float damage;
	
	public float normalizedHealth { get { return hitPoints / maxHitPoints; } }
	
	// Use this for initialization
	void OnEnable () {
		hitPoints = maxHitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.deltaTime == 0 || Time.timeScale == 0)
			return;
		
		if (hitPoints > 0)
			hitPoints += Time.deltaTime * healingSpeed;
		hitPoints = Mathf.Clamp(hitPoints, 0, maxHitPoints);
	}
	
	void ApplyDamage () {
		maxHitPoints -= damage;
		hitPoints = Mathf.Clamp(hitPoints, 0, maxHitPoints);
		
	}
}
