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
	public int etherealLayer;
	public Transform etherealExitPos;
	public float etherealDistance = 3f;
	public LayerMask rayLayerMask;

	private AnimState state = AnimState.idle;
	private bool ethernal;

	public bool isEthereal { get { return state == AnimState.ethereal; } }

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
		// Check for input
		float movement = Input.GetAxis("Horizontal");

		// Tell the animator
		anim.SetBool("Walking", movement != 0);
		anim.SetFloat("Movement", Mathf.Abs(movement));

		//-----------------------
		// Movement
		//-----------------------

		// Can't move while attacking nor warping
		if (state == AnimState.attack || state == AnimState.ethereal)
			return;

		// Move with the input
		body.AddForce(new Vector2(movement * speed, 0f));

		// Turn around
		if (movement != 0)
			Turn(movement > 0);
	}

	void Update() { 

		//-----------------------
		// Reading input
		//-----------------------

		// Check for input
		bool attack = Input.GetButtonDown("Attack");
		bool ethereal = Input.GetButton("Ethereal");

		// Tell the animator
		if (attack && state != AnimState.ethereal) anim.SetTrigger("Attack");
		anim.SetBool("Ethereal", ethereal);

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
		RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0f), etherealDistance, rayLayerMask);
		if (hit.collider)
			etherealExitPos.transform.position = new Vector3(hit.point.x, etherealExitPos.transform.position.y, etherealExitPos.transform.position.z);
		else
			etherealExitPos.transform.position = transform.position + new Vector3(transform.localScale.x * etherealDistance, 0f);
		
		gameObject.layer = etherealLayer;
	}

	void EthernalExit() {
		gameObject.layer = defaultLayer;

		// Move playah
		transform.position = etherealExitPos.position;
	}

	void MouthParticles() {
		mouthParticles.Play();
	}
	
	public void SetState(AnimState state) {
		if (this.state != AnimState.ethereal && state == AnimState.ethereal)
			EthernalEnter();
		if (this.state == AnimState.ethereal && state != AnimState.ethereal)
			EthernalExit();

		this.state = state;
	}
	#endregion

	public enum AnimState {
		idle, moving, attack, ethereal
	}

}
