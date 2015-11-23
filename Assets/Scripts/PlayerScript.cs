using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	[Header("Object references")]

	public HealthScript health;
	public Animator anim;
	public Rigidbody2D rbody;
	public Transform damagePoint;

	[Header("Settings")]

	public float topSpeed = 1;
	public float acceleration = 1;

	private AnimState state = AnimState.idle;

	void Start() {
		HealthGUIScript.instance.UpdateUIElements(health);
	}

	void Update() {
		Movement();
		Attack();
    }

	void Movement() {
		if (state == AnimState.attack)
			return;

		// Motion vector
		Vector2 motion = rbody.velocity;

		// Move with the input
		float input = Input.GetAxis("Horizontal");
		motion.x = Mathf.MoveTowards(motion.x, topSpeed * input, acceleration * Time.deltaTime);

		// Apply the movement
		rbody.velocity = motion;

		// Turn around
		if (input != 0) {
			Turn(input > 0);
			state = AnimState.moving;
		} else {
			state = AnimState.idle;
		}
	}

	void Attack() {
		if (state == AnimState.attack)
			return;

		// Check for input
		bool attack = Input.GetButtonDown("Attack");

		// Tell the animation controller
		if (attack) {
			anim.SetTrigger("Attack");
			state = AnimState.attack;
		}
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

	// Called by animations when they are done
	public void SetState(AnimState state) {
		this.state = state;
	}

	public enum AnimState {
		idle, moving, attack
	}

}
