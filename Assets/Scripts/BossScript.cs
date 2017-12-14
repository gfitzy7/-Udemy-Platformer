using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

	public bool bossActive = false;

	public float startingTimeBetweenDrops;
	public float sawDeltaDecrement;
	private float timeBetweenDrops;
	private float dropCount;

	public float waitForPlatforms;
	private float platformCount;

	public Transform leftSpawnPoint;
	public Transform rightSpawnPoint;
	public Transform sawSpawnPoint;

	public GameObject dropSaw;

	public GameObject boss;
	public bool bossRight;

	public GameObject rightPlatforms;
	public GameObject leftPlatforms;

	public bool takeDamage;

	public int startingHealth;
	private int currentHealth;

	public GameObject levelExit;

	private CameraController camera;

	private LevelManager levelManager;

	public bool waitingForRespawn = false;

	// Use this for initialization
	void Start () {
		dropCount = timeBetweenDrops;
		platformCount = waitForPlatforms;
		currentHealth = startingHealth;
		timeBetweenDrops = startingTimeBetweenDrops;

		boss.transform.position = rightSpawnPoint.position;
		bossRight = true;

		camera = FindObjectOfType<CameraController> ();
		levelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (levelManager.respawnCoActive) {
			bossActive = false;
			waitingForRespawn = true;
		}

		if (waitingForRespawn && !levelManager.respawnCoActive) {
			waitingForRespawn = false;
			boss.SetActive (false);
			leftPlatforms.SetActive (false);
			rightPlatforms.SetActive (false);
			camera.followTarget = true;
		}

		if(bossActive)
		{
			camera.followTarget = false;
			camera.transform.position = Vector3.Lerp (camera.transform.position, new Vector3 (transform.position.x, camera.transform.position.y, camera.transform.position.z), camera.smoothing * Time.deltaTime);

			boss.SetActive (true);

			if(dropCount > 0)
			{
				dropCount -= Time.deltaTime;
			}
			else
			{
				sawSpawnPoint.position = new Vector3(Random.Range(leftSpawnPoint.position.x, rightSpawnPoint.position.x), sawSpawnPoint.position.y , sawSpawnPoint.position.z);
				GameObject.Instantiate(dropSaw, sawSpawnPoint.position, sawSpawnPoint.rotation);
				dropCount = timeBetweenDrops;
			}

			if (platformCount > 0) {
				platformCount -= Time.deltaTime;
			} else {
				if (bossRight) {
					rightPlatforms.SetActive (true);
				} else {
					leftPlatforms.SetActive (true);
				}
			}

			if (takeDamage) {
				currentHealth--;
				takeDamage = false;

				if (currentHealth <= 0) {
					levelExit.SetActive (true);
					camera.followTarget = true;
					gameObject.SetActive (false);
				}


				if (bossRight) {
					boss.transform.position = leftSpawnPoint.position;
				} else {
					boss.transform.position = rightSpawnPoint.position;
				}

				bossRight = !bossRight;

				rightPlatforms.SetActive (false);
				leftPlatforms.SetActive (false);

				platformCount = waitForPlatforms;
				timeBetweenDrops -= sawDeltaDecrement;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player" && !bossActive)
		{
			bossActive = true;
			boss.transform.position = rightSpawnPoint.position;
			bossRight = true;
			platformCount = waitForPlatforms;
			timeBetweenDrops = startingTimeBetweenDrops;
			dropCount = timeBetweenDrops;
			currentHealth = startingHealth;
		}
	}
}