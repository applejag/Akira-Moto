using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(HealthScript))]
public class _HealthScript : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		var script = target as HealthScript;

		if (HealthGUIScript.instance != null) {
			GUI.enabled = !script.updatesUI;
			if (GUILayout.Button("Updates UI")) {
				HealthGUIScript.instance.health = script;
			}
			GUI.enabled = true;
		} else {
			EditorGUILayout.HelpBox("HealthGUIScript has no instance setup!\nGo and initate it if you wish to change which healthscript that updates the UI", MessageType.Warning);
		}
	}

}
