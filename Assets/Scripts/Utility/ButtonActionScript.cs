using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonActionScript : MonoBehaviour {

	public FadingTransitionScript fading;

	public void JumpToLevel(int level) {
		ResetTimeScale();
        fading.StartTransition(false, delegate () {
			SceneManager.LoadScene(level);
		});
	}

	public void JumpToLevel(string level) {
		ResetTimeScale();
        fading.StartTransition(false, delegate () {
			SceneManager.LoadScene(level);
		});
	}

	public void RestartLevel() {
		ResetTimeScale();
        JumpToLevel(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	void ResetTimeScale() {
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
	}
}