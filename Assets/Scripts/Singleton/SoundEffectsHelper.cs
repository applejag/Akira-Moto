using UnityEngine;
using System.Collections;

public class SoundEffectsHelper : SingletonBaseScript<SoundEffectsHelper> {

	public Sound glassShatter;
	public Sound swoosh;
	[HideInInspector]
	public Sound[] dirt;

	public void PlayDirtSound(Vector3 position) {
		dirt[Random.Range(0, dirt.Length)].PlaySound(position);
	}

	public void PlayGlasShatterSound(Vector3 position) {
		glassShatter.PlaySound(position);
	}

	// Player attack
	public void PlaySwooshSound(Vector3 position) {
		swoosh.PlaySound(position);
	}

	/*
	public static void PlayExplosionSound() {
		instance.explosionSound.PlaySound();
	}

	public static void PlayPlayerShotSound() {
		instance.playerShotSound.PlaySound();
	}

	public static void PlayEnemyShotSound() {
		instance.enemyShotSound.PlaySound();
	}

	public static void PlayLifeSound() {
		instance.lifeSound.PlaySound();
	}
	*/

	private static void PlaySound(AudioClip clip, Vector3 position, float volume = 1.0f) {
		AudioSource.PlayClipAtPoint(clip, position, volume);
	}

	private static void PlaySound(AudioClip clip, float volume = 1.0f) {
		PlaySound (clip, instance.transform.position, volume);
	}


	[System.Serializable]
	public class Sound {
		public AudioClip clip = null;
		[Range(0f,1f)]
		public float volume = 1f;

		public void PlaySound() {
			SoundEffectsHelper.PlaySound(clip, volume);
		}

		public void PlaySound(float volumeOverride) {
			SoundEffectsHelper.PlaySound(clip, volumeOverride);
		}

		public void PlaySound(Vector3 position) {
			SoundEffectsHelper.PlaySound(clip, position, volume);
		}

		public void PlaySound(Vector3 position, float volumeOverride) {
			SoundEffectsHelper.PlaySound(clip, position, volumeOverride);
		}
	}
}
