using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class BottleProjectileScript : MonoBehaviour {

	public float force = 1f;
	public float angle;
	public Rigidbody2D rbody;
	public Transform[] saveOnDeath;
	[Space]
	public ParticleSystem playerCollision;
	public ParticleSystem groundCollision;

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {

		if (Application.isPlaying)
			return;

		// Draw trajectory
		float timePerSegment = Time.fixedDeltaTime; // Defalt: 1/50th second
		float simulateTime = 1f / timePerSegment; // simulate in 1 second

		Vector3 gravity = Physics2D.gravity.ToVector3() * rbody.gravityScale;
		var last = transform.position;
		var pos = transform.position;
		var vel = VectorHelper.FromDegrees(angle).ToVector3() * force / rbody.mass;

		vel.Scale(transform.localScale);
		
		for (;simulateTime > 0; simulateTime--) {

			pos += vel * timePerSegment;
			vel += gravity * timePerSegment;

			Gizmos.color = Color.Lerp(Color.red, Color.green, simulateTime / 50f);
			Gizmos.DrawLine(last, pos);

			last = pos;
		}
	}
#endif

	void Start () {
		rbody.AddForce(Vector2.Scale(transform.localScale.ToVector2(), VectorHelper.FromDegrees(angle) * force), ForceMode2D.Impulse);

		// Just in case
		Destroy(gameObject, 10f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Ground") {
			// Collided with ground
			groundCollision.Play();
			Die();
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.rigidbody != null && other.rigidbody.GetComponent<PlayerScript>() != null) {
			// Collided with player
			print("player");
			playerCollision.Play();
			Die();
		}

	}

	void Die() {

		// Dont destroy the particle systems imidietly
		foreach(var obj in saveOnDeath) {
			obj.transform.parent = transform.parent;

			var sys = obj.GetComponent<ParticleSystem>();
			var trail = obj.GetComponent<TrailRenderer>();

			if (sys != null) {
				sys.Play();

				// Destroy it after delay
				Destroy(obj.gameObject, sys.startLifetime);
			} else if (trail != null) {
				// Destroy it after delay
				Destroy(obj.gameObject, trail.time);
			}
		}

		Destroy(gameObject);

	}

}
