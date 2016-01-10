using UnityEngine;
using System.Collections;
using System;

public class RunnerAI : BaseAI {
	protected override bool turnWithAnimation { get { return true; } }

	[Header("RunnerAI")]
	public Transform damagePoint;
	public float damageRadius = 1f;

#if UNITY_EDITOR
	void OnDrawGizmos() {
		Transform selected = UnityEditor.Selection.activeTransform;
		if (selected == transform || (selected == damagePoint && damagePoint != null)) {
			UnityEditor.Handles.color = Color.red;
			UnityEditor.Handles.CircleCap(0, damagePoint.position, Quaternion.identity, damageRadius);
		}
	}
#endif

	#region Animation events
	/*
	 * Methods called by animations.
	*/

	public void SpawnDamage() {
		DamageScript.SpawnDamage(damagePoint.position, damageRadius, 1, health);
	}

	#endregion

}
