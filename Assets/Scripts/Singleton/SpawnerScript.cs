using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class SpawnerScript : SingletonBaseScript<SpawnerScript> { 

	public float spawnArea = 64f;
	public float cameraMargin = 0f;
	public float entityMargin = 0f;
	public Transform spawnInto;
	[Space]
	public float spawnDelay = 1f;
	public float timeLeft = 0f;
	public int spawnCap = 10;
	[HideInInspector]
	public List<RandomGameObject> objs = new List<RandomGameObject>();
	
	public int enemiesAlive = 0;

	private Vector3 leftEdge { get { return new Vector3(transform.position.x - spawnArea, 0f); } }
	private Vector3 rightEdge { get { return new Vector3(transform.position.x + spawnArea, 0f); } }
	private Vector3 camMin { get { return new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x - cameraMargin, 0f); } }
	private Vector3 camMax { get { return new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x + cameraMargin, 0f); } }

	// The spawning area is invalid if there is no place to spawn
	// That would happen if the camera covers the entire area
	public bool invalidArea { get { return (camMin.x <= leftEdge.x && camMax.x >= rightEdge.x); } }

#if UNITY_EDITOR
	void OnValidate() {
		spawnArea = Mathf.Max(spawnArea, 0f);
		cameraMargin = Mathf.Max(cameraMargin, 0f);
		entityMargin = Mathf.Max(entityMargin, 0f);

		objs.UpdateChance();
	}

	void OnDrawGizmos() {
		// Make list
		List<Obstruction> obs = new List<Obstruction>();
		obs.Add(new Obstruction(camMin.x, camMax.x));
		if (entityMargin > 0) {
			foreach (BaseAI enemy in FindObjectsOfType<BaseAI>()) {
				obs.Add(new Obstruction(enemy.transform.position.x - entityMargin, enemy.transform.position.x + entityMargin));
			}
		}

		// Compress
		bool change;
		do {
			change = obs.RemoveAll(delegate (Obstruction a) {
				// Outside area
				if (a.left > rightEdge.x || a.right < leftEdge.x)
					return true;
				for (int i = 0; i < obs.Count; i++) {
					Obstruction b = obs[i];
					if (a.right >= b.left && a.right < b.right) {
						// Combine
						b.left = Mathf.Min(a.left, b.left);
						b.right = Mathf.Max(a.right, b.right);
						return true;
					}
				}
				if (obs.Exists(delegate (Obstruction b) {
					return a.left == b.left && a.right == b.right && a != b && !b.dying;
				})) {
					a.dying = true;
					return true;
				}
				return false;
			}) > 0;
		} while (change);
		
		// Sort
        obs.Sort(delegate (Obstruction a, Obstruction b) {
			return a.left.CompareTo(b.left);
		});

		bool green = true;
		List<float> points = new List<float>();
		points.Add(leftEdge.x);
		foreach(var a in obs) {
			// Left
			float l = Mathf.Max(a.left, leftEdge.x);
			float r = Mathf.Min(a.right, rightEdge.x);

			if (Mathf.Approximately(l, leftEdge.x))
				green = false;
			else if (!points.Contains(l))
				points.Add(l);

			if (!points.Contains(r))
				points.Add(r);
		}
		if (points[points.Count - 1] < rightEdge.x)
			points.Add(rightEdge.x);

		// Draw
		float left = leftEdge.x;
		foreach (float point in points) {
			if (point == left) continue;

			Gizmos.color = left == leftEdge.x && green ? Color.green : Color.red;
			Gizmos.DrawRay(new Vector3(left, .5f), Vector3.down);
			Gizmos.color = green ? Color.green : Color.red;
			Gizmos.DrawLine(new Vector3(left, 0), new Vector3(point, 0));

			left = point;
			green = !green;
		}

		Gizmos.color = !green ? Color.green : Color.red;
		Gizmos.DrawRay(new Vector3(left, .5f), Vector3.down);
	}

	private class Obstruction {
		public float left;
		public float right;
		public bool dying = false;

		public Obstruction(float left, float right) {
			this.left = left;
			this.right = right;
		}
	}
#endif

	void Update() {
		timeLeft -= Time.deltaTime;

		if (timeLeft <= 0f && enemiesAlive < spawnCap && !GameOverScript.instance.over) {
			timeLeft += spawnDelay;
			SpawnRandom();
		}
		if (enemiesAlive >= spawnCap) {
			timeLeft = 0;
		}
	}

	void SpawnRandom() {
		var obj = objs.GetRandomObject();
		if (obj == null || obj.target == null) return;

		Instantiate(obj.target, RandomSpawnPoint(), obj.target.transform.rotation);
	}

	public Vector3 RandomSpawnPoint() {
		Vector3 point = Vector3.zero;

		if (invalidArea)
			throw new InvalidAreaException();
		
		// Repeat until valid spawning point
		do {
			point.x = Random.Range(leftEdge.x, rightEdge.x);
		} while (!ValidSpawnPoint(point.x));

		return point;
	}

	public bool ValidSpawnPoint(float x) {
		// Inside spawn area
		if (x < leftEdge.x || x > rightEdge.x)
			return false;

		// Inside camera space
		if (x >= camMin.x && x <= camMax.x)
			return false;

		// Collides with entity?
		if (entityMargin > 0) {
			foreach (BaseAI enemy in FindObjectsOfType<BaseAI>()) {
				float enemyX = enemy.transform.position.x;
				if (x > enemyX - entityMargin && x < enemyX + entityMargin)
					return false;
			}
		}

		// It's valid
		return true;
    }

	#region Invalid area exception
	public class InvalidAreaException : System.Exception {
		public static readonly string message = "There are no valid points in the spawning area!";

		public InvalidAreaException() : base(message) {}
		public InvalidAreaException(System.Exception inner) : base(message, inner) {}
	}
	#endregion

}
