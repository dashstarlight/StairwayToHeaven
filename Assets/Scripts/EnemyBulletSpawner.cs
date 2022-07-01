using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpawner : MonoBehaviour {
	public int speed;
	public float bulletSpawnDelay;		// Time before bullet spawn
	public float bulletSpawnInterval;	// Time between bullet spawns if repeating
	public bool repeat = false;
	public GameObject bullet;
	public Vector2 target;
	private AudioSource bulletSpawnSound;

	void Start() {
		bulletSpawnSound = GameObject.Find("BulletSpawnerSound").GetComponent<AudioSource>();
	}

	void Update() {
		bulletSpawnDelay -= Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

		if (bulletSpawnDelay < 0f) {	// Spawn bullet, then either destroy self or restart loop
			Instantiate(bullet, transform.position, transform.rotation).SetActive(true);
			bulletSpawnSound.Play();
			if (repeat) bulletSpawnDelay = bulletSpawnInterval;
			else Destroy(gameObject);
		}
	}
}
