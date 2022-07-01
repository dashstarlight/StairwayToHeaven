using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {
	public GameObject transitionAnimation;

	public void StartTransition() {	// Activates scene transition animation
		transitionAnimation.SetActive(true);
	}

	public void ExitGame() {
		Application.Quit();
	}
}
