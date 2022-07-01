using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatsStorage : MonoBehaviour {
	public int previousSceneIndex;
	public int mana = 0;
	public int manaUsed = 0;
	public int manaOld = 0;
	public int manaUsedOld = 0;
	public int stagesCleared = 0;
	public int retries = 0;
	public float playtime = 0f;
	public bool timerRunning;

	void Start() {
		DontDestroyOnLoad(gameObject);	// Script persists between scenes
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {	// Called when scene loads, e.g. level change
		if (scene.buildIndex == previousSceneIndex) {	// Check if retry
			mana = manaOld;
			manaUsed = manaUsedOld;
			retries++;
		} else {
			manaOld = mana;
			manaUsedOld = manaUsed;
			previousSceneIndex = scene.buildIndex;
		};

		if (scene.buildIndex == 1) {	// Reset stats when returning to menu
			mana = manaUsed = 0;
			manaOld = manaUsedOld = 0;
			stagesCleared = 0;
			retries = 0;
			playtime = 0f;
		}

		if (scene.buildIndex == 2) timerRunning = true;		// Start timer when level 1 loaded
		if (scene.buildIndex == 6) timerRunning = false;	// Stop timer on results screen
	}

	void Update() {
		if (timerRunning) playtime += Time.deltaTime;
	}
}
