using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class ScrollingScript : MonoBehaviour {

	public float speed = 2f;
	public float angle = 180f;

	void Update() {
		// Movement
		Vector3 motion = VectorHelper.FromDegrees (angle).ToVector3 () * speed;

		motion *= Time.deltaTime;
		transform.Translate (motion);
	}

}