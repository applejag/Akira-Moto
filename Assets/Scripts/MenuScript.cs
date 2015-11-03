using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public void StartGame(string nextLevel = "Stage 01") {
		Application.LoadLevel (nextLevel);
	}

	public void StartGame(int nextLevel = 1) {
		Application.LoadLevel (nextLevel);
	}

}
