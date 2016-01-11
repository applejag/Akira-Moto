using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	[HideInInspector]
	public int health, maxHealth = 1;
	[HideInInspector]
	public float timePerDamage = 60; // seconds

	public bool isEnemy = true;
	public bool isPlayer { get { return !isEnemy; } }

	private bool dead;

#if UNITY_EDITOR
	void OnValidate() {
		maxHealth = Mathf.Max(health, maxHealth);
		health = Mathf.Clamp(health, 0, maxHealth);
	}
#endif

	void Start() {
		if (isEnemy)
			SpawnerScript.instance.enemiesAlive++;
	}

	public void ModifyHealth(int delta, bool raw = false) {
		if (dead) return;
		if (isPlayer && GameOverScript.instance.over) return;

		// Limit it; because health can't go below 0 and above /maxHealth/
		delta = Mathf.Clamp(delta, -health, maxHealth - health);

		if (isEnemy || raw) {
			// Change the health
			health += delta;

			// Check if dead
			if (health == 0) {
				dead = true;

				SendMessage("OnDeath");

				if (isEnemy) {
					SpawnerScript.instance.enemiesAlive--;
				}
			}
		} else if (isPlayer && !raw) {
			ScoreKeeperScript.instance.AddTime(delta * timePerDamage);
		}
	}
	
}
