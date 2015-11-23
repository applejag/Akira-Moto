using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class CameraScript : MonoBehaviour {

	public PlayerScript target;
	public float speed = 1f;
	public bool includeOffset = true;
	public float velocityScale = 1f;

	private Vector3 offset;

	void Start() {
		if (target != null && includeOffset)
			offset = transform.position - target.transform.position;
	}

	void Update() {
		if (target == null)
			return;
		
		Vector3 to = target.transform.position;
		if (includeOffset) to += offset;
		to += target.rbody.velocity.ToVector3() * velocityScale;

		transform.position = Vector3.Lerp(transform.position, to, speed * Time.deltaTime);
	}

}
