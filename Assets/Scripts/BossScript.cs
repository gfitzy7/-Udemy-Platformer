using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

	public bool bossActive = false;

	public float timeBetweenDrops;
	private float dropCount;

	public float waitForPlatforms;
	private float platformCount;

	public Transform leftSpawnPoint;
	public Transform rightSpawnPoint;
	public Transform sawSpawnPoint;

	public GameObject dropSaw;

	// Use this for initialization
	void Start () {
		dropCount = timeBetweenDrops;
		platformCount = waitForPlatforms;
	}
	
	// Update is called once per frame
	void Update () {
		if(bossActive)
		{
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
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player" && !bossActive)
		{
			bossActive = true;
		}
	}
}