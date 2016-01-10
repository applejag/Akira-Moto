using UnityEngine;
using System.Collections;

public class MoonScript : MonoBehaviour {

	public float baseScale = 1f;
	public float speed = 1f;
	[Space]
	[Range(0f,1f)]
	public float percentage = 1f;

	[Range(0f,1f)]
	public float current;
	private bool reachedTop = false;

#if UNITY_EDITOR
	private float old_current;
	void OnValidate() {

		if (old_current != current) {
			current = Mathf.MoveTowards(current, percentage, speed * Time.deltaTime);
			transform.localScale = new Vector3(transform.localScale.x, current * baseScale, transform.localScale.z);
			percentage = current;
		}

		old_current = current;
	}
#endif

	void Update() {
		current = Mathf.MoveTowards(current, percentage, speed * Time.deltaTime);
		transform.localScale = new Vector3(transform.localScale.x, current * baseScale, transform.localScale.z);

		bool approx = Mathf.Approximately(current, 1);
        if (approx && !reachedTop) {
			ScoreKeeperScript.instance.MoonFilled();
			reachedTop = true;
		}
		if (!approx && reachedTop) {
			reachedTop = false;
		}
	}

}
