using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeTransition : MonoBehaviour {
	public int fadeDirection = -1;
	public int loadSceneIndex = -1;
	private SpriteRenderer spriteRenderer;
	private CanvasGroup canvasGroup;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	void Update() {
		Time.timeScale = 0f;
		spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a + Time.unscaledDeltaTime * fadeDirection);
		if (canvasGroup != null) canvasGroup.alpha = spriteRenderer.color.a;	// Change alpha of the CanvasGroup (makes )

		if (spriteRenderer.color.a < 1f) {
			if (canvasGroup != null) canvasGroup.interactable = false;	// Prevent accidental UI button presses when fading (pause, death overlays etc.)
			if (spriteRenderer.color.a < 0f) {	// Become inactive if not visible (when fading out)
				Time.timeScale = 1f;
				gameObject.SetActive(false);
			}
		}

		if (spriteRenderer.color.a > 1f) {
			if (canvasGroup != null) canvasGroup.interactable = true;	// Only allow UI buttons when fully faded in
			spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
			if (loadSceneIndex > -1) SceneManager.LoadScene(loadSceneIndex); // Load next scene once fully visible (when fading in)
		}
	}
}
