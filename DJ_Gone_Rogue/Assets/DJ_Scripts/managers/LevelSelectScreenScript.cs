using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class LevelSelectScreenScript : MonoBehaviour
{
	public static bool onLevelSelectPoint = false;
	public bool levelSelectPlayOnce = true;
	
	public static int level;
	public string sceneName;
	
	public Transform userText;

	// Use this for initialization
	void Start ()
	{
		DJ_Input.inLevelSelect = true;
	}

	// Update is called once per frame
	void Update ()
	{
		ReachLevelSelect();
	}

	public static int NumberOfStars;


	public void ReachLevelSelect()
	{
		for (int i = 0; i < DJ_TileManagerScript.levelselectList.Count; i++)
		{
			if (DJ_PlayerManager.player.GetComponent<DJ_Movement>().currentTilePos.Equals(DJ_TileManagerScript.levelselectList[i]))
			{
				onLevelSelectPoint = true;
				//print("Level: " + level);
				//print("Stars: " + stars);
				if (levelSelectPlayOnce == true)
				{
					level = DJ_TileManagerScript.tileMap[DJ_TileManagerScript.levelselectList[i]].tile.GetComponentInChildren<DJ_LevelSelectScript>().levelNumber;
					sceneName = DJ_TileManagerScript.tileMap[DJ_TileManagerScript.levelselectList[i]].tile.GetComponentInChildren<DJ_LevelSelectScript>().sceneName;

					int numStars = PlayerPrefs.GetInt("stars" + level.ToString());
                    //Debug.Log("lvl string = " + level.ToString());
                    //Debug.Log("numstars = " + numStars);
					NumberOfStars = numStars;

					levelSelectPlayOnce = false;
					return;
				}
				return;
			}
		}
		//print("Player is not on a level select tile right?");
		onLevelSelectPoint = false;
		levelSelectPlayOnce = true;
	}

	public static void PlayLevel()
	{
        VolumeControl.saveData(); // Volume slider data save. Peter Kong, 6/7/14

		DJ_LevelManager.currentLevel = level;
		//Application.LoadLevel("level" + DJ_LevelManager.currentLevel);
		//Debug.Log("Starting to load: level" + DJ_LevelManager.currentLevel);
		//DJ_LoadingScreen.LoadLevel("level" + DJ_LevelManager.currentLevel);
		//Debug.Log ("Im in here: " + level);
		Application.LoadLevel("LoadingScene");
	}
}
