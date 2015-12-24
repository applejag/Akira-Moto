using UnityEngine;
using System.Collections;

public class RotatorScript : MonoBehaviour {

	public Vector3 eulerMotion;
	public Space relativeTo;
	public MoveVia moveVia;

	public Rigidbody rbody;
	public Rigidbody2D rbody2D;

	void Update() {
		switch (moveVia) {
			case MoveVia.transform:
				if (relativeTo == Space.Self)
					transform.localEulerAngles += eulerMotion * Time.deltaTime;
				if (relativeTo == Space.World)
					transform.eulerAngles += eulerMotion * Time.deltaTime;
				break;

			case MoveVia.rigidbody:

				break;

			case MoveVia.rigidbody2D:

				break;
		}
	}

	public enum MoveVia {
		transform, rigidbody, rigidbody2D
	}
}
