using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SpritesToGameObjects : MonoBehaviour {

	public bool run = false;
	public string generatedName = "Sprite <index> of <count>";
	public int minIntLength = 0;
	public bool autoLeadingZeros = false;
	public RendererType type = RendererType.SpriteRenderer;

	public List<Sprite> sprites = new List<Sprite>();

	void OnValidate() {
		if (autoLeadingZeros) {
			minIntLength = sprites.Count > 0 ? Mathf.CeilToInt(Mathf.Log10(sprites.Count)) : 1;
		}
	}

	void Update() {
		Check();
	}

	string LeadingZeros(int value) {
		return value.ToString("D" + minIntLength);
	}

	void Check() {
		if (run) run = false;
		else return;

		for (int i=0; i<sprites.Count; i++) {
			Sprite sprite = sprites[i];

			string name = generatedName;
			name = name.Replace("<index>", LeadingZeros(i));
			name = name.Replace("<count>", LeadingZeros(sprites.Count));
			
			var obj = new GameObject(name);

			obj.transform.SetParent(transform);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			obj.transform.localRotation = Quaternion.identity;

			if (type == RendererType.SpriteRenderer) { 
				var ren = obj.AddComponent<SpriteRenderer>();
				ren.sprite = sprite;
			}
			if (type == RendererType.UIImage) {
				var ren = obj.AddComponent<Image>();
				ren.sprite = sprite;
			}
		}

		print("Successfully created " + sprites.Count + " gameobjects!");
	}

	public enum RendererType {
		SpriteRenderer, UIImage
	}

}
