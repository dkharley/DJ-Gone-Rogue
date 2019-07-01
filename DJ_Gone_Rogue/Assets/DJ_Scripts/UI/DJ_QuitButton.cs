using UnityEngine;
using System.Collections;

public class DJ_QuitButton : MonoBehaviour {

	void OnClick() 
	{	
		Debug.Log("Pressed the Quit Button");
		Application.Quit();
	}

}

