using UnityEngine;
using System.Collections;

public class ScoreKeeperScript : SingletonBaseScript<ScoreKeeperScript> { 
	
	public int goal = 1;
	public MoonScript moon;

	private static int _score = 0;
	public static int score {
		get { return _score; }
		set { int old = _score; _score = value; instance.OnScoreChange(old); }
	}
	public static bool gameover = false;

	public void OnScoreChange(int old) {
		moon.percentage = score / (float) goal;
	}

	public void MoonFilled() {
		if (!gameover) {
			gameover = true;
			// Do some stuff
		}
	}

}
