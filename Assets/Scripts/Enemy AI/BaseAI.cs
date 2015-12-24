using UnityEngine;
using System.Collections;

public abstract class BaseAI : MonoBehaviour {

	[Header("BaseAI")]
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
	public bool playerInRange { get { return player != null && playerDistance <= attackRange && !player.isEthereal; } }
	
	protected virtual void Start() {
		target = FindObjectOfType<PlayerScript>();
	}

	protected virtual void Update() {
		if (attackDelay > 0)
			attackDelay = Mathf.Max(attackDelay - Time.deltaTime, 0f);
	}

	// Walk close enough to player
	public bool WalkIntoAttackRange() {
		if (player == null)
			return false;

		TurnTowardsPlayer();
		if (playerDistance > attackRange || player.isEthereal)
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
		int sign = (int)Mathf.Sign(delta);

		Vector3 scale = transform.localScale;
		scale.x = Mathf.Abs(scale.x) * (sign != 0 ? sign : 1);
		transform.localScale = scale;
	}

	public void TurnTowardsPlayer() {
		if (player == null)
			return;

		TurnTowards(player.transform.position.x);
	}

	protected virtual void StateChange(State last) {
		if (last == State.attacking)
			canAttack = false;
	}

	public enum State {
		idle, moving, attacking
	}

}
