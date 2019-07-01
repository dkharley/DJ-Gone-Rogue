using UnityEngine;
using System.Collections;

//Only to be used in the Fast Loading Screen, takes out tap requirement
//This is placed on the Loading text in the loading screen
public class FastLoadingScript : MonoBehaviour {
	private AsyncOperation async = null;
	
	// Use this for initialization
	void Start () {
		//Debug.Log("Loading Level: " + DJ_LevelManager.currentLevel);
		StartCoroutine( Load("level" + DJ_LevelManager.currentLevel) );
	}

	
	//loads the level but does not activate it
	private IEnumerator Load(string level){

		//checks to see if you are trying to go to the level select
		//this is to avoid erroring out trying to load scene-1
		if (DJ_LevelManager.currentLevel == -1){
			async = Application.LoadLevelAsync("levelSelectStage");
		}
		else if (DJ_LevelManager.currentLevel == -2){
			async = Application.LoadLevelAsync("mainScreen");
		}
		else{
			async = Application.LoadLevelAsync(level);
		}
		yield return async;
	}

}
