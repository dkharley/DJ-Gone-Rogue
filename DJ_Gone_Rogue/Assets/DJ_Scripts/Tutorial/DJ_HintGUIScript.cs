using UnityEngine;
using System.Collections;

/// <summary>
/// Script for a tile to display a GUI Text when player enters it.
/// Author: Jason Wang
/// </summary>

public class DJ_HintGUIScript : MonoBehaviour {

    public Transform userText;
    public string hint;
    public bool dontPlayAgain;

    public float xdisplacement;
    public float ydisplacement;
    public float zdisplacement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void OnCollisionEnter(Collision col)
    {
        if (dontPlayAgain == false)
        {
            SpawnText(hint,
            DJ_PlayerManager.player.GetComponent<DJ_Movement>().currentPosition.x + xdisplacement,
            DJ_PlayerManager.player.GetComponent<DJ_Movement>().currentPosition.y + ydisplacement,
            DJ_PlayerManager.player.GetComponent<DJ_Movement>().currentPosition.z + zdisplacement);
            dontPlayAgain = true;
        }

    }

    void SpawnText(string text, float x, float y, float z)
    {
        // convert coordinates into world screen GUI coordinates.
        Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(x, y, z));
        // Scale x releative to the screen width.
        screenPos.x = screenPos.x / Screen.width;
        // Scale y releative to the screen height.
        screenPos.y = screenPos.y / Screen.height;
        Vector3 coordinates = new Vector3(screenPos.x, screenPos.y, 0);
        Transform gui = Instantiate(userText, coordinates, Quaternion.identity) as Transform;
        gui.GetComponent<GUIText>().text = text;
        gui.GetComponent<GUIText>().fontSize = 1;
        //gui.localScale *= .5f;
    }
}
