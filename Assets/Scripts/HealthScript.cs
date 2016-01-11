using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int health = 1;
	public int maxHealth = 1;
	public bool isEnemy = true;

	private bool dead;	
	public bool updatesUI { get { return HealthGUIScript.instance != null && HealthGUIScript.instance.health == this; } }

#if UNITY_EDITOR
	void OnValidate() {
		maxHealth = Mathf.Max(health, maxHealth);
		health = Mathf.Clamp(health, 0, maxHealth);
	}
#endif

	void Start() {
		if (updatesUI) {
			HealthGUIScript.instance.UpdateUIElements(health, maxHealth);
		}

		if (isEnemy)
			SpawnerScript.enemiesAlive++;
	}

	public void ModifyHealth(int delta) {
		if (dead) return;
		if (!isEnemy && GameOverScript.instance.over) return;

		// Limit it; because health can't go below 0 and above /maxHealth/
		delta = Mathf.Clamp(delta, -health, maxHealth - health);

		// Change the health
		health += delta;

		// Check if dead
		if (health == 0) {
			dead = true;

			SendMessage("OnDeath");

			if (isEnemy) {
				SpawnerScript.enemiesAlive--;
			}
		}

		if (updatesUI)
			HealthGUIScript.instance.UpdateUIElements(health, maxHealth);
	}
	
}
