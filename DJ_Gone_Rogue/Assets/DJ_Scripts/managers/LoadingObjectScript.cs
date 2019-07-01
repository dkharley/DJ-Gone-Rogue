using UnityEngine;
using System.Collections;

//This is placed on the Loading text in the loading screen
//This script requires an extra tap to load once loading has finishised
public class LoadingObjectScript : MonoBehaviour {
	private AsyncOperation async = null;
	private bool hasWaited = false;

	// Use this for initialization
	void Start () {
		//Debug.Log("Loading Level: " + DJ_LevelManager.currentLevel);
		StartCoroutine( Load("level" + DJ_LevelManager.currentLevel) );
		StartCoroutine( WaitSeconds(2));
	}

	void Update() {
		//finally activates the level after making the player wait
		if (Input.GetMouseButton(0) && hasWaited){
			ActivateScene();
		}

	}

	//loads the level but does not activate it
	private IEnumerator Load(string level){
		async = Application.LoadLevelAsync(level);
		async.allowSceneActivation = false;
		yield return async;
	}

	//Waits a certain amount of seconds
	private IEnumerator WaitSeconds(float delay){
		yield return new WaitForSeconds(delay);
		hasWaited = true;
		GetComponent<UILabel>().text = "Tap anywhere to start";
		TweenScale.Begin(gameObject, .3f, new Vector3(1.1f, 1.1f, 1.1f));
		//GetComponent<UILabel>().color = Color.white;
		//this.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
	}


	//activates the scene loaded
	public void ActivateScene() {
		async.allowSceneActivation = true;
	}
}
