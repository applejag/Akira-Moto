using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int health = 1;
	public int maxHealth = 1;
	public bool isEnemy = false;

#if UNITY_EDITOR
	void OnValidate() {
		maxHealth = Mathf.Max(health, maxHealth);
		health = Mathf.Clamp(health, 0, maxHealth);
	}
#endif

	public void ModifyHealth(int delta) {
		// Limit it; because health can't go below 0 and above /maxHealth/
		delta = Mathf.Clamp(delta, -health, maxHealth - health);

		// Change the health
		health += delta;

		// Send the event
		if (delta < 0)
			SendMessage("OnDamaged", delta, SendMessageOptions.DontRequireReceiver);
		else if (delta > 0)
			SendMessage("OnHealed", delta, SendMessageOptions.DontRequireReceiver);

	}

	void OnDamaged(int delta) {
		if (health <= 0) {
			Destroy(gameObject);
		}
	}
}
