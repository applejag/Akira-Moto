using UnityEngine;
using System.Collections;

public class SoundEffectsHelper : MonoBehaviour {

	public static SoundEffectsHelper instance;

	public Sound explosionSound;
	public Sound playerShotSound;
	public Sound enemyShotSound;
	public Sound lifeSound;

	void Awake () {
		if (instance != null) {
			Debug.LogError ("WAZAAAAAAAAAAAAAAAAA MAH BROTHEREN");
		}
		
		instance = this;
	}

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

	private static void PlaySound(AudioClip clip, float volume = 1.0f) {
		AudioSource.PlayClipAtPoint (clip, instance.transform.position, volume);
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
	}
}
