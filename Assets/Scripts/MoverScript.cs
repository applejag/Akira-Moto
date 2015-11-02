using UnityEngine;
using System.Collections;

public class MoverScript : MonoBehaviour {

    public Rigidbody2D rbody;

    [Space]

    public float speed = 10f;
    public float angle = 0f;

#if UNITY_EDITOR

    void OnDrawGizmosSelected() {
        float a = angle * Mathf.Deg2Rad;
        Vector3 point = transform.position + new Vector3(Mathf.Cos(a), Mathf.Sin(a));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, point);
        Gizmos.DrawWireSphere(point, 0.125f);
    }

#endif

    void Update() {

        float a = angle * Mathf.Deg2Rad;
        Vector2 vec = new Vector2(Mathf.Cos(a), Mathf.Sin(a));

        rbody.velocity = vec * speed;

    }

}
