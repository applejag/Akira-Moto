using UnityEngine;
using System.Collections;

public class SoundEffectsHelper : MonoBehaviour {

	public static SoundEffectsHelper instance;

	public AudioClip explosionSound;
	public AudioClip playerShotSound;
	public AudioClip enemyShotSound;

	void Awake () {
		if (instance != null) {
			print ("WAZAAAAAAAAAAAAAAAAA MAH BROTHEREN");
		}
		
		instance = this;
	}

	public static void PlayExplosionSound() {
		PlaySound (instance.explosionSound);
	}

	public static void PlayPlayerShotSound() {
		PlaySound (instance.playerShotSound);
	}

	public static void PlayEnemyShotSound() {
		PlaySound (instance.enemyShotSound);
	}

	private static void PlaySound(AudioClip clip) {
		AudioSource.PlayClipAtPoint (clip, instance.transform.position);
	}
}
