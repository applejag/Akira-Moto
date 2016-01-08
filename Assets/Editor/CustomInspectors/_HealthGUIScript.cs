using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(HealthGUIScript))]
public class _HealthGUIScript : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		
		var script = target as HealthGUIScript;

		if (HealthGUIScript.instance == null) {
			if (GUILayout.Button("Initiate script!"))
				HealthGUIScript.instance = script;
		} else if (HealthGUIScript.instance != script) {
			EditorGUILayout.HelpBox("Another script is assigned as THE instance! Be aware!", MessageType.Warning);
		}
	}

}
