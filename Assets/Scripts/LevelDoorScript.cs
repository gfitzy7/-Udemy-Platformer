using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoorScript : MonoBehaviour {

	public string levelToLoad;

	public bool isUnlocked;

	public Sprite doorBottomOpen;
	public Sprite doorTopOpen;
	public Sprite doorBottomClosed;
	public Sprite doorTopClosed;

	public SpriteRenderer doorTop;
	public SpriteRenderer doorBottom;

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt("Level1", 1);

		isUnlocked = (PlayerPrefs.GetInt(levelToLoad) == 1);

		if(isUnlocked)
		{
			doorTop.sprite = doorTopOpen;
			doorBottom.sprite = doorBottomOpen;
		}
		else
		{
			doorTop.sprite = doorTopClosed;
			doorBottom.sprite = doorBottomClosed;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			if(Input.GetButtonDown("Jump") && isUnlocked)
			{
				SceneManager.LoadScene(levelToLoad);
			}
		}
	}
}