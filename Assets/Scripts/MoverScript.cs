using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class MoverScript : MonoBehaviour {

    public Rigidbody2D rbody;

    [Header("Settings")]

    public float speed = 10f;
    public float angle = 0f;

#if UNITY_EDITOR

    void OnDrawGizmosSelected() {
		Vector3 point = transform.position + VectorHelper.FromDegrees (angle).ToVector3 ();

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, point);
        Gizmos.DrawWireSphere(point, 0.125f);
    }

#endif

    void Update() {

		Vector2 vec = VectorHelper.FromDegrees (angle);

        rbody.velocity = vec * speed;

    }

}
