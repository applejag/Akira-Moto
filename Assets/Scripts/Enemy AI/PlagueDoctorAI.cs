using UnityEngine;
using System.Collections;
using System;

public class PlagueDoctorAI : BaseAI {

	[Header("PlagueDoctorAI")]
	public ParticleSystem[] bottleParticles;
	public GameObject prefab;
	public Transform spawnPoint;
	public Animator anim;

	void FixedUpdate() {
		if (player == null) {
			// Idle
			anim.SetBool("Attack", false);
			anim.SetFloat("Movement", 0f);
			anim.SetBool("Walking", false);
            return;
		}

		bool attack = WalkIntoAttackRange();

		anim.SetBool("Attack", attack);
		anim.SetBool("Walking", Mathf.Abs(body.velocity.x) > 0.5f);
		anim.SetFloat("Movement", Mathf.Abs(body.velocity.x));
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
	}

	#endregion
}
