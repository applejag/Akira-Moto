using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class EnemyScript : MonoBehaviour {

    public List<WeaponScript> weapons = new List<WeaponScript>();
	public MoverScript moverScript;
	public Collider2D col;
	public Renderer ren;

	private bool hasSpawned = false;
	
	void Start() {
		// Disable everything
		Reset ();
	}

	void Update() {
		// Check if the enemy has spawned
		if (hasSpawned) {
			// Auto-fire
			bool success = false;
			foreach (var weapon in weapons) {
				success |= weapon.Attack(true);
			}
			// Play sound effect if ANY of the weapons fired successfully
			if (success)
				SoundEffectsHelper.PlayEnemyShotSound();

			// Outside camera bounds? Destroy the game object
			if (ren.IsVisableFrom(Camera.main) == false) {
				Destroy(gameObject);
			}
		} else {
			// Wait until inside camera bounds
			if (ren.IsVisableFrom(Camera.main)) {
				// Spawn
				Spawn();
			}
		}
	}

	private void Reset() {
		hasSpawned = false;

		// Disable everything
		// -- collider
		col.enabled = false;
		// -- moving
		moverScript.enabled = false;
		// -- shooting
		foreach (var weapon in weapons) {
			weapon.enabled = false;
		}
	}

	private void Spawn() {
		hasSpawned = true;

		// Enable everything
		// -- collider
		col.enabled = true;
		// -- moving
		moverScript.enabled = true;
		// -- shooting
		foreach (var weapon in weapons) {
			weapon.enabled = true;
		}
	}
}
