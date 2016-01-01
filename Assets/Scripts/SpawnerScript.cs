using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

public class SpawnerScript : MonoBehaviour {

	public float spawnArea = 64f;
	public float cameraMargin = 0f;
	public Transform spawnInto;
	[Space]
	public float spawnDelay = 1f;
	public float timeLeft = 0f;
	public int spawnCap = 10;
	[HideInInspector]
	public List<RandomGameObject> objs = new List<RandomGameObject>();

	public static int enemiesAlive = 0;

	public Vector3 leftEdge { get { return new Vector3(transform.position.x - spawnArea, 0f); } }
	public Vector3 rightEdge { get { return new Vector3(transform.position.x + spawnArea, 0f); } }
	public Vector3 camMin { get { return new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x - cameraMargin, 0f); } }
	public Vector3 camMax { get { return new Vector3(Camera.main.ViewportToWorldPoint(Vector3.one).x + cameraMargin, 0f); } }

	// The spawning area is invalid if there is no place to spawn
	// That would happen if the camera covers the entire area
	public bool invalidArea { get { return (camMin.x <= leftEdge.x && camMax.x >= rightEdge.x); } }

#if UNITY_EDITOR
	void OnValidate() {
		spawnArea = Mathf.Max(spawnArea, 0f);
		cameraMargin = Mathf.Max(cameraMargin, 0f);

		objs.UpdateChance();
	}

	void OnDrawGizmos() {

		if (camMin.x > leftEdge.x && camMax.x < rightEdge.x) {
			// Camera boundary is inside spawn area

			Gizmos.color = Color.green;
			Gizmos.DrawLine(leftEdge, camMin);
			Gizmos.DrawLine(camMax, rightEdge);
			Gizmos.DrawRay(new Vector3(leftEdge.x, .5f), Vector3.down);
			Gizmos.DrawRay(new Vector3(rightEdge.x, .5f), Vector3.down);

			Gizmos.color = Color.red;
			Gizmos.DrawLine(camMin, camMax);
			Gizmos.DrawRay(new Vector3(camMin.x, .5f), Vector3.down);
			Gizmos.DrawRay(new Vector3(camMax.x, .5f), Vector3.down);

		} else {
			// Not whole camera inside spawn area

			if (camMin.x <= leftEdge.x && camMax.x >= rightEdge.x) {
				// Camera area covers entire spawn area
				Gizmos.color = Color.red;
				Gizmos.DrawLine(leftEdge, rightEdge);
				Gizmos.DrawRay(new Vector3(leftEdge.x, .5f), Vector3.down);
				Gizmos.DrawRay(new Vector3(rightEdge.x, .5f), Vector3.down);
			} else if (camMin.x > rightEdge.x || camMax.x < leftEdge.x) {
				// Camera area outside area
				Gizmos.color = Color.green;
				Gizmos.DrawLine(leftEdge, rightEdge);
				Gizmos.DrawRay(new Vector3(leftEdge.x, .5f), Vector3.down);
				Gizmos.DrawRay(new Vector3(rightEdge.x, .5f), Vector3.down);
			} else if(camMax.x <= rightEdge.x) {
				// Camera overlaps left side
				Gizmos.color = Color.red;
				Gizmos.DrawLine(leftEdge, camMax);
				Gizmos.DrawRay(new Vector3(leftEdge.x, .5f), Vector3.down);
				Gizmos.DrawRay(new Vector3(camMax.x, .5f), Vector3.down);

				Gizmos.color = Color.green;
				Gizmos.DrawLine(camMax, rightEdge);
				Gizmos.DrawRay(new Vector3(rightEdge.x, .5f), Vector3.down);
			} else if(camMin.x >= leftEdge.x) {
				// Camera overlaps right side
				Gizmos.color = Color.green;
				Gizmos.DrawLine(leftEdge, camMin);
				Gizmos.DrawRay(new Vector3(leftEdge.x, .5f), Vector3.down);

				Gizmos.color = Color.red;
				Gizmos.DrawLine(camMin, rightEdge);
				Gizmos.DrawRay(new Vector3(camMin.x, .5f), Vector3.down);
				Gizmos.DrawRay(new Vector3(rightEdge.x, .5f), Vector3.down);
			}
		}
		
	}
#endif

	void Update() {
		timeLeft -= Time.deltaTime;

		if (timeLeft <= 0f && enemiesAlive < spawnCap) {
			timeLeft += spawnDelay;
			SpawnRandom();
		} else if(enemiesAlive >= spawnCap) {
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
		} while (point.x >= camMin.x && point.x <= camMax.x);

		return point;
	}

	#region Invalid area exception
	public class InvalidAreaException : System.Exception {
		public static readonly string message = "There are no valid points in the spawning area!";

		public InvalidAreaException() : base(message) {}
		public InvalidAreaException(System.Exception inner) : base(message, inner) {}
	}
	#endregion

}
