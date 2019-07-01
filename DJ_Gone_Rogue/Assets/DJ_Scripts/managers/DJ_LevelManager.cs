using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

/// <summary>
/// DJ_LevelManager.cs: This script is responsible for calling the parser
/// to parse a certain level. IMPORTANT: This has to be called before the
/// TileManager is instantiated or else you will have an empty game world.
/// 
/// @author - Fernando Carrillo 1/23/2014
/// @modified - Jason Wang 1/27/2014
///     - added public static int currentLevel to represent current level
///     - other scripts can obtain current level with DJ_LevelManager.currentLevel
///     - added public static void resetScene to reset current scene
///     
/// </summary>
public class DJ_LevelManager : MonoBehaviour {

    public static int currentLevel 
    {
        get 
        { 
            return PlayerPrefs.GetInt("currentLevel"); 
        }
        set
        {
            PlayerPrefs.SetInt("currentLevel", value);
            PlayerPrefs.Flush();
        }
    }
    public static float startTime;
    public static float timeElapsed;
    private static bool startTimer;

	// Use this for initialization
	void Start ()
	{
        startTime = 0.0f;
        timeElapsed = 0;
        startTimer = false;
	}

    public static void nextLevel()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        currentLevel++;
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.Flush();
    }

	// Update is called once per frame
    void Update()
    {
        if (DJ_CameraManager.startOfLevel == false && startTime <= 0)
        {
            startTime = Time.time;
            startTimer = true;
        }
        if (startTimer && !DJ_PlayerManager.PlayerReachedEndOfLevel)
        {
            timeElapsed = Time.time - startTime;
        }
    }

	public static bool isTileAvailable(DJ_Point pos)
	{
		return DJ_TileManagerScript.isTileAvailable(pos) && DJ_PlayerManager.isTileAvailable(pos);
	}

    public static void resetScene()
    {
        //DJ_PlayerManager.playerFalling = false;
        Application.LoadLevel(DJ_LevelManager.currentLevel);
    }
}
