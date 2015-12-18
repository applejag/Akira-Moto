using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	[Header("Object references")]

	public HealthScript health;
	public Animator anim;
	public Rigidbody2D rbody;
	public Transform damagePoint;
	public ParticleSystem mouthParticles;

	[Header("Movement")]

	public float topSpeed = 1;
	public float acceleration = 1;

	[Header("Ethernal state")]

	[SingleLayer]
	public int defaultLayer;
	[SingleLayer]
	public int ethernalLayer;

	private AnimState state = AnimState.idle;
	private bool ethernal;

	void Start() {
		HealthGUIScript.instance.UpdateUIElements(health.health, health.maxHealth);
	}
	
	void Update() {

		//-----------------------
		// Reading input
		//-----------------------

		// Check for input
		float movement = Input.GetAxis("Horizontal");
		bool attack = Input.GetButtonDown("Attack");
		bool ethernal = Input.GetButton("Ethernal");

		// Tell the animator
		if (attack && state != AnimState.ethernal) anim.SetTrigger("Attack");
		anim.SetBool("Ethernal", ethernal);
		anim.SetBool("Walking", movement != 0);
		anim.SetFloat("Movement", Mathf.Abs(movement));

		//-----------------------
		// Movement
		//-----------------------
		
		// Move with the input
		Vector2 motion = rbody.velocity;
		motion.x = Mathf.MoveTowards(motion.x, topSpeed * movement, acceleration * Time.deltaTime);

		// Apply the movement
		rbody.velocity = motion;

		// Turn around
		if (movement != 0)
			Turn(movement > 0);

	}

	void Turn(bool right) {
		int sign = right ? 1 : -1;

		Vector3 scale = transform.localScale;
		scale.x = Mathf.Abs(scale.x) * sign;
		transform.localScale = scale;
	}

	public void DealDamage() {
		// Spawn a damage object
		DamageScript.SpawnDamage(damagePoint.position, radius:1f, damage:1, isEnemy:false);
	}

	void EthernalEnter() {
		gameObject.layer = ethernalLayer;
	}

	void EthernalExit() {
		gameObject.layer = defaultLayer;
	}

	#region Animation events
	// Called by animations

	void MouthParticles() {
		mouthParticles.Play();
	}
	
	public void SetState(AnimState state) {
		if (this.state != AnimState.ethernal && state == AnimState.ethernal)
			EthernalEnter();
		if (this.state == AnimState.ethernal && state != AnimState.ethernal)
			EthernalExit();

		this.state = state;
	}
	#endregion

	public enum AnimState {
		idle, moving, attack, ethernal
	}

}
