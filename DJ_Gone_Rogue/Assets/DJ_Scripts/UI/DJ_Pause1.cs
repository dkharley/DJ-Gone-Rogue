using UnityEngine;
using System.Collections;

public class DJ_Pause1 : MonoBehaviour {


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnClick()
    {
            NGUITools.SetActive(DJ_UIManager.pauseOverlay, true);
            NGUITools.SetActive(DJ_UIManager.pauseButton, false);
        
    }
}
