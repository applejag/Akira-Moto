using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HourglassScript : MonoBehaviour {

	public RectTransform top;
	public RectTransform bot;
	public Vector2 topYFromTo;
	public Vector2 botYFromTo;

	[Range(0,1)]
	public float shakeBelow = 0.2f;
	public float shakeAmount = 20f;
	private float per;

	public void SetTime(float percentage) {
		per = percentage;
		top.localPosition = new Vector3(top.localPosition.x, Mathf.Lerp(topYFromTo.x, topYFromTo.y, percentage));
		bot.localPosition = new Vector3(bot.localPosition.x, Mathf.Lerp(botYFromTo.x, botYFromTo.y, percentage));
	}

	void FixedUpdate() {
		if (per <= shakeBelow) {
			float p = per / shakeBelow;
			transform.rotation = Quaternion.Euler(0, 0, Random.Range(-shakeAmount * p, shakeAmount * p));
		}
	}
	
}
