using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageScript : MonoBehaviour {

	public bool isEnemy = false;
	public int damage = 1;
	public bool dieOnStart = true;

	private List<HealthScript> damaged = new List<HealthScript>();

	public IEnumerator Start() {
		yield return new WaitForFixedUpdate();

		// Will only live for one frame
		if (dieOnStart)
			Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D col) {
		DealDamage(col.gameObject.GetComponent<HealthScript>());
	}

	void OnTriggerEnter2D(Collider2D other) { 
		DealDamage(other.GetComponent<HealthScript>());
	}

	void DealDamage(HealthScript health) {
		if (health != null && health.isEnemy != isEnemy && !damaged.Contains(health)) {
			// Can only damage each healthScript once (per damage/attack)
			damaged.Add(health);
			
			// Deal the damage
			health.ModifyHealth(-damage);

			// Send messages
			/*
			health.SendMessage("TookDamage", this, SendMessageOptions.DontRequireReceiver);
			SendMessage("DealtDamage", this, SendMessageOptions.DontRequireReceiver);
			*/
		}
	}

	public static DamageScript SpawnDamage(Vector3 position, float radius, int damage, bool isEnemy) {
		// Create the object
		var damageObject = new GameObject("Damage");
		damageObject.transform.position = position;

		// Add components
		var circleCollider = damageObject.AddComponent<CircleCollider2D>();
		var damageScript = damageObject.AddComponent<DamageScript>();
		var rbody = damageObject.AddComponent<Rigidbody2D>();

		// Init components
		circleCollider.radius = radius;
		circleCollider.isTrigger = true;
		damageScript.isEnemy = isEnemy;
		damageScript.damage = damage;
		rbody.gravityScale = 0;

		// Return referance
		return damageScript;
	}

}
