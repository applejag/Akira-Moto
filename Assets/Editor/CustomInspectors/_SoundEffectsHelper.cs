using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

[CustomEditor(typeof(SoundEffectsHelper))]
public class _SoundEffectsHelper : Editor {

	private ReorderableList dirt;

	private void OnEnable() {
		dirt = InitiateList<AudioClip>(serializedObject, "dirt", "clip", "Dirt sound effects", 2.2f);
		
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		serializedObject.Update();
	
		dirt.DoLayoutList();

		serializedObject.ApplyModifiedProperties();
	}

	ReorderableList InitiateList<Select>(SerializedObject serializedObject, string propertyPath, string subObjPath, string title, float elementHeightMultiplier = 1f) where Select: Object {
		var list = new ReorderableList(serializedObject, serializedObject.FindProperty("dirt"), false, true, true, true);

		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;

			EditorGUI.PropertyField(rect, element);
		};
		list.onSelectCallback = (ReorderableList l) => {
			var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative(subObjPath);
			if (prefab != null && prefab.propertyType == SerializedPropertyType.ObjectReference)
				EditorGUIUtility.PingObject(prefab.objectReferenceValue as Select);
		};
		list.elementHeight = elementHeightMultiplier * EditorGUIUtility.singleLineHeight;
		list.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField(rect, title);
		};

		return list;
	}

}
