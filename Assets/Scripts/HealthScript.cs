using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int health = 1;
	public int maxHealth = 1;
	public bool isPlayer = false;
	public bool rewardOnDeath = false;

	public static HealthScript player;

	void Awake() {
		if (isPlayer) {
			if (player != null)
				Debug.LogError("There can only be one player!");

			player = this;
			HealthGUIScript.UpdateHearts(source: this);
		}
	}

	void Start() {
		if (isPlayer) {
			HealthGUIScript.UpdateHearts(source: player);
		}
	}

#if UNITY_EDITOR
	void OnValidate() {
		health = Mathf.Max (health, 0);
		maxHealth = Mathf.Max (health, maxHealth);
	}
#endif

	public void Damage(int damageCount)	{
		health = Mathf.Clamp(health - damageCount, 0, maxHealth);
		if (isPlayer)
			HealthGUIScript.UpdateHearts(source:this);

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
			if (shot.isEnemyShot == isPlayer) {
				// Take damage
				Damage(shot.damage);
				
				// Destroy the shot
				Destroy(shot.gameObject);

				// Health rewards
				if (rewardOnDeath) {
					Destroy(gameObject);
					SoundEffectsHelper.PlayLifeSound();

					// Heal the player
					player.Damage(-1);
				}
			}
		}
	}
}
