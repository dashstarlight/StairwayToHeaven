using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOverlay : MonoBehaviour {
	public GameObject pauseOverlay;
	public GameObject[] transitions;

	private bool transitioning = false;
	private FadeTransition pauseTransition;
	private SpriteRenderer spriteRenderer;

	void Start() {
		pauseTransition = pauseOverlay.GetComponent<FadeTransition>();
		spriteRenderer = pauseOverlay.GetComponent<SpriteRenderer>();
	}

	void Update() {
		foreach (GameObject transition in transitions) {
			if (transition.activeSelf) {
				transitioning = true;
				break;
			} else transitioning = false;
		}

		if ((spriteRenderer.color.a < 1f) && (spriteRenderer.color.a > 0.01f)) transitioning = true;

		if (Input.GetKeyDown(KeyCode.Escape) && !transitioning) {
			if (!pauseOverlay.activeSelf) {
				pauseOverlay.SetActive(true);
				pauseTransition.fadeDirection = 1;
			} else pauseTransition.fadeDirection = -1;
		}
	}

	public void PauseGame() {	// Used by unpause button on click
		pauseTransition.fadeDirection = -1;
	}
}
