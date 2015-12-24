using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(RotatorScript))]
public class _RotatorScript : Editor {

	public override void OnInspectorGUI() {
		var script = target as RotatorScript;
		
		script.eulerMotion = EditorGUILayout.Vector3Field("Degrees per second", script.eulerMotion);
		script.moveVia = (RotatorScript.MoveVia)EditorGUILayout.EnumPopup("Move using", script.moveVia);

		EditorGUI.indentLevel++;
		switch (script.moveVia) {
			case RotatorScript.MoveVia.transform:
				script.relativeTo = (Space)EditorGUILayout.EnumPopup("Relative to", script.relativeTo);
				script.rbody = null; script.rbody2D = null;
				break;

			case RotatorScript.MoveVia.rigidbody:
				script.rbody = EditorGUILayout.ObjectField("Rigidbody", script.rbody, typeof(Rigidbody), true) as Rigidbody;
				script.relativeTo = 0; script.rbody2D = null;
				break;

			case RotatorScript.MoveVia.rigidbody2D:
				script.rbody2D = EditorGUILayout.ObjectField("Rigidbody 2D", script.rbody2D, typeof(Rigidbody2D), true) as Rigidbody2D;
				script.relativeTo = 0; script.rbody = null;
				break;
		}
		EditorGUI.indentLevel--;
	}

}
