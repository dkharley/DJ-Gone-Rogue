using UnityEngine;
using System.Collections;

public class DJ_ScoreScreen : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		instantiateText();
		setLocations();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void instantiateText()
	{
		
		next = (GameObject.Instantiate(nextPrefab) as GameObject);
		
		retry = (GameObject.Instantiate(retryPrefab) as GameObject);
		
		quit = (GameObject.Instantiate(quitPrefab) as GameObject);
		
		title = (GameObject.Instantiate(titlePrefab) as GameObject);
	}
	
	void setLocations()
	{
		title.transform.position = new Vector2(.5f, .8f);
		next.transform.position = new Vector2(.5f, .5f);
		retry.transform.position = new Vector2(.5f, .4f);
		quit.transform.position = new Vector2(.5f, .3f);

	}


	public GameObject nextPrefab;
	public GameObject retryPrefab;
	public GameObject quitPrefab;
	
	GameObject next;
	GameObject retry;
	GameObject quit;
	
	
	public GameObject titlePrefab;
	GameObject title;
}
