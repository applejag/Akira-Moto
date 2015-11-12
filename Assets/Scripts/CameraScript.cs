using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class CameraScript : MonoBehaviour {
	public Rigidbody2D rbody;
	[Header("Settings")]
	public float collidersThickness = 4f;
	[SingleLayer]
	public int collidersLayer;

	void Awake() {
		SetupAllColliders ();
	}

	private void SetupAllColliders() {
		Bounds bounds = Camera.main.OrthographicBounds ();
        bounds.center = Vector3.zero;

		// Calculate bounds
		Bounds top = new Bounds ();
		top.SetMinMax (bounds.min + Vector3.up * bounds.size.y, bounds.max + Vector3.up * collidersThickness);

		Bounds left = new Bounds ();
		left.SetMinMax (bounds.min + Vector3.left * collidersThickness, bounds.min + Vector3.up * bounds.size.y);

		Bounds right = new Bounds ();
		right.SetMinMax (bounds.min + Vector3.right * bounds.size.x, bounds.max + Vector3.right * collidersThickness);

		Bounds bottom = new Bounds ();
		bottom.SetMinMax (bounds.min + Vector3.down * collidersThickness, bounds.min + Vector3.right * bounds.size.x);

		// Top
		SetupCollider ("Collider-top", top);
		SetupCollider ("Collider-left", left);
		SetupCollider ("Collider-right", right);
		SetupCollider ("Collider-bottom", bottom);
	}

	private void SetupCollider (string name, Bounds bounds) {
		GameObject obj = new GameObject (name);

		// Set the layer
		obj.layer = collidersLayer;

		// Make it child of this object (in the hiararchy)
		obj.transform.SetParent (transform);

		// Reset transform
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;

		// Add collider
		BoxCollider2D box = obj.AddComponent<BoxCollider2D> ();
		box.offset = bounds.center;
		box.size = bounds.size;
	}
}

