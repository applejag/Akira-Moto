using UnityEngine;
using System.Collections;

public class SpecialEffectsHelper : SingletonBaseScript<SpecialEffectsHelper> {

	public ParticleSystem smokeEffect;
	public ParticleSystem fireEffect;

	public static void Explosion(Vector3 position) {
		// zUwEIt9ez7M
		SpawnEffect (instance.smokeEffect, position);
		SpawnEffect (instance.fireEffect, position);
	}

	public static void SpawnEffect(ParticleSystem prefab, Vector3 position) {
		// Spawn 'et
		var clone = Instantiate(prefab, position, Quaternion.identity) as ParticleSystem;

		// KILL 'ET
		Destroy (clone.gameObject, clone.startLifetime);
	}
}
