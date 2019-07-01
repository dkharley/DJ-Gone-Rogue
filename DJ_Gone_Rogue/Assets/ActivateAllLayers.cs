using UnityEngine;
using System.Collections;

public class ActivateAllLayers : MonoBehaviour
{
	public bool firstTime = true;
	// Use this for initialization
	void Start () {
		firstTime = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(firstTime) {
			firstTime = false;

			DJ_BeatManager.ActivateLayerOne();
			DJ_BeatManager.ActivateLayerTwo();
			DJ_BeatManager.ActivateLayerThree();
			DJ_BeatManager.ActivateLayerFour();

			this.gameObject.SetActive(false);
		}
	}
}
