using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public Rigidbody2D rbody;
    public WeaponScript weapon;

    [Space]

    public Vector2 speed = Vector2.one * 50f;

    void Update() {
        // Movement
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(xAxis * speed.x, yAxis * speed.y);
        rbody.velocity = movement;

        // Shotting
        bool shoot = Input.GetButtonDown("Fire1");
        shoot |= Input.GetButtonDown("Fire2");

        if (shoot) {
            weapon.Attack(false);
        }
    }

}
