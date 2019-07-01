using UnityEngine;
using System.Collections;

public class CurrentLevelText : MonoBehaviour {

	private string[] levelNames = new string[20]
	{ 
		"Hello World", //0
		"Where'd You Go?", //1 
		"Shocking, Ain't It?", //2
		"A Little Push & Shove", //3
		"Playtime Is Over", //4
		"Smile", //5
		"Leap of Faith", //6
		"Smashing!", //7
		"Hip-Hopscotch", //8
		"Squishy", //9
		"Happy Days", //10
		"Mean Groove", //11
		"Stormgates", //12
		"Counter-Melody", //13
		"Kick It", //14
		"Gli-gli-glitchy", //15
		"Chillzhilla", //16
		"Filter Pass", //17
		"Placeholder", //18
		"Placeholder" //19
	};

	private string currLevelName;
	// Use this for initialization
	void Start () {
        if (DJ_LevelManager.currentLevel == -1)
        {
            currLevelName = "Level Select";
        }
        else
        {
            currLevelName = levelNames[DJ_LevelManager.currentLevel];
        }
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<UILabel>().text = currLevelName;
	}
}
