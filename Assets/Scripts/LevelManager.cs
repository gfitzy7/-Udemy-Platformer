using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public float respawnDelay;
	private PlayerController player;
	public GameObject deathSplosion;

	public int coinCount;

	public Text coinText;

	public Image heart1;
	public Image heart2;
	public Image heart3;

	public Sprite heartFull;
	public Sprite heartHalf;
	public Sprite heartEmpty;

	public int maxHealth;
	public int currentHealth;

	public ResetOnRespawn[] objectsToReset;

	public bool invincible;

	public Text livesText;
	public int startingLives;
	public int currentLives;

	public GameObject gameOverScreen;

	public AudioSource coinSound;

	public AudioSource levelMusic;
	public AudioSource gameOverMusic;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();

		currentHealth = maxHealth;

		objectsToReset = FindObjectsOfType<ResetOnRespawn>();

		if(PlayerPrefs.HasKey("Coins"))
		{
			coinCount = PlayerPrefs.GetInt("Coins");
		}

		if(PlayerPrefs.HasKey("Lives"))
		{
			currentLives = PlayerPrefs.GetInt("Lives");
		}
		else
		{
			currentLives = startingLives;
		}

		coinText.text = "Coins: " + coinCount;
		livesText.text = "Lives x " + currentLives;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Respawn() {
		currentLives--;
		livesText.text = "Lives x " + currentLives;

		if(currentLives > 0){
			StartCoroutine("RespawnCo");
		}
		else{
			player.gameObject.SetActive(false);
			currentHealth = 0;
			UpdateHeartMeter();

			coinCount = 0;
			coinText.text = "Coins: " + coinCount;

			Instantiate(deathSplosion, player.transform.position, deathSplosion.transform.rotation);

			gameOverScreen.SetActive(true);
			levelMusic.Stop();
			gameOverMusic.Play();
		}
	}

	public IEnumerator RespawnCo() {
		player.gameObject.SetActive(false);
		currentHealth = 0;
		UpdateHeartMeter();

		coinCount = 0;
		coinText.text = "Coins: " + coinCount;

		Instantiate(deathSplosion, player.transform.position, deathSplosion.transform.rotation);

		yield return new WaitForSeconds(respawnDelay);

		foreach (ResetOnRespawn obj in objectsToReset) {
			obj.Reset();
		}

		currentHealth = maxHealth;
		UpdateHeartMeter();
		player.transform.position = player.respawnPos;
		player.gameObject.SetActive(true);
		player.setRespawning(false);
	}

	public void AddCoins(int coins) {
		if(coinCount / 100 < (coinCount + coins) / 100) AddLives(1);

		coinCount += coins;
		coinText.text = "Coins: " + coinCount;

		coinSound.Play();
	}


	public void DamagePlayer(int damage) {
		if(!invincible) {
			currentHealth -= damage;
			UpdateHeartMeter();

			if(currentHealth <= 0){
				Respawn();
			}
			else{
				player.Knockback();

				player.hurtSound.Play();
			}
		}
	}

	public void UpdateHeartMeter() {
		heart1.sprite = (currentHealth >= 2 ? heartFull : (currentHealth == 1 ? heartHalf : heartEmpty));
		heart2.sprite = (currentHealth >= 4 ? heartFull : (currentHealth == 3 ? heartHalf : heartEmpty));
		heart3.sprite = (currentHealth >= 6 ? heartFull : (currentHealth == 5 ? heartHalf : heartEmpty));
	}

	public void AddLives(int livesToAdd){
		currentLives += livesToAdd;
		livesText.text = "Lives x " + currentLives;
		coinSound.Play();
	}

	public void GiveHealth(int healthToGive){
		currentHealth += healthToGive;
		if(currentHealth > maxHealth) currentHealth = maxHealth;
		UpdateHeartMeter();
		coinSound.Play();
	}
}
