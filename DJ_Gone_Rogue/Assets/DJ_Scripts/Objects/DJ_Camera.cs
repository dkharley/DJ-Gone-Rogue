using UnityEngine;
using System.Collections;

public class DJ_Camera : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		targetLookAtPos = new Vector3();
		pos = new Vector3();
	}
	
	// Update is called once per frame
	void Update ()
	{
		targetLookAtPos.y = 0;

		transform.position = pos;

		transform.LookAt(targetLookAtPos);
	}


	public Vector3 pos;
	public Vector3 targetLookAtPos;
}
