using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(HealthScript))]
public class _HealthScript : Editor {

	HealthScript script;

	void OnEnable() {
		script = target as HealthScript;
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (!script.isEnemy) {
			script.timePerDamage = EditorGUILayout.FloatField("Time Per Damage", script.timePerDamage);
		} else {
			script.health = EditorGUILayout.IntField("Health", script.health);
			script.maxHealth = EditorGUILayout.IntField("Health", script.maxHealth);
		}
	}

}
