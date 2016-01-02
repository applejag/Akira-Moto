using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int health = 1;
	public int maxHealth = 1;
	public bool isEnemy = true;
	public bool updatesUI = false;

	private static HealthScript ui;

#if UNITY_EDITOR
	void OnValidate() {
		maxHealth = Mathf.Max(health, maxHealth);
		health = Mathf.Clamp(health, 0, maxHealth);
	}
#endif

	void Start() {
		if (updatesUI) {
			// see if theres multiple ui updators'
			if (ui != null)
				Debug.LogError("There should only be one that updates the ui!!");
			ui = this;

			HealthGUIScript.instance.UpdateUIElements(health, maxHealth);
		}

		if (isEnemy)
			SpawnerScript.enemiesAlive++;
	}

	public void ModifyHealth(int delta) {
		// Limit it; because health can't go below 0 and above /maxHealth/
		delta = Mathf.Clamp(delta, -health, maxHealth - health);

		// Change the health
		health += delta;

		// Check if dead
		if (health == 0) {
			Destroy(gameObject);

			if (isEnemy)
				SpawnerScript.enemiesAlive--;
		}

		if (updatesUI)
			HealthGUIScript.instance.UpdateUIElements(health, maxHealth);
	}
	
}
