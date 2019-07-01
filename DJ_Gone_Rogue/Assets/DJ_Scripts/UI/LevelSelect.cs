using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/*void OnClick () 
	{
		Application.LoadLevel("NewScene");
	}*/

	void OnClick() {
		//AudioSource.PlayClipAtPoint(scratch, Camera.main.transform.position);
		Debug.Log("mouseClick");
		//GetComponentInChildren<TextMesh>().font.material.color = Color.blue;
		StartCoroutine(NextScene());
	}

	IEnumerator NextScene() {
		AsyncOperation async = Application.LoadLevelAsync("newScene");
		//yield return new WaitForSeconds (3.0f);
		while(!async.isDone) {
			Debug.Log("Async Progress " + async.progress);
			yield return 0;
		}
	}
	
}
