﻿using UnityEngine;
using System.Collections;

public class quitGameButton : MonoBehaviour {
	
	
	
	void OnMouseDown() {
		
		Debug.Log("mouseDown");    
		
		Application.Quit();
		
		GetComponentInChildren<TextMesh>().font.material.color = Color.blue;
		
	}
	
	
	
	void OnMouseExit() {
		
		Debug.Log("left the 3d text");
		
		GetComponent<TextMesh>().font.material.color = Color.white;     
		
	}
	
}

