using UnityEngine;
using System.Collections;

public class DJ_ClearQuadUI : MonoBehaviour {

    public bool bottomRight;
    public bool bottomLeft;
    public bool topLeft;
    public bool topRight;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter(Collision col)
    {
        if (bottomRight)
            DJ_TutorialMovementScript.reachBottomRight = true;
        if (bottomLeft)
            DJ_TutorialMovementScript.reachBottomLeft = true;
        if (topLeft)
            DJ_TutorialMovementScript.reachTopLeft = true;
        if (topRight)
            DJ_TutorialMovementScript.reachTopRight = true;

        gameObject.SetActive(false);
    }
}
