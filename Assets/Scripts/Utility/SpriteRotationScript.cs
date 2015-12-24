using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteRotationScript : MonoBehaviour {
	
	public SpriteRenderer spriteRen;
	public List<Sprite> sprites = new List<Sprite>();

	void LateUpdate() {
		if (sprites.Count > 0 && spriteRen != null) {
			float angle = transform.eulerAngles.z % 360;
			int index = Mathf.FloorToInt((angle / 360) * sprites.Count);
			
			spriteRen.sprite = sprites[index] ?? spriteRen.sprite;
		}
	}

}
