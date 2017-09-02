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

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();

		coinText.text = "Coins: " + coinCount;

		currentHealth = maxHealth;

		objectsToReset = FindObjectsOfType<ResetOnRespawn>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Respawn() {
		StartCoroutine("RespawnCo");
	}

	public IEnumerator RespawnCo() {
		currentHealth = 0;
		UpdateHeartMeter();
		player.gameObject.SetActive(false);

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
	}

	public void AddCoins(int coins) {
		coinCount += coins;
		coinText.text = "Coins: " + coinCount;
	}


	public void DamagePlayer(int damage) {
		currentHealth -= damage;
		UpdateHeartMeter();

		if(currentHealth <= 0){
			Respawn();
		}
	}

	public void UpdateHeartMeter() {
		heart1.sprite = (currentHealth >= 2 ? heartFull : (currentHealth == 1 ? heartHalf : heartEmpty));
		heart2.sprite = (currentHealth >= 4 ? heartFull : (currentHealth == 3 ? heartHalf : heartEmpty));
		heart3.sprite = (currentHealth >= 6 ? heartFull : (currentHealth == 5 ? heartHalf : heartEmpty));
	}
}
