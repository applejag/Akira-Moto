using UnityEngine;
using System.Collections;

public class ParallaxScript : MonoBehaviour {

	[Range(0f,1f)]
	public float scale = 1f;
	public float offset = 0f;
	public float offsetMotion = 0f;
	public bool wrap = false;
	public float wrapAfter = 64f;

	void LateUpdate () {
		var pos = transform.position;

		// Alter
		offset += offsetMotion * Time.deltaTime;

		pos.x = Camera.main.transform.position.x * scale + offset;

		if (wrap)
			offset %= wrapAfter;

		// Apply
		transform.position = pos;
	}
}
