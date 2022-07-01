using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public GameObject normalBullet;
	public GameObject homingBullet;
	public GameObject deathEffect;			// Spawned on death, triggers death overlay
	public Sprite[] sprites;				// Player and book sprites

	private int inputX = 0;					// Goes between -1 (left), 0, 1 (right)
	private int inputY = 0;					// same but vertical
	private int movementSpeed = 6;
	private float normalCooldown = 0.15f;	// Bullet cooldown, also used for mana drain
	private float homingCooldown = 0.5f;	// Homing bullet fires 1/4 as fast
	private float manaCooldown = 0.05f;		// Mana drain timer
	private BoxCollider2D hitbox;
	private SpriteRenderer spriteRenderer;	// For sprite switching
	private SpriteRenderer book;			// For book animations
	private SpriteRenderer focusOverlay;	// Enabled when S key is pressed
	private AudioSource attackSound;
	private AudioSource deathSound;
	private PlayerStatsStorage playerStats;

	void Awake() {
		hitbox = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start() {
		book = GameObject.Find("Book").GetComponent<SpriteRenderer>();
		focusOverlay = GameObject.Find("Focus").GetComponent<SpriteRenderer>();
		attackSound = GameObject.Find("PlayerAttackSound").GetComponent<AudioSource>();
		deathSound = GameObject.Find("DeathSound").GetComponent<AudioSource>();
		playerStats = GameObject.Find("PlayerStatsStorage").GetComponent<PlayerStatsStorage>();
	}

	void Update() {
		if (Time.timeScale == 1f) {	// Only run if not paused
			normalCooldown -= Time.deltaTime;
			homingCooldown -= Time.deltaTime;
			manaCooldown -= Time.deltaTime;

			inputX = (int)Input.GetAxisRaw("Horizontal");
			inputY = (int)Input.GetAxisRaw("Vertical");

			book.sprite = sprites[3];						// Sets book to idle sprite
			hitbox.size = new Vector2(0.2f, 0.6f);			// Sets hitbox to normal size
			spriteRenderer.color = focusOverlay.color = new Color(255, 255, 255, 1f);
			spriteRenderer.sprite = sprites[inputX + 1];	// Changes the sprite when moving left or right

			if (Input.GetKey(KeyCode.LeftShift)) {	// Focus mode
				hitbox.size = new Vector2(0.12f, 0.12f);
				focusOverlay.enabled = true;
				movementSpeed = 4;
			} else {
				focusOverlay.enabled = false;
				movementSpeed = 6;
			}

			if (Input.GetKey(KeyCode.Z)) {	// Attack
				book.sprite = sprites[4];	// Sets book to attack sprite

				if (normalCooldown <= 0f) {
					Instantiate(normalBullet, new Vector2(transform.position.x, transform.position.y + 0.56f), transform.rotation).SetActive(true);
					attackSound.Play();
					normalCooldown = 0.15f;
				}

				if (homingCooldown <= 0f) {
					Instantiate(homingBullet, new Vector2(transform.position.x + 0.24f, transform.position.y + 0.56f), transform.rotation).SetActive(true);
					Instantiate(homingBullet, new Vector2(transform.position.x - 0.24f, transform.position.y + 0.56f), transform.rotation).SetActive(true);
					homingCooldown = 0.5f;
				}
			}

			if (Input.GetKey(KeyCode.X) && playerStats.mana > 0) {	// Ghost mode
				hitbox.size = new Vector2(0f, 0f);
				spriteRenderer.color = focusOverlay.color = new Color(255, 255, 255, 0.5f);
				if (manaCooldown < 0f) {
					playerStats.mana--;
					playerStats.manaUsed++;
					manaCooldown = 0.05f;
				}
			}
		}

		transform.Translate(movementSpeed * inputX * Time.deltaTime, movementSpeed * inputY * Time.deltaTime, 0); // Move player

		// Prevent player from going out of bounds
		if (Mathf.Abs(transform.position.x) > 2.64f) transform.position = new Vector2(2.64f * Mathf.Sign(transform.position.x), transform.position.y);
		if (Mathf.Abs(transform.position.y) > 3.48f) transform.position = new Vector2(transform.position.x, 3.48f * Mathf.Sign(transform.position.y));
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (((collision.gameObject.tag == "EnemyBullet") || (collision.gameObject.tag == "Boss")) && GameObject.Find("Boss") != null) {
			if (playerStats.mana > 63) {
				playerStats.mana -= 64;
				playerStats.manaUsed += 64;
			} else {	// Die if player mana is 63 or less
				playerStats.manaUsed += playerStats.mana;
				playerStats.mana = 0;
				Instantiate(deathEffect, transform.position, transform.rotation).SetActive(true);
				deathSound.Play();
				Destroy(gameObject);
			}
		}
	}
}
