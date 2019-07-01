using UnityEngine;
using System.Collections;

/*
 *Author: Davidson Harley
 *Created On: 2/9/2014
 *
 *Summary: Initializes all menu elements
 *
 *Revision Log:
 */

public class DJ_MenuSetup : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		numParams = 4;
		instantiateText();
		setLocations();
	}



	// Update is called once per frame
	void Update () 
	{
	
	}

	void instantiateText()
	{

		menu1 = (GameObject.Instantiate(menuPrefab1) as GameObject);
	

		title = (GameObject.Instantiate(titlePrefab) as GameObject);
	}

	void setLocations()
	{
		title.transform.position = new Vector2(.5f, .85f);
		Debug.Log("Screen width" + Screen.width);
		menu1.transform.position = new Vector2(.5f, .5f);

	}


	public int numParams;
	public GameObject menuPrefab1;
	public GameObject menuPrefab2;
	public GameObject menuPrefab3;
	public GameObject menuPrefab4;

	GameObject menu1;
	GameObject menu2;
	GameObject menu3;
	GameObject menu4;


	public GameObject titlePrefab;
	GameObject title;
}
