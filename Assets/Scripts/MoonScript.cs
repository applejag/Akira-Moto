using UnityEngine;
using System.Collections;

public class MoonScript : SingletonBaseScript<MoonScript> {

	[Header("Souls")]
	public GameObject soulPrefab;
	public ParticleSystem part;
	public Transform soulTarget;
	public float radius = 1f;
	[Header("Moon cover")]
	public Transform cover;
	public float baseScale = 1f;
	public float speed = 1f;
	[Space]
	[Range(0f,1f)]
	public float percentage = 1f;

	[Range(0f,1f)]
	public float current;
	private bool reachedTop = false;

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		UnityEditor.Handles.color = Color.green;
		UnityEditor.Handles.CircleCap(0, soulTarget.position, Quaternion.identity, radius);
	}

	private float old_current;
	void OnValidate() {

		if (old_current != current && cover != null) {
			current = Mathf.MoveTowards(current, percentage, speed * Time.deltaTime);
			cover.localScale = new Vector3(cover.localScale.x, current * baseScale, cover.localScale.z);
			percentage = current;
		}

		old_current = current;
	}
#endif

	void Update() {
		current = Mathf.MoveTowards(current, percentage, speed * Time.deltaTime);
		cover.localScale = new Vector3(cover.localScale.x, current * baseScale, cover.localScale.z);

		bool approx = Mathf.Approximately(current, 1);
        if (approx && !reachedTop) {
			ScoreKeeperScript.instance.MoonFilled();
			reachedTop = true;
		}
		if (!approx && reachedTop) {
			reachedTop = false;
		}
	}

	public void CollectSoul(SoulMovementScript soul) {
		Destroy(soul.gameObject);
		part.Play();
		ScoreKeeperScript.instance.score++;
	}

	public void SpawnSoulAt(Vector3 position) {
		Instantiate(soulPrefab, position, soulPrefab.transform.rotation);
	}

}
