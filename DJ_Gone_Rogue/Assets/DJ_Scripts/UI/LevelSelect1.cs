﻿using UnityEngine;
using System.Collections;

public class LevelSelect1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
        DJ_LevelManager.currentLevel = 1;
		StartCoroutine(StartScene());
	}

	IEnumerator StartScene()
	{
		AsyncOperation async = Application.LoadLevelAsync("Level" + DJ_LevelManager.currentLevel);
		while (!async.isDone)
		{
			Debug.Log("Async Progress " + async.progress);
			yield return 0;
		}
	}
}
