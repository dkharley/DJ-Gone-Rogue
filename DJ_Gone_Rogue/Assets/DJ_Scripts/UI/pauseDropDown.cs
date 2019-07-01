using UnityEngine;
using System.Collections;

public class pauseDropDown : MonoBehaviour {
	//private bool inPauseMenu;
	private GameObject pauseContainer;
	private Vector3 collapsedPosition, expandedPosition;
	public float speed;
	public float pullDownDistance;
	// Use this for initialization
	void Start () {
		speed = .5f;
		pullDownDistance = 600;
		pauseContainer = GameObject.FindGameObjectWithTag(DJ_Tag.DJ_PauseOverlay.ToString());
		collapsedPosition = pauseContainer.transform.localPosition;
		expandedPosition = collapsedPosition - new Vector3 (0, pullDownDistance, 0);
		//inPauseMenu = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnClick()
	{
        if (!DJ_Input.inScoreScreen())
        {
            isPausedScript pause = pauseContainer.GetComponent<isPausedScript>();
            if (pause.isPaused)
            { // Open Pause UI
                TweenPosition.Begin(pauseContainer, speed, collapsedPosition);
                pause.isPaused = false;
                VolumeControl.saveData(); // Volume slider data save. Peter Kong, 6/7/14
            }
            else
            { // Close Pause UI
                TweenPosition.Begin(pauseContainer, speed, expandedPosition);
                pause.isPaused = true;
            }
        }
	}
}
