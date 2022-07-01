using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
	public int hitpoints;
	public int secondPhase;				// Trigger phase 2 below this amount of HP
	public int thirdPhase;				// same for phase 3
	public float moveInterval;			// Time between movements
	public float attackTelegraphTime;	// Time that attack sprite is shown before attacking
	public float attackTime;			// Time that attack actually happens
	public Sprite[] sprites;			// Index 0 = idle, 1 = move left, 2 = attack, 3 = move right
	public GameObject deathEffect;
	public GameObject phaseChange;		// Phase change animation
	public GameObject bullet1;			// Phase 1 bullets
	public GameObject[] bullet2;		// Phase 2 bullets
	public GameObject[] bullet3;		// Phase 3 bullets
	public GameObject[] waypoints;		// Phase 1 waypoints
	public GameObject[] waypoints2;		// Phase 2 waypoints (only used by some bosses)

	private int phase = 1;
	private int waypointIndex = 0;			// Next waypoint to move to
	private float attackTimer;			// Timer used for movement and attacks, resets to moveInterval
	private bool attacked = false;			// I wonder what this does...
	private bool movingToNextWaypoint = false;
	private SpriteRenderer spriteRenderer;	// Used for changing sprites
	private AudioSource attackSound;
	private AudioSource phaseChangeSound;
	private AudioSource deathSound;
	private GameObject player;				// The player's GameObject
	private PlayerStatsStorage playerStats;	// Keeps track of player statistics
	private Transform target;				// Transform of the next waypoint

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		target = waypoints[waypointIndex].transform;
		attackTimer = moveInterval;
	}

	void Start() {
		attackSound = GameObject.Find("BossAttackSound").GetComponent<AudioSource>();
		phaseChangeSound = GameObject.Find("PhaseChangeSound").GetComponent<AudioSource>();
		deathSound = GameObject.Find("DeathSound").GetComponent<AudioSource>();
		player = GameObject.Find("Player");
		playerStats = GameObject.Find("PlayerStatsStorage").GetComponent<PlayerStatsStorage>();
	}

	void Update() {
		if (hitpoints < secondPhase && phase < 2) {	// Detects when phase should change
			Instantiate(phaseChange, transform.position, transform.rotation).SetActive(true);
			phase = 2;
			phaseChangeSound.Play();
			waypoints = waypoints2;
		} else if (hitpoints < thirdPhase && phase < 3) {
			Instantiate(phaseChange, transform.position, transform.rotation).SetActive(true);
			phase = 3;
			phaseChangeSound.Play();
		} else if (hitpoints <= 0) {
			hitpoints = 0;
			playerStats.stagesCleared++;
			Instantiate(deathEffect, transform.position, transform.rotation).SetActive(true);
			deathSound.Play();
			Destroy(gameObject);
		}

		if (movingToNextWaypoint) {
			spriteRenderer.sprite = sprites[(int)Mathf.Sign(transform.position.x - target.position.x) + 2];	// Changes sprite based on direction
			transform.position = Vector2.MoveTowards(transform.position, target.position, 5f * Time.deltaTime);

			if (Vector2.Distance(transform.position, target.position) <= 0.01f) {	// Executes when a waypoint is reached
				movingToNextWaypoint = false;
				if (waypointIndex < waypoints.Length - 1) waypointIndex++;
				else waypointIndex = 0;
				spriteRenderer.sprite = sprites[0];
			}
		} else {
			attackTimer -= Time.deltaTime;
			target = waypoints[waypointIndex].transform;	// Sets target to the next waypoint
		}

		if (attackTimer < attackTelegraphTime && !attacked) {
			spriteRenderer.sprite = sprites[2];	// Show attack sprite

			if (attackTimer < attackTime) {
				if (phase == 1) Instantiate(bullet1, transform.position, transform.rotation).SetActive(true);
				if (phase == 2) Instantiate(bullet2[waypointIndex], transform.position, transform.rotation).SetActive(true);
				if (phase == 3) Instantiate(bullet3[waypointIndex], transform.position, transform.rotation).SetActive(true);

				attackSound.Play();
				attacked = true;
			}
		}

		if (attackTimer < 0.4f) spriteRenderer.sprite = sprites[0];	// Set sprite to idle before moving

		if (attackTimer < 0f) {	// Start moving to next waypoint
			attackTimer = moveInterval;	// Reset attack timer
			movingToNextWaypoint = true;
			attacked = false;
		}

		if (player == null) {	// Remove all bullet animations if player is dead
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("HitAnimation")) {
				Destroy(obj);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "PlayerBullet" && player != null) {
			hitpoints--;

			if (playerStats.mana < 256) playerStats.mana++;
			if (hitpoints < 0) hitpoints = 0;
		}
	}
}
