using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public Rigidbody2D rbody;

    [Space]

    public Vector2 speed = Vector2.one * 50f;

    void Update() {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(xAxis * speed.x, yAxis * speed.y);
        //movement *= Time.deltaTime;

        rbody.velocity = movement;
    }

}
