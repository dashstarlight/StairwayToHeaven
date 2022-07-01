using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCounter : MonoBehaviour {
	private TMPro.TextMeshProUGUI textMesh;
	private RectTransform rect;
	private Boss boss;
	private PlayerStatsStorage playerStats;

	void Start() {
		textMesh = GetComponent<TMPro.TextMeshProUGUI>();
		rect = transform.GetChild(0).GetComponent<RectTransform>();
		boss = GameObject.Find("Boss").GetComponent<Boss>();
		playerStats = GameObject.Find("PlayerStatsStorage").GetComponent<PlayerStatsStorage>();
	}

	void Update() {
		if (gameObject.name == "BossHP") {
			textMesh.text = boss.hitpoints.ToString();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, boss.hitpoints);
		} else if (gameObject.name == "Mana") {
			textMesh.text = playerStats.mana.ToString();
			rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, playerStats.mana);
		}
	}
}
