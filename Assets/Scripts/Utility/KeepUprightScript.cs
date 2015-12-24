using UnityEngine;
using System.Collections;

public class KeepUprightScript : MonoBehaviour {
	void LateUpdate () {
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
	}
}
