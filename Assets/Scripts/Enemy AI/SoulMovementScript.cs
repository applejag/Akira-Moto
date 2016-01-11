using UnityEngine;
using System.Collections;

public class SoulMovementScript : MonoBehaviour {

	//public ParallaxScript parallax;
	public float directSpeed = 1f;
	public float verticalSpeed = 1f;

	void Awake() {
		transform.SetParent(MoonScript.instance.soulTarget);
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1);
		//parallax.scale = 0;
		//parallax.offset = transform.position.x;
	}

	// TODO: REWRITE THIS
	void Update() {

		Vector3 delta = MoonScript.instance.soulTarget.position - transform.position;
		delta.z = 0;
		Vector3 dir = delta.normalized;

		// Set movementspeed
		Vector2 velocity = new Vector2(dir.x, dir.y) * directSpeed + new Vector2(0, delta.y) * verticalSpeed;

		// Rotate
		transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);

		// Move
		transform.position += new Vector3(velocity.x, velocity.y) * Time.deltaTime;
		//parallax.offsetMotion = velocity.x;

		// Update parallax
		//parallax.scale = transform.position.y / MoonScript.instance.soulTarget.position.y;

		// Am I there yet?
		if (delta.magnitude <= MoonScript.instance.radius ) {
			MoonScript.instance.CollectSoul(this);
		}
	}

}
