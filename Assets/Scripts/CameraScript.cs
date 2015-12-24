using UnityEngine;
using System.Collections;
using ExtensionMethods;
using UnityStandardAssets.ImageEffects;

public class CameraScript : MonoBehaviour {

	public PlayerScript target;
	public float speed = 1f;
	public bool includeOffset = true;
	public float velocityScale = 1f;
	[Space]
	public bool ignoreX = false;
	public bool ignoreY = false;

	[Header("Image effects (Post-processing)")]

	public VignetteAndChromaticAberration vignette;
	public float randomVignetting;
	public float vignettingSpeed = 1f;
	private float startVignetting;

	private Vector3 offset;

	void Start() {
		if (target != null && includeOffset)
			offset = transform.position - target.transform.position;

		if (vignette != null)
			startVignetting = vignette.intensity;
	}

	void Update() {

		if (vignette != null && randomVignetting != 0f)
			vignette.intensity = Mathf.Lerp(vignette.intensity, startVignetting + Random.Range(-randomVignetting, randomVignetting), Time.deltaTime * vignettingSpeed);

		if (target == null)
			return;
		
		Vector3 to = target.transform.position;
		if (includeOffset) to += offset;
		to += target.body.velocity.ToVector3() * velocityScale;

		if (ignoreX) to.x = transform.position.x;
		if (ignoreY) to.y = transform.position.y;

		transform.position = Vector3.Lerp(transform.position, to, speed * Time.deltaTime);
	}

}
