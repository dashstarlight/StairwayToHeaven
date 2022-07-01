using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Results : MonoBehaviour {
	public string statistic;
	private PlayerStatsStorage playerStats;
	private TMPro.TextMeshProUGUI textMesh;

	void Start() {
		playerStats = GameObject.Find("PlayerStatsStorage").GetComponent<PlayerStatsStorage>();
		textMesh = GetComponent<TMPro.TextMeshProUGUI>();

		switch (statistic) {	// Allows selecting the statistic in Unity by changing statistic
			case "clear":		// There's surely better ways of doing this
				textMesh.text = playerStats.stagesCleared.ToString();
				break;
			case "mana":
				textMesh.text = playerStats.manaUsed.ToString();
				break;
			case "retry":
				textMesh.text = playerStats.retries.ToString();
				break;
			case "time":		// This case converts playtime (float) to a TimeSpan, then formats it
				textMesh.text = TimeSpan.FromSeconds(playerStats.playtime).ToString("mm\\:ss");
				break;
			case "skill":		// The massive if block checks the stats and sets the text to a skill rating
				if (playerStats.stagesCleared == 1) textMesh.text = "Apprentice";
				if (playerStats.stagesCleared == 2) textMesh.text = "Journeyman";
				if (playerStats.stagesCleared == 3) textMesh.text = "Sorcerer";
				if (playerStats.stagesCleared == 4) {
					if (playerStats.manaUsed == 0) textMesh.text = "Evasion Master";	
					if (playerStats.manaUsed > 0) textMesh.text = "Sharpshooter";
					if (playerStats.manaUsed > 768) textMesh.text = "Shadow Warrior";
					if (playerStats.manaUsed > 1536) textMesh.text = "Apparition";
					if (playerStats.retries == 0) textMesh.text = "Perfectionist";	// Finished without being killed
					if (playerStats.playtime < 180f) textMesh.text = "Speedrunner";	// Finished the game in under 3 minutes
					if (playerStats.retries > 16) textMesh.text = "Completionist";	// Retried way too many times
				}
				break;
		}
	}
}
