using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(ScoreKeeperScript))]
public class _ScoreKeeperScript : Editor {

	ScoreKeeperScript script;

	void OnEnable() {
		script = target as ScoreKeeperScript;
	}
	
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		GUI.enabled = false;
		EditorGUILayout.FloatField("Score", script.score);
		GUI.enabled = true;
	}

}
