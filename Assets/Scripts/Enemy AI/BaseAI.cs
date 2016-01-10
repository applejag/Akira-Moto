using UnityEngine;
using System.Collections;
using ExtensionMethods;

public abstract class BaseAI : MonoBehaviour {

	protected abstract bool turnWithAnimation { get; }

	[Header("BaseAI")]
	public Animator anim;
	public Rigidbody2D body;
	public HealthScript health;
	public float speed = 1f; // units per second
	public float attackRange = 1f;
	public float attackCooldown = 1f; // delay in seconds
	public float attackVariety = 0.3f; // ± seconds

	private float attackDelay = 0f;
	public bool canAttack {
		get { return attackDelay <= 0f && state != State.attacking; }
		set { attackDelay = value ? attackCooldown + Random.Range(-attackVariety,attackVariety) : 0f; }
	}

	private State currentState = State.idle;
	// GETS SET BY THE ANIMATION BEHAVIOUR
	public State state {
		get { return currentState; }
		set {
			// Run the NewState method when the value is changed
			if (value != state) {
				var last = state;
				currentState = value;
				StateChange(last);
			}
		}
	}

	private PlayerScript target;
	public PlayerScript player { get { return target; } }
	public float playerDelta { get {
			if (player == null) return Mathf.Infinity;
			return player.transform.position.x - transform.position.x;
	} }
	public float playerDistance { get { return Mathf.Abs(playerDelta); } }
	public int playerSign { get { return player != null ? (int)Mathf.Sign(playerDelta) : 0; } }
	public bool playerInRange { get {
			return player != null
				&& playerDistance <= attackRange // in range
				&& !player.isWarping // player isnt warping
				&& currentSign == playerSign; // facing player
		} }

	public int currentSign { get { return (int)Mathf.Sign(transform.localScale.x); } }

	private int turnSign;

	protected virtual void Start() {
		target = FindObjectOfType<PlayerScript>();
	}

	protected virtual void Update() {
		if (attackDelay > 0)
			attackDelay = Mathf.Max(attackDelay - Time.deltaTime, 0f);
	}

	public virtual void FixedUpdate() {
		if (state == State.dead)
			return;
		
		bool inRange = WalkIntoAttackRange();

		anim.SetBool("Attack", inRange && state != State.turning);
		anim.SetBool("Walking", Mathf.Abs(body.velocity.x) > 0.5f);
		anim.SetFloat("Movement", Mathf.Abs(body.velocity.x));
	}

	public virtual void OnDeath() {
		anim.SetBool("Dead", true);
	}

	// Walk close enough to player
	public bool WalkIntoAttackRange() {
		if (player == null)
			return false;

		TurnTowardsPlayer();
		if (playerDistance > attackRange || player.isWarping)
			WalkTowardsPlayer();

		return playerInRange;
	}

	public void WalkTowards(float x) {
		float delta = x - transform.position.x;
		float dist = Mathf.Abs(delta);
		
		if (dist < 1f) // Close enough
			return;

		body.AddForce(new Vector2(Mathf.Sign(delta) * speed, 0f), ForceMode2D.Force);
	}

	public void WalkTowardsPlayer() {
		if (player == null)
			return;

		WalkTowards(player.transform.position.x);
	}

	public void TurnTowards(float x) {
		float delta = x - transform.position.x;
		turnSign = (int)Mathf.Sign(delta);

		if (currentSign != turnSign && state != State.turning) {
			// TURN

			if (turnWithAnimation) {
				// Play animation
				anim.SetTrigger("Turn");
			} else if(state == State.idle || state == State.moving) {
				// Turn instantly
				TurnComplete();
			}
		}
	}

	public void TurnTowardsPlayer() {
		if (player == null)
			return;

		TurnTowards(player.transform.position.x);
	}
	
	public void TurnComplete() {
		if (turnSign == 0)
			return;

		Vector3 scale = transform.localScale;
		scale.x = Mathf.Abs(scale.x) * turnSign;
		transform.localScale = scale;
	}

	protected virtual void StateChange(State last) {
		if (last == State.attacking)
			canAttack = false;

		if (last == State.turning)
			TurnComplete();

		if (state == State.dead) {
			turnSign = playerSign;
			TurnComplete();
			
			// Disable the colliders
			foreach (var collider in GetComponentsInChildren<Collider2D>()) {
				collider.enabled = false;
			}
		}
	}

	public enum State {
		idle, moving, attacking, turning, dead
	}

}
