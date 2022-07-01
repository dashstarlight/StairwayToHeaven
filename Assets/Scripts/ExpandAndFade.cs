using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandAndFade : MonoBehaviour {
	public int expandSpeed = 4;
	public int fadeSpeed = 4;
	public GameObject setActiveAfterFade;
	private SpriteRenderer spriteRenderer;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update() {
		transform.localScale = transform.localScale * (expandSpeed * Time.deltaTime + 1);	// Increase size
		spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a - fadeSpeed * Time.deltaTime);	// Reduce opacity

		if (spriteRenderer.color.a < -2f) {	// Destroy after 2 seconds of being transparent
			if (setActiveAfterFade != null) setActiveAfterFade.SetActive(true);
			Destroy(gameObject);
		}
	}
}
