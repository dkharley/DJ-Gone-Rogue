using UnityEngine;
using System.Collections;

public class DJ_Retry : MonoBehaviour {
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnClick ()
	{
        VolumeControl.saveData(); // Volume slider data save. Peter Kong, 6/7/14
		StartCoroutine(RepeatScene());
	}
	
	IEnumerator RepeatScene()
	{
		AsyncOperation async = Application.LoadLevelAsync("Level" + DJ_LevelManager.currentLevel);
		while(!async.isDone) {
			Debug.Log("Async Progress " + async.progress);
			yield return 0;
		}
	}
}