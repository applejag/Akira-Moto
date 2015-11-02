using UnityEngine;
using System.Collections;

public class MoverScript : MonoBehaviour {

    public Rigidbody2D rbody;

    [Space]

    public Vector2 speed = - Vector2.left * 10f;
	
    void Update() {

        rbody.velocity = speed;

    }

}
