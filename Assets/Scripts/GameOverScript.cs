using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	public static GameOverScript instance;

	public GameObject gameOverPanel;

	void Awake () {
		if (instance != null) {
			print ("Wanna lose twice? AT THE SAME TIME?");
		}
		
		instance = this;

		// Disable the UI
		gameOverPanel.SetActive (false);
	}

	// PlayerScript calls this upon death
	public static void GameOver() {
		instance.gameOverPanel.SetActive (true);
	}

	#region Button events
	public void ButtonRetry() {
		Application.LoadLevel (Application.loadedLevel);
	}

	public void ButtonBackToMenu(string level) {
		Application.LoadLevel (level);
	}
	#endregion
}
