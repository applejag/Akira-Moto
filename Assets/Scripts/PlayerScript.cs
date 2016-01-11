using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class PlayerScript : MonoBehaviour {

	[Header("Object references")]

	public HealthScript health;
	public Animator anim;
	public Rigidbody2D body;
	public Transform damagePoint;
	public ParticleSystem mouthParticles;

	[Header("Attacking")]

	public float damageRadius = 1f;

	[Header("Movement")]

	public float speed = 1;

	[Header("Ethernal state")]

	[SingleLayer]
	public int defaultLayer;
	[SingleLayer]
	public int warpLayer;
	public Transform warpExitPos;
	public float warpDistance = 3f;
	public float colliderRadius = 0f;
	public LayerMask rayLayerMask;
	
	private AnimState state = AnimState.idle;

	public bool isWarping { get { return state == AnimState.warping; } }
	public bool isDead { get { return state == AnimState.dead; } }

#if UNITY_EDITOR
	void OnDrawGizmos() {
		Transform selected = UnityEditor.Selection.activeTransform;
		if (selected == transform || (selected == damagePoint && damagePoint != null)) {
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.CircleCap(0, damagePoint.position, Quaternion.identity, damageRadius);
		}
	}
#endif

	void FixedUpdate() {
		if (state == AnimState.dead || GameOverScript.instance.over)
			return;


		// Check for input
		float movement = Input.GetAxis("Horizontal");

		// Tell the animator
		anim.SetBool("Walking", movement != 0);
		anim.SetFloat("Movement", Mathf.Abs(movement));

		//-----------------------
		// Movement
		//-----------------------

		// Can't move while attacking nor warping
		if (state == AnimState.attack || state == AnimState.warping)
			return;

		// Move with the input
		body.AddForce(new Vector2(movement * speed, 0f));

		// Turn around
		if (movement != 0)
			Turn(movement > 0);
	}

	void Update() {
		if (state == AnimState.dead || GameOverScript.instance.over)
			return;

		//-----------------------
		// Reading input
		//-----------------------

		// Check for input
		bool attack = Input.GetButtonDown("Attack");
		bool warp = Input.GetButton("Warp");

		// Tell the animator
		if (attack && state != AnimState.warping && state != AnimState.turning && state != AnimState.attack) anim.SetTrigger("Attack");
		anim.SetBool("Warp", warp);
	}

	void OnDeath() {
		anim.SetBool("Dead", true);
		state = AnimState.dead;
		GameOverScript.instance.over = true;

		// Disable the emitters
		foreach(var part in GetComponentsInChildren<ParticleSystem>()) {
			var em = part.emission;
			em.enabled = false;
		}

		// Disable the colliders
		foreach (var collider in GetComponentsInChildren<Collider2D>()) {
			collider.enabled = false;
		}
	}
	
	void Turn(bool right) {
		int sign = right ? 1 : -1;

		Vector3 scale = transform.localScale;
		scale.x = Mathf.Abs(scale.x) * sign;
		transform.localScale = scale;
	}

	#region Animation events
	// Called by animations

	public void DealDamage() {
		// Spawn a damage object
		DamageScript.SpawnDamage(damagePoint.position, damageRadius, damage:1, source:health);
	}

	void EthernalEnter() {
		// Calc distance
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0f), warpDistance, rayLayerMask);
		if (hit.collider) {
			float sign = Mathf.Sign(transform.position.x - hit.point.x);
			float x = hit.point.x + colliderRadius * sign;
            warpExitPos.transform.position = new Vector3(x, warpExitPos.transform.position.y, warpExitPos.transform.position.z);
		} else
			warpExitPos.transform.position = transform.position + new Vector3(transform.localScale.x * warpDistance, 0f);
		
		gameObject.layer = warpLayer;
	}

	void EthernalExit() {
		gameObject.layer = defaultLayer;

		// Move playah
		transform.position = warpExitPos.position;
	}

	void MouthParticles() {
		mouthParticles.Play();
	}
	
	public void SetState(AnimState state) {
		if (this.state != AnimState.warping && state == AnimState.warping)
			EthernalEnter();
		if (this.state == AnimState.warping && state != AnimState.warping)
			EthernalExit();

		this.state = state;
	}

	public void PlayAttackSound() {
		SoundEffectsHelper.instance.PlaySwooshSound(damagePoint.position);
	}

	public void PlayStepSound() {
		SoundEffectsHelper.instance.PlayDirtSound(transform.position);
	}
	#endregion

	public enum AnimState {
		idle, moving, attack, warping, turning, dead
	}

}
