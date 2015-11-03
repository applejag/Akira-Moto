using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class PlayerScript : MonoBehaviour {

    public Rigidbody2D rbody;
    public WeaponScript weapon;
	public CameraScript cam;
    [Header("Settings")]
    public Vector2 speed = Vector2.one * 50f;
	public float scrollingSpeed = 1f;

	private bool dead = false;

    void Update() {
        // Movement
		Vector2 axis = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));

		Vector2 movement = Vector2.Scale (axis, speed);
        rbody.velocity = movement;

        // Shotting
        bool shoot = Input.GetButtonDown("Fire1");
        shoot |= Input.GetButtonDown("Fire2");

        if (shoot) {
            if (weapon.Attack(false))
				SoundEffectsHelper.PlayPlayerShotSound();
        }

		// Scrolling
		rbody.velocity += Vector2.right * scrollingSpeed;
		//Camera.main.transform.Translate (Vector3.right * scrollingSpeed * Time.deltaTime);
		cam.rbody.velocity = Vector2.right * scrollingSpeed;
    }

	void OnApplicationQuit() {
		dead = true;
	}

	// Gets destroyed by the HealthScript, so basically this = OnDeath
	void OnDestroy() {
		if (!dead) {
			GameOverScript.GameOver ();
		}
	}

}
