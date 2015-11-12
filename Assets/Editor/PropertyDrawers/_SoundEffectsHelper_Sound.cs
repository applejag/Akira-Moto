using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[CustomPropertyDrawer(typeof(SoundEffectsHelper.Sound))]
public class _SoundEffectsHelper_Sound : PropertyDrawer {

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		return base.GetPropertyHeight (property, label) * 2f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Get properties
		var clipProp = property.FindPropertyRelative("clip");
		var volumeProp = property.FindPropertyRelative("volume");
		var vLabelContent = new GUIContent("Volume");

		// Calc rects
		var clipRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

		var volumeRect = new Rect(position.x, position.y + clipRect.height, position.width, EditorGUIUtility.singleLineHeight);
		var vLabelRect = new Rect(volumeRect.x, volumeRect.y, 50, volumeRect.height);
		var vSliderRect = new Rect(volumeRect.x + vLabelRect.width, volumeRect.y, volumeRect.width - vLabelRect.width, volumeRect.height);

		// Create labels
		var clipLabel = new GUIContent("clip");
		var volumeLabel = new GUIContent("volume");

		// Draw fields
		EditorGUI.BeginProperty(clipRect, clipLabel, clipProp);
			EditorGUI.PropertyField(clipRect, clipProp, GUIContent.none);
		EditorGUI.EndProperty();
		EditorGUI.BeginProperty(volumeRect, volumeLabel, volumeProp);
			EditorGUI.LabelField(vLabelRect, vLabelContent);
			EditorGUI.PropertyField(vSliderRect, volumeProp, GUIContent.none);
		EditorGUI.EndProperty();
		
		EditorGUI.EndProperty();
	}

}
