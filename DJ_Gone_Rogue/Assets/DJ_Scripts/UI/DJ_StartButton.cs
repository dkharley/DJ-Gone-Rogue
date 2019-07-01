using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

/*
 *Author: Davidson Harley
 *Version: 1.0
 *Created On: 2/9/2014
 *
 *Revision Log:
 */

public class DJ_StartButton : MonoBehaviour {
	
	void OnClick() 
	{
		//iterates though all game objects tagged UI
		foreach (GameObject uiPiece in GameObject.FindGameObjectsWithTag("UI")) {
			NGUITools.SetActive(uiPiece, false);
		}
        
        if (PlayerPrefs.GetBool("TutorialBeaten"))
        {
            DJ_LevelManager.currentLevel = -1;
            //DJ_LoadingScreen.LoadLevel("LevelSelectStage");
        }
        else
        {
            DJ_LevelManager.currentLevel = 0;
            //DJ_LoadingScreen.LoadLevel("Level0");
        }
        Application.LoadLevel("FastLoadingScene");

	}

}
