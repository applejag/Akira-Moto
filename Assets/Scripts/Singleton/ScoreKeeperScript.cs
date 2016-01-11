using UnityEngine;
using System.Collections;

public class ScoreKeeperScript : SingletonBaseScript<ScoreKeeperScript> { 
	
	public MoonScript moon;
	public HourglassScript hourglass;
	public float timeLeft = 10;
	public float maxTime = 10;
	public int goal = 1;

	private int _score = 0;
	public int score {
		get { return _score; }
		set { int old = _score; _score = value; if (score != old) instance.OnScoreChange(old); }
	}

	private bool outOfTime = false;

	void Update() {
		if (outOfTime)
			return;

		timeLeft = Mathf.Max(timeLeft - Time.deltaTime, 0f);
		hourglass.SetTime(timeLeft / maxTime);

		if (timeLeft <= 0)
			OutOfTime();
	}

	public void OnScoreChange(int old) {
		moon.percentage = score / (float) goal;
	}

	public void OutOfTime() {
		outOfTime = true;
		if (!GameOverScript.instance.over) {
			FindObjectOfType<PlayerScript>().GetComponent<HealthScript>().ModifyHealth(-666,true);
		}
	}

	public void AddTime(float seconds) {
		if (!outOfTime)
			timeLeft = Mathf.Clamp(timeLeft + seconds, 0, maxTime);
	}

	public void MoonFilled() {
		if (!GameOverScript.instance.over) {
			GameOverScript.instance.over = true;
		}
	}

}
