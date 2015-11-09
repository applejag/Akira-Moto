using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[CustomPropertyDrawer(typeof(CameraDropDown))]
public class _CameraDropDown : PropertyDrawer {

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		
		CameraDropDown dropDown = attribute as CameraDropDown;

		if (property.propertyType == SerializedPropertyType.ObjectReference && !dropDown.hideBool) {
			return base.GetPropertyHeight(property,label) * 2f;
		}

		return base.GetPropertyHeight (property, label);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		EditorGUI.BeginProperty (position, label, property);


		CameraDropDown dropDown = attribute as CameraDropDown;

		if (property.propertyType == SerializedPropertyType.ObjectReference) {
			if (dropDown.hideBool)  {
				// Draw

				Camera cam = DrawCameraPopup(position,label,property.objectReferenceValue as Camera, dropDown.fullPath);
				property.objectReferenceValue = cam;
				Debug.Log(cam + " --- " + property.objectReferenceValue);
				
			} else {
				// Calc rects
				Rect popupRect = new Rect(position.x,
				                          position.y, 
				                          position.width, 
				                          EditorGUIUtility.singleLineHeight);
				Rect boolRect = new Rect(position.x + EditorGUIUtility.labelWidth,
				                         position.y + popupRect.height,
				                         position.width - EditorGUIUtility.labelWidth, 
				                         EditorGUIUtility.singleLineHeight);

				// Draw
				property.objectReferenceValue = DrawCameraPopup(popupRect,label,property.objectReferenceValue as Camera, dropDown.fullPath);
				dropDown.fullPath = EditorGUI.Toggle(boolRect, "Show full path", dropDown.fullPath);
			}
		} else {
			EditorGUI.LabelField(position,label.text, "Use CameraDropDown with object references.");
		}
		
		EditorGUI.EndProperty();
		
	}

	Camera DrawCameraPopup(Rect position, GUIContent label, Camera currentCamera, bool fullPath) {
		Camera[] cameras = Camera.allCameras;
		string[] cameraNames = new string[cameras.Length+1];
		int currentIndex = 0;
		
		// Fill names array
		for (int i=0; i<cameras.Length; i++) {
			cameraNames[i+1] = (fullPath ? cameras[i].transform.GetPath() : cameras[i].name) + (Camera.main == cameras[i] ? " [main]" : "");
			
			if (cameras[i].Equals(currentCamera))
				currentIndex = i;
		}

		cameraNames [0] = "-";
		
		// Draw
		int index = EditorGUI.Popup(position,label.text,currentIndex,cameraNames);

		if (index == 0)
			return null;

		// Get layer from name via index
		return cameras[index-1];
	}

}
