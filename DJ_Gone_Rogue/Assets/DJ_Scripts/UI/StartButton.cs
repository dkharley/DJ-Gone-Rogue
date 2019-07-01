using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {
	
	

	void OnMouseDown() {
		
		Debug.Log("mouseDown");    

		Application.LoadLevel("newScene");

		GetComponentInChildren<TextMesh>().font.material.color = Color.blue;
		
	}
	
	
	
	void OnMouseExit() {
		
		Debug.Log("left the 3d text");
		
		GetComponent<TextMesh>().font.material.color = Color.white;     
		
	}
	
}
