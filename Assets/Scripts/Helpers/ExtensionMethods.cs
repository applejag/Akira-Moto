using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ExtensionMethods {
	public static class VectorExtension {
		public static Vector3 ToVector3(this Vector2 vec) {
			// Z = 0
			return new Vector3 (vec.x, vec.y);
		}
		
		public static Vector2 ToVector2(this Vector3 vec) {
			// Z gets lost
			return new Vector2 (vec.x, vec.y);
		}
	}

	public static class RendererExtensions {
		public static bool IsVisableFrom(this Renderer renderer, Camera camera) {
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes (camera);
			return GeometryUtility.TestPlanesAABB (planes, renderer.bounds);
		}
	}

	public static class CameraExtensions {
		public static Bounds OrthographicBounds (this Camera cam) {
			return cam.OrthographicBounds (cam.nearClipPlane);
		}

		public static Bounds OrthographicBounds (this Camera cam, float dist) {
			
			float height = cam.orthographicSize * 2f;
			float width = height * cam.aspect;
			
			Vector3 center = cam.transform.position + cam.transform.forward * dist;
			Vector3 size = new Vector3 (width, height);
			
			return new Bounds(center, size);
		}
	}

	public static class TransformExitensions {
		// Recursive
		public static string GetPath(this Transform current) {
			if (current.parent == null) 
				return current.name;

			return current.parent.GetPath () + "/" + current.name;
		}
	}

	public static class RandomOptionExtension {

		#region Mane functions
		public static float GetSpawningChance<T, S>(List<T> list, T obj) where T : RandomOption<S> where S : Object {
			float chance = 0f;

			foreach (var o in list) {
				chance += o.weight;
			}

			return chance != 0f ? obj.weight / chance : 0f;
		}

		public static void UpdateChance<T, S>(List<T> list) where T : RandomOption<S> where S : Object {
			foreach (var obj in list) {
				obj.chance = GetSpawningChance<T, S>(list, obj);
			}
		}

		public static T GetRandomObject<T, S>(List<T> list) where T : RandomOption<S> where S : Object {
			float coin = Random.value;
			float chance = 0f;

			foreach (var obj in list) {
				chance += obj.chance;

				// where Equals(0.99999999f, 1.0f) gives false
				// Approx(0.99999999f, 1.0f) gives true
				// and while adding there's a big chance I'll end up with a similar scenario
				if (chance > coin || Mathf.Approximately(chance, coin))
					return obj;
			}
			// Noone. SHOULD only happen if the list is empty
			// Or if all got weight=0
			return null;
		}
		#endregion

		#region Wrappers
		public static void UpdateChance(this List<RandomGameObject> list) {
			UpdateChance<RandomGameObject, GameObject>(list);
		}
		public static void UpdateChance(this List<RandomSprite> list) {
			UpdateChance<RandomSprite, Sprite>(list);
		}

		public static float GetSpawningChance(this List<RandomGameObject> list, RandomGameObject obj) {
			return GetSpawningChance<RandomGameObject, GameObject>(list, obj);
		}
		public static float GetSpawningChance(this List<RandomSprite> list, RandomSprite obj) {
			return GetSpawningChance<RandomSprite, Sprite>(list, obj);
		}

		public static RandomGameObject GetRandomObject(this List<RandomGameObject> list) {
			return GetRandomObject<RandomGameObject, GameObject>(list);
		}
		public static RandomSprite GetRandomObject(this List<RandomSprite> list) {
			return GetRandomObject<RandomSprite, Sprite>(list);
		}
		#endregion

	}
}