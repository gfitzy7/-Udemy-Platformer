using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {

	public string levelSelect;
	public string mainMenu;

	private LevelManager levelManager;
	private PlayerController player;

	public GameObject pauseScreen;

	private bool paused = false;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager>();
		player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pause"))
		{
			paused = !paused;
			SetGamePaused();
		}
	}

	public void SetGamePaused()
	{
		if(paused)
		{
			levelManager.levelMusic.Pause();
		}
		else
		{
			levelManager.levelMusic.Play();
		}

		pauseScreen.SetActive(paused);
		player.canMove = !paused;

		Time.timeScale = paused ? 0 : 1;


	}

	public void ResumeGame()
	{
		paused = false;
		SetGamePaused();
	}

	public void LevelSelect()
	{
		PlayerPrefs.SetInt("PlayerLives", levelManager.currentLives);
		PlayerPrefs.SetInt("CoinCount", levelManager.coinCount);

		Time.timeScale = 1;

		SceneManager.LoadScene(levelSelect);
	}

	public void QuitToMainMenu()
	{
		PlayerPrefs.SetInt("PlayerLives", levelManager.currentLives);
		PlayerPrefs.SetInt("CoinCount", levelManager.coinCount);

		Time.timeScale = 1;

		SceneManager.LoadScene(mainMenu);
	}
}
