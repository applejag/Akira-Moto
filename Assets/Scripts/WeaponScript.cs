using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class WeaponScript : MonoBehaviour {

	public Transform shotPrefab;
	public float shootingRate = 0.25f;
    public float angleOverride = 0f;
	
	private float shootCooldown;
	
	void Start() {
		shootCooldown = 0f;
	}
	
	void Update() {
		if (shootCooldown > 0) {
			shootCooldown -= Time.deltaTime;
		}
	}

#if UNITY_EDITOR

    void OnDrawGizmosSelected() {
		Vector3 point = transform.position + VectorHelper.FromDegrees (angleOverride).ToVector3 ();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, point);
        Gizmos.DrawWireSphere(point, 0.125f);
    }

#endif

    public void Attack(bool isEnemy) {
		if (canAttack) {
			shootCooldown = shootingRate;
			
			// Create a new shot
			var shotTransform = Instantiate(shotPrefab) as Transform;
			
			// Assign position
			shotTransform.position = transform.position;
			
			// The is enemy property
			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
			if (shot != null) {
				shot.isEnemyShot = isEnemy;
			}
			
            
			// Make the weapon shot always towards it
			MoverScript move = shotTransform.gameObject.GetComponent<MoverScript>();
			if (move != null) {
                move.angle = angleOverride;
			}
            
		}
	}
	
	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool canAttack {
		get	{
			return shootCooldown <= 0f;
		}
	}
}