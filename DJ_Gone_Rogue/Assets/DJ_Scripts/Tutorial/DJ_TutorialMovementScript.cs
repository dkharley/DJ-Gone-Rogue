using UnityEngine;
using System.Collections;

public class DJ_TutorialMovementScript : MonoBehaviour {

    public Transform userText;

    public GUISkin GUISkinUPLEFT;
    public GUISkin GUISkinUPRIGHT;
    public GUISkin GUISkinDOWNLEFT;
    public GUISkin GUISkinDOWNRIGHT;

    public GUISkin GUISkinTUTORIALTESTOR;

    public Texture ArrowUpLeft;
    public Texture ArrowUpRight;
    public Texture ArrowDownLeft;
    public Texture ArrowDownRight;

    public float xdisplacement;
    public float ydisplacement;
    public float zdisplacement;

    public static bool reachBottomRight, reachBottomLeft, reachTopLeft, reachTopRight;

    // Use this for initialization
    void Start()
    {
        reachBottomRight = reachBottomLeft = reachTopLeft = reachTopRight = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnGUI()
    {
       
        if (!reachBottomRight)
        {
            GUI.skin = GUISkinDOWNRIGHT;
            if (GUI.Button(new Rect(Screen.width / 2 + 5, Screen.height / 2 + 5, Screen.width / 2 - 10, Screen.height / 2 - 10), " "))
            {
            }
        }

        if (reachBottomRight && !reachBottomLeft)
        {
            GUI.skin = GUISkinDOWNLEFT;
            if (GUI.Button(new Rect(5, Screen.height / 2 + 5, Screen.width / 2 - 10, Screen.height / 2 - 10), " "))
            {
            }
        }

        if (reachBottomLeft && !reachTopLeft)
        {
            GUI.skin = GUISkinUPLEFT;
            if (GUI.Button(new Rect(5, 5, Screen.width / 2 - 10, Screen.height / 2 - 10), " "))
            {
            }
        }

        if (reachTopLeft && !reachTopRight)
        {
            GUI.skin = GUISkinUPRIGHT;
            if (GUI.Button(new Rect(Screen.width / 2 + 5, 5, Screen.width / 2 - 10, Screen.height / 2 - 10), " "))
            {
            }
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
    }
}

