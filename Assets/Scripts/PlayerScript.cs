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

    void Update() {
        // Movement
		Vector2 axis = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));

		Vector2 movement = Vector2.Scale (axis, speed);
        rbody.velocity = movement;

        // Shotting
        bool shoot = Input.GetButtonDown("Fire1");
        shoot |= Input.GetButtonDown("Fire2");

        if (shoot) {
            weapon.Attack(false);
        }

		// Scrolling
		rbody.velocity += Vector2.right * scrollingSpeed;
		//Camera.main.transform.Translate (Vector3.right * scrollingSpeed * Time.deltaTime);
		cam.rbody.velocity = Vector2.right * scrollingSpeed;
    }

}
