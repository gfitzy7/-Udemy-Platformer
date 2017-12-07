using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

	public string levelToLoad;
	public string levelToUnlock;

	public float waitToMove;
	public float waitToLoad;

	public Sprite flagOpen;
	private SpriteRenderer spriteRenderer;

	private bool movePlayer;

	private PlayerController player;
	private CameraController gameCamera;
	private LevelManager levelManager;

	// Use this for initialization
	void Start () {
		movePlayer = false;

		spriteRenderer = GetComponent<SpriteRenderer>();

		player = FindObjectOfType<PlayerController>();
		gameCamera = FindObjectOfType<CameraController>();
		levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(movePlayer)
		{
			player.myRigidbody.velocity = new Vector3(player.moveSpeed, player.myRigidbody.velocity.y, 0f);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			spriteRenderer.sprite = flagOpen;
			StartCoroutine("LevelEndCo");
		}
	}

	public IEnumerator LevelEndCo()
	{
		player.canMove = false;
		gameCamera.followTarget = false;
		levelManager.invincible = true;

		levelManager.levelMusic.Stop();

		player.myRigidbody.velocity = Vector3.zero;

		PlayerPrefs.SetInt("Lives", levelManager.currentLives);
		PlayerPrefs.SetInt("Coins", levelManager.coinCount);

		PlayerPrefs.SetInt(levelToUnlock, 1);

		yield return new WaitForSeconds(waitToMove);

		movePlayer = true;

		yield return new WaitForSeconds(waitToLoad);

		SceneManager.LoadScene(levelToLoad);
	}
}