using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class MimicSpriteScript : MonoBehaviour {

	public SpriteRenderer otherRenderer;

	private SpriteRenderer myRenderer;
	private float skew;

	void Start() {
		myRenderer = GetComponent<SpriteRenderer>();
		skew = myRenderer.material.GetFloat("_HorizontalSkew");
	}
	
	void LateUpdate () {
		// Same sprite
		myRenderer.sprite = otherRenderer.sprite;

		myRenderer.material.SetFloat("_HorizontalSkew", skew * Mathf.Sign(otherRenderer.transform.lossyScale.x));
	}
}
