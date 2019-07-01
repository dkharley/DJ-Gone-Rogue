using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DJ_Flipper : MonoBehaviour {
	public GameObject spring;
	//private float flipperSpeed = .2f;
	//private DJ_BeatActivation _activationScript;
	
	// Use this for initialization
	void Start () {
		//Flip();
		FixDirection ();
		//gets grandparent
		//_activationScript = transform.parent.GetComponent<DJ_BeatActivation>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Flip(){
		StartCoroutine(FlipThenReset());
	}
	
	private void FixDirection(){
		switch ( spring.GetComponent<DJ_Spring>().direction){
		case DJ_Dir.UP:
			gameObject.transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
			gameObject.transform.localPosition = new Vector3(0.0f, 0.01f, -0.5f);
			break;
		case DJ_Dir.DOWN:
			gameObject.transform.localEulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
			gameObject.transform.localPosition = new Vector3(0.0f, 0.01f, 0.5f);
			break;
		case DJ_Dir.LEFT:
			gameObject.transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
			gameObject.transform.localPosition = new Vector3(-0.5f, 0.01f, 0.0f);
			break;
		case DJ_Dir.RIGHT:
			gameObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
			gameObject.transform.localPosition = new Vector3(0.5f, 0.01f, 0.0f);
			break;
		default:
			break;
		}
	}
	
	IEnumerator FlipThenReset(){
		yield return StartCoroutine(HOTween.To(transform, .2f, "localEulerAngles", new Vector3(0.0f, 0.0f, -90.0f), true).WaitForCompletion());

		/* Uncomment if you want to use flipperSpeed for a smoother and slower rotate
		if (_activationScript.instrument1)
		{
			flipperSpeed = DJ_BeatManager.GetNextLayerOneOn();
			Debug.Log("Im in here");
		}
		if (_activationScript.instrument2)
		{
			flipperSpeed = DJ_BeatManager.GetNextLayerTwoOn();
			Debug.Log("Im in here 2");
		}
		if (_activationScript.instrument3)
		{
			flipperSpeed = DJ_BeatManager.GetNextLayerThreeOn(); 
			Debug.Log("Im in here 3");
		}
		if (_activationScript.instrument4)
		{
			flipperSpeed = DJ_BeatManager.GetNextLayerFourOn(); 
			Debug.Log("Im in here 4");
		}
		*/
		HOTween.To(transform, .2f , "localEulerAngles", new Vector3(0.0f, 0.0f, 90.0f), true);
	}
	
}
