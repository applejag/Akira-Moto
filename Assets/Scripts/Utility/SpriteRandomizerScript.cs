using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRandomizerScript : MonoBehaviour {

	public bool playOnAwake = true;

	[HideInInspector]
	public List<RandomSprite> sprites = new List<RandomSprite>();

	private SpriteRenderer ren;

#if UNITY_EDITOR
	void OnValidate() {
		sprites.UpdateChance();
	}
#endif

	void Awake() {
		ren = GetComponent<SpriteRenderer>();
		if (playOnAwake)
			Randomize();
	}

	public void Randomize() {
		var sprite = sprites.GetRandomObject();
		if (sprite == null || sprite.target == null)
			ren.sprite = null;
		else
			ren.sprite = sprite.target;
	}

}
