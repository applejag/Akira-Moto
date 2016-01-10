using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonActionScript : MonoBehaviour {

	public FadingTransitionScript fading;

	public void JumpToLevel(int level) {
		fading.StartTransition(false, delegate () {
			SceneManager.LoadScene(level);
		});
	}

	public void JumpToLevel(string level) {
		fading.StartTransition(false, delegate () {
			SceneManager.LoadScene(level);
		});
	}

	public void RestartLevel() {
		JumpToLevel(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}