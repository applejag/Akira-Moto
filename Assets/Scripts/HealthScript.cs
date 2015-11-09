using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int health = 1;
	public int maxHealth = 1;
	public bool isEnemy = true;

	void OnValidate() {
		health = Mathf.Max (health, 0);
		maxHealth = Mathf.Max (health, maxHealth);
	}

	public void Damage(int damageCount)	{
		health -= damageCount;
		SendMessage ("OnHealthChange", SendMessageOptions.DontRequireReceiver);

		if (health <= 0) {
			// Dead!
			Destroy (gameObject);

			// MIACHEKL BEJ
			SpecialEffectsHelper.Explosion (transform.position);
			SoundEffectsHelper.PlayExplosionSound ();
		}
	}
	
	void OnTriggerEnter2D(Collider2D otherCollider)	{
		// Is this a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null) {
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy) {
				Damage(shot.damage);
				
				// Destroy the shot
				Destroy(shot.gameObject);
			}
		}
	}
}
