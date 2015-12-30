using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[CustomPropertyDrawer(typeof(RandomGameObject))]
[CustomPropertyDrawer(typeof(RandomSprite))]
public class RandomOption_Transform : PropertyDrawer {

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		return base.GetPropertyHeight (property, label) * 2.5f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		
		int level = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Get properties
		var target = property.FindPropertyRelative("target");
		var weight = property.FindPropertyRelative("weight");
		var chance = property.FindPropertyRelative("chance");

		// Calc rects
		var targetRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		var weightLabelRect = new Rect(position.x - 50f, targetRect.y + targetRect.height, 50f, EditorGUIUtility.singleLineHeight);
		var chanceRect = new Rect(position.x + position.width - 50f, targetRect.y + targetRect.height, 50f, EditorGUIUtility.singleLineHeight);
		var weightSliderRect = new Rect(position.x, targetRect.y + targetRect.height, Mathf.Max(position.width-chanceRect.width,115), EditorGUIUtility.singleLineHeight);
		weightSliderRect.x = position.max.x - weightSliderRect.width - chanceRect.width;
		weightLabelRect.x = weightSliderRect.x - weightLabelRect.width;
		
		// Create labels
		var targetLabel = new GUIContent(target.displayName);
		var weightLabel = new GUIContent(weight.displayName);

		// Draw fields
		EditorGUI.BeginProperty(targetRect, targetLabel, target);
			EditorGUI.PropertyField(targetRect, target, GUIContent.none);
		EditorGUI.EndProperty();
		EditorGUI.BeginProperty(weightSliderRect, weightLabel, weight);
			EditorGUI.LabelField(weightLabelRect, weightLabel);
			weight.floatValue = EditorGUI.Slider(weightSliderRect, weight.floatValue, 0, 1);
		EditorGUI.EndProperty();

		EditorGUI.HelpBox(chanceRect, Mathf.Floor(chance.floatValue * 100f).ToString() + "%", MessageType.Info);
		
		
		EditorGUI.indentLevel = level;

		EditorGUI.EndProperty();
	}

}
