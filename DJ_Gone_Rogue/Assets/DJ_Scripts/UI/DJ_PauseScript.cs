using UnityEngine;
using System.Collections;

public class DJ_PauseScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

		pauseState = 0;

		quit = (GUIText.Instantiate(quitPrefab) as GUIText);
		background = (GUITexture.Instantiate(backgroundPrefab) as GUITexture);

		quit.GetComponent<Collider>().enabled = false;


		quitColor = quit.color;
		bgColor = background.color;

		quitColor.a = 0;
		bgColor.a = 0;
		quit.color = quitColor;
		background.color = bgColor;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void OnMouseDown()
	{
		if(pauseState%2 == 0)
		{
			Time.timeScale = 0;
			quitColor.a = 1;
			bgColor.a = .5f;
			quit.color = quitColor;
			background.color = bgColor;
			quit.GetComponent<Collider>().enabled = true;
			pauseState++;
		}
		else if(pauseState%2 == 1)
		{
			Time.timeScale = 1;
			quitColor.a = 0;
			bgColor.a = 0;
			quit.color = quitColor;
			background.color = bgColor;
			quit.GetComponent<Collider>().enabled = false;
			pauseState++;
		}
	}


	public GUIText quitPrefab;
	public GUITexture backgroundPrefab;


	GUIText quit;
	GUITexture background;


	public Color quitColor;
	public Color bgColor;

	int pauseState;
}
