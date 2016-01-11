using UnityEngine;
using System.Collections;

public class ScoreKeeperScript : SingletonBaseScript<ScoreKeeperScript> { 
	
	public MoonScript moon;
	public int goal = 1;

	private int _score = 0;
	public int score {
		get { return _score; }
		set { int old = _score; _score = value; if (score != old) instance.OnScoreChange(old); }
	}

	public void OnScoreChange(int old) {
		moon.percentage = score / (float) goal;
		if (score >= goal) {
			
		}
	}

	public void MoonFilled() {
		if (!GameOverScript.instance.over) {
			GameOverScript.instance.over = true;
		}
	}

}
