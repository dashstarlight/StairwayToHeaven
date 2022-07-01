using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public int speed;
	public bool homing;
	public GameObject hitAnimation;
	private GameObject boss;
	private AudioSource hitSound;
	private Vector2 distance;

	void Start() {
		hitSound = GameObject.Find("BulletImpactSound").GetComponent<AudioSource>();
		boss = GameObject.Find("Boss");

		if (homing && gameObject.tag == "EnemyBullet") {	// Enemy homing bullet will aim towards the player
			distance = GameObject.Find("Player").transform.position - transform.position;	// Calculate the x and y distance from the player
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg - 90f);	// Aims at the player
		}
	}

	void Update() {
		if (homing && boss != null && gameObject.tag == "PlayerBullet") {	// Player homing bullet will aim towards the boss
			distance = boss.transform.position - transform.position;
			distance.Normalize();
			transform.Rotate(Vector3.forward, -Vector3.Cross(distance, transform.up).z * 2f * Mathf.Abs(distance.x + distance.y));
		}

		transform.Translate(Vector3.up * Time.deltaTime * speed);	// Move forwards
		if (Mathf.Abs(transform.position.y) > 4.2f || Mathf.Abs(transform.position.x) > 3.2f) Destroy(gameObject);	// Destroy if out of bounds
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if ((collision.gameObject.tag == "Player" && gameObject.tag == "EnemyBullet") || (collision.gameObject.tag == "Boss" && gameObject.tag == "PlayerBullet") && hitAnimation != null) {
			Instantiate(hitAnimation, transform.position, transform.rotation).SetActive(true);
			hitSound.Play();
			Destroy(gameObject);
		}
	}
}
