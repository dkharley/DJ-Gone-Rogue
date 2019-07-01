using UnityEngine;
using System.Collections;

public class DJ_GoToMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
        VolumeControl.saveData(); // Volume slider data save. Peter Kong, 6/7/14
        DJ_LevelManager.currentLevel = -2;
		Application.LoadLevel("mainScreen");
	}
	

}
