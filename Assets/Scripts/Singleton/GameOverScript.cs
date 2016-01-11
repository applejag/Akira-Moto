using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverScript : SingletonBaseScript<GameOverScript> {

	public RectTransform winTitle;
	public RectTransform winButtons;
	public RectTransform lossTitle;
	public RectTransform lossButtons;
	[Space]
	public float zoomToSize = 5;
	public float zoomToY = 5;
	[Range(0,1)]
	public float slowDownTo = 0.5f;
	public float delay = 1f;

	private bool _over = false;
	public bool over { get { return _over; }
	set { var old = _over; _over = value; if(value && !old) OnGameOver(); }
	}

	private float zoomToSizeDelay;
	private float zoomToYDelay;

	private RectTransform title;
	private RectTransform buttons;
	private float titleHeight;
	private float buttonsHeight;
	private float titleDelay;
	private float buttonsDelay;

	private Stages stage = Stages.idle;

	void Start() {
		over = false;

		zoomToSizeDelay = delay / (Camera.main.orthographicSize - zoomToSize);
		zoomToYDelay = delay / (Camera.main.transform.position.y - zoomToY);

		titleHeight = winTitle.sizeDelta.y;
		buttonsHeight = winButtons.sizeDelta.y;
		titleDelay = delay / titleHeight;
		buttonsDelay = delay / buttonsHeight;
		SetHeight(winTitle, 0);
		SetHeight(winButtons, 0);
	}

	void Update() {
		switch (stage) {
			case Stages.idle:
				if (over)
					stage = Stages.zoom;
				break;

			case Stages.zoom:
				// Slow down time
				Time.timeScale = Mathf.MoveTowards(Time.timeScale, slowDownTo, Time.unscaledDeltaTime / delay);
				Time.fixedDeltaTime = 0.02f * Time.timeScale;

				var cam = Camera.main;
				// Move camera on the Y axis
				cam.transform.position = new Vector3(
					cam.transform.position.x,
					Mathf.MoveTowards(cam.transform.position.y, zoomToY, Time.unscaledDeltaTime / zoomToYDelay),
					cam.transform.position.z);
				// Zoom
				cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, zoomToSize, Time.unscaledDeltaTime / zoomToSizeDelay);

				if (Time.timeScale == slowDownTo)
					stage = Stages.reveal;
				break;

			case Stages.reveal:
				SetHeight(title, Mathf.MoveTowards(title.sizeDelta.y, titleHeight, Time.unscaledDeltaTime / titleDelay));
				SetHeight(buttons, Mathf.MoveTowards(buttons.sizeDelta.y, buttonsHeight, Time.unscaledDeltaTime / buttonsDelay));
				break;
		}
	}

	void OnGameOver() {
		if (ScoreKeeperScript.instance.score >= ScoreKeeperScript.instance.goal) {
			title = winTitle;
			buttons = winButtons;

			// Kill all enemies
			foreach (var health in FindObjectsOfType<HealthScript>()) {
				if (health.isEnemy)
					health.ModifyHealth(-health.health);
			}
		} else {
			title = lossTitle;
			buttons = lossButtons;
		}
	}

	void SetHeight(RectTransform rect, float height) {
		rect.sizeDelta = new Vector2(rect.sizeDelta.y, height);
		
		rect.gameObject.SetActive(height != 0);
		
	}

	public enum Stages {
		idle, zoom, reveal
	}
}
