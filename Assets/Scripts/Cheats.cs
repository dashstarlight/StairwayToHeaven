using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour {
	private string cheatCodeInput;
	private string cheatCodeHash;
	private bool playerInvincible = false;

	void Start() {
		DontDestroyOnLoad(gameObject);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Return)) {
			cheatCodeHash = Hash128.Compute(cheatCodeInput).ToString();
			Debug.Log(cheatCodeInput);
			Debug.Log(cheatCodeHash);

			switch (cheatCodeHash) {
				case "926497d33d704726b62d9ebcd8e62dda":
					SceneManager.LoadScene(2);
					break;
				case "4d1db4740478409817748bbdd431f3f4":
					SceneManager.LoadScene(3);
					break;
				case "c8d37a26bd7d6c02a3c1a99b44b57e13":
					SceneManager.LoadScene(4);
					break;
				case "b853dd2116b45a6d43e496404a89b6f4":
					SceneManager.LoadScene(5);
					break;
				case "a0eebce56456824409c3ce4d695de06a":
					GameObject.Find("Player").GetComponent<Player>().attackReset = 0.05f;
					break;
				case "f960ca2eb7aeb3d602e3960fb730c6f1":
					gameObject.GetComponent<PlayerStatsStorage>().maxMana = 999;
					gameObject.GetComponent<PlayerStatsStorage>().mana = 999;
					break;
				case "36451c9e36ed3458eebd365006ae0279":
					playerInvincible = !playerInvincible;
					break;
			}

			cheatCodeInput = "";
		} else cheatCodeInput += Input.inputString;

		if (playerInvincible) gameObject.GetComponent<PlayerStatsStorage>().mana = 256;
	}
}
