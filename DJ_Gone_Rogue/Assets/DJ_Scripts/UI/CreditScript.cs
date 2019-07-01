using UnityEngine;
using System.Collections;

public class CreditScript : MonoBehaviour {
	public float yOffset = 500f;
	public float duration = 20f;

	// Use this for initialization
	void Start () {
		TweenPosition.Begin(gameObject, duration, gameObject.transform.localPosition + new Vector3(0, yOffset, 0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
