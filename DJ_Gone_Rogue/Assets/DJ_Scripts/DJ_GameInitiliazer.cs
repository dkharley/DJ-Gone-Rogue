using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DJ_GameInitiliazer : MonoBehaviour {

    public const int maxLevel = 18;
    #region starTimeForLevels
    private static int[] starTime = new int[maxLevel] {  40,    // Hello World
                                                         35,    // Where'd You Go
                                                         40,    // Shocking, Ain't it?
                                                         35,    // A Little Push & Shove
                                                         45,    // Playtime Is Over
                                                         80,    // Smile
                                                         55,    // Leap of Faith                
                                                         60,    // Smashing!
                                                         75,    // Hip-Hopscotch
                                                         70,    // Squishy
                                                         0,     // Happy Days
                                                         65,    // Mean Groove
                                                         50,    // Stormgates
                                                         65,    // Counter-melody
                                                         35,    // Kick It
                                                         70,    // Glitchy
                                                         75,    // Chillzilla
                                                         55};   // FilterPass
    #endregion
    #region maxDeathsForLevels
    private static int[] maxDeath = new int[maxLevel] { 3,      // Hello World
                                                        3,      // Where'd You Go
                                                        3,      // Shocking, Ain't it?
                                                        2,      // A Little Push & Shove
                                                        3,      // Playtime Is Over
                                                        2,      // Smile
                                                        3,      // Leap of Faith     
                                                        2,      // Smashing!
                                                        2,      // Hip-Hopscotch
                                                        2,      // Squishy
                                                        0,      // Happy Days
                                                        1,      // Mean Groove
                                                        2,      // Stormgates
                                                        2,      // Counter-melody
                                                        1,      // Kick It
                                                        1,      // Glitchy
                                                        1,      // Chillzilla
                                                        1};     // FilterPass

    #endregion

    // Use this for initialization
	void Start () {
        if (!PlayerPrefs.HasKey("DataInit"))
        {
            SetDefaultDataValues();
            PlayerPrefs.SetBool("DataInit", true);
        }
        PlayerPrefs.SetInt("currentLevel", -1);
        //Debug.Log("Current level number is " + PlayerPrefs.GetInt("currentLevel"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void SetDefaultDataValues()
    {
        //Debug.Log("Initialize default values");
        for (int i = 0; i < maxLevel; i++)
        {
            PlayerPrefs.SetInt("stars" + i.ToString(), 0);
            PlayerPrefs.SetFloat("starTime" + i.ToString(), starTime[i]);
            PlayerPrefs.SetInt("maxDeaths" + i.ToString(), maxDeath[i]);
        }
        PlayerPrefs.SetBool("TutorialBeaten", false);
        PlayerPrefs.SetInt("PlayerX", 0);
        PlayerPrefs.SetInt("PlayerY", 1);
        PlayerPrefs.SetFloat("GameVolume", .5f);
        VolumeControl.setVolume(PlayerPrefs.GetFloat("GameVolume"));
        PlayerPrefs.Flush();
    }

    public static void ResetDefaultDataValues()
    {
        Debug.Log("Resetting values");
        for (int i = 0; i < maxLevel; i++)
        {
            PlayerPrefs.SetInt("stars" + i.ToString(), 0);
            PlayerPrefs.SetFloat("starTime" + i.ToString(), starTime[i]);
            PlayerPrefs.SetInt("maxDeaths" + i.ToString(), maxDeath[i]);
        }
        PlayerPrefs.SetBool("TutorialBeaten", false);
        PlayerPrefs.SetInt("PlayerX", 0);
        PlayerPrefs.SetInt("PlayerY", 1);
        PlayerPrefs.SetFloat("GameVolume", .5f);
        VolumeControl.setVolume(PlayerPrefs.GetFloat("GameVolume"));
        PlayerPrefs.Flush();
    }
}
