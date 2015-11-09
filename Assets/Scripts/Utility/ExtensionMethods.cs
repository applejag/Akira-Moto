using UnityEngine;
using System.Collections;

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
}