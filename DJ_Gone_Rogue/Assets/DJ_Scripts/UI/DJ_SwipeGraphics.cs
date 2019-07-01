using UnityEngine;
using System.Collections;

public class DJ_SwipeGraphics : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) && Time.timeScale == 1)
		{
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 70)); 
		}
	}
}
