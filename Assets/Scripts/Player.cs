using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float attackReset = 0.15f;
	public float homingReset = 0.5f;
	public GameObject normalBullet;
	public GameObject homingBullet;
	public GameObject deathEffect;			// Spawned on death, triggers death overlay
	public Sprite[] sprites;				// Player and book sprites

	private int inputX = 0;					// Goes between -1 (left), 0, 1 (right)
	private int inputY = 0;					// same but vertical
	private int movementSpeed = 6;
	private float attackTimer = 0.15f;		// Attack cooldown
	private float homingTimer = 0.5f;		// Homing attack cooldown
	private float manaTimer = 0.05f;		// Mana drain timer
	private BoxCollider2D hitbox;
	private SpriteRenderer spriteRenderer;	// For sprite switching
	private SpriteRenderer book;			// For book animations
	private AudioSource attackSound;
	private AudioSource deathSound;
	private PlayerStatsStorage playerStats;

	void Awake() {
		hitbox = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start() {
		book = GameObject.Find("Book").GetComponent<SpriteRenderer>();
		attackSound = GameObject.Find("PlayerAttackSound").GetComponent<AudioSource>();
		deathSound = GameObject.Find("DeathSound").GetComponent<AudioSource>();
		playerStats = GameObject.Find("PlayerStatsStorage").GetComponent<PlayerStatsStorage>();
	}

	void Update() {
		book.sprite = sprites[3];	// Sets book to idle sprite
		spriteRenderer.color = new Color(255, 255, 255, 1f);
		hitbox.size = new Vector2(0.2f, 0.6f);

		inputX = (int)Input.GetAxisRaw("Horizontal");
		inputY = (int)Input.GetAxisRaw("Vertical");
		transform.Translate(movementSpeed * inputX * Time.deltaTime, movementSpeed * inputY * Time.deltaTime, 0);
		transform.position = new Vector2(Mathf.Clamp(transform.position.x, -2.64f, 2.64f), Mathf.Clamp(transform.position.y, -3.48f, 3.48f));
		spriteRenderer.sprite = sprites[inputX * (int)Time.timeScale + 1];	// Changes the sprite when moving left or right

		if (Time.timeScale != 0f) {
			if (Input.GetKey(KeyCode.X) && playerStats.mana > 0) {	// Ghost mode
				manaTimer -= Time.deltaTime;
				hitbox.size = new Vector2(0f, 0f);
				spriteRenderer.color = new Color(255, 255, 255, 0.5f);
				if (manaTimer < 0f) {
					playerStats.mana--;
					playerStats.manaUsed++;
					manaTimer = 0.05f;
				}
			} 
			
			if (Input.GetKey(KeyCode.LeftShift)) {	// Focus mode
				hitbox.size = new Vector2(0.1f, 0.1f);
				movementSpeed = 4;
			} else movementSpeed = 6;

			if (Input.GetKey(KeyCode.Z)) {
				book.sprite = sprites[4];	// Sets book to attack sprite
				attackTimer -= Time.deltaTime;
				homingTimer -= Time.deltaTime;

				if (attackTimer < 0f) {
					Instantiate(normalBullet, new Vector2(transform.position.x, transform.position.y + 0.56f), transform.rotation).SetActive(true);
					attackSound.Play();
					attackTimer = attackReset;
				}

				if (homingTimer < 0f) {
					Instantiate(homingBullet, new Vector2(transform.position.x + 0.24f, transform.position.y + 0.56f), transform.rotation).SetActive(true);
					Instantiate(homingBullet, new Vector2(transform.position.x - 0.24f, transform.position.y + 0.56f), transform.rotation).SetActive(true);
					homingTimer = homingReset;
				}
			}
		}
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
