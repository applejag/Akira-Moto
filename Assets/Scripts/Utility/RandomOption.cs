using UnityEngine;
using System.Collections;

public abstract class RandomObject<T> where T : UnityEngine.Object {
	[Range(0f, 1f)]
	public float weight = 1f;
	[Tooltip("Chance")]
	public float chance = 0f;
	public T target;
}

[System.Serializable]
public class RandomGameObject : RandomObject<GameObject> {}
[System.Serializable]
public class RandomSprite : RandomObject<Sprite> {}