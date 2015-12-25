using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadingTransitionScript : MonoBehaviour {

	public delegate void Callback();

	public Image image;
	public float fadingTime = 1f;
	public bool fadeInOnAwake = true;

	private Callback callback;
	private bool active = false;
	private bool fadeIn = false;

	void Awake() {
		if (fadeInOnAwake)
			StartTransition(true);
	}

	void Update() {
		if (active) {
			float target = fadeIn ? 0f : 1f;
			float alpha = Mathf.MoveTowards(image.color.a, target, Time.deltaTime / fadingTime);

			image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
			
			if (alpha == target) {
				// Transition complete
				active = fadeIn = false;
				if (callback != null)
					callback();
			}
		}
	}

	public void StartTransition(bool fadeIn, Callback callback) {
		this.fadeIn = fadeIn;
		image.color = fadeIn ? Color.black : Color.clear;
		active = true;
		this.callback = callback;
	}

	public void StartTransition(bool fadeIn) {
		this.fadeIn = fadeIn;
		image.color = fadeIn ? Color.black : Color.clear;
		active = true;
	}

}
