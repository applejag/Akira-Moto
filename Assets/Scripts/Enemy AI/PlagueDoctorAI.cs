using UnityEngine;
using System.Collections;
using System;

public class PlagueDoctorAI : BaseAI {

	protected override bool turnWithAnimation { get { return false; } }

	[Header("PlagueDoctorAI")]
	public ParticleSystem[] bottleParticles;
	public GameObject prefab;
	public Transform spawnPoint;

	protected override void StateChange(State last) {
		base.StateChange(last);

		if (state == State.dead) {
			// Disable all particles
			for (int i=0; i<bottleParticles.Length; i++) {
				BottleParticlesOFF(i);
            }
		}
	}

	#region Animation events
	/*
	 * Methods called by animations.
	*/

	public void BottleParticlesON(int index) {
		var em = bottleParticles[index].emission;
		if (!em.enabled)
			em.enabled = true;
	}

	public void BottleParticlesOFF(int index) {
		var em = bottleParticles[index].emission;
		if (em.enabled)
			em.enabled = false;
	}

	public void SpawnProjectile() {
		var clone = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		clone.transform.parent = transform.parent;
		clone.transform.localScale = transform.localScale;
		var damage = clone.GetComponent<DamageScript>();
		damage.source = health;
	}

	public void SpawnSoul() {
		MoonScript.instance.SpawnSoulAt(transform.position);
	}
	#endregion
}
