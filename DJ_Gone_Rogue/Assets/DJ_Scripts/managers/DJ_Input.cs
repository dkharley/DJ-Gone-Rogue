using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// 
/// 
/// @modified - Jason Wang 1/27/2014
/// <summary>
///  
/// </summary>

public class DJ_Input : MonoBehaviour {

    private float tapTimer;
    public float tapThreshold;

    private bool prevMouseDown;
    private bool mouseDown;
    private bool prevKeyDown;
    private bool keyDown;

    public int currLevel;
    public Vector2 currentMouse; //Checks the current mouse or touch location
    public static DJ_Dir inputDir; //Check which direction has been swiped

    public static bool inLevelSelect = false;

	// This will be utilized in BeatManager to check Input
	public static bool onInput
	{
		get;
		set;
	}

	// Use this for initialization
	void Start () 
	{

	}

	void Awake()
	{
		inputDir = DJ_Dir.NONE;
		currentMouse = new Vector2(0, 0);
        currLevel = DJ_LevelManager.currentLevel;
		onInput = false;

		tapTimer = 0.0f;
		prevMouseDown = false;
        mouseDown = false;

        prevKeyDown = false;
        keyDown = false;
	}

	void Update ()
	{
		tapTimer += Time.deltaTime;

		onInput = false;
		inputDir = DJ_Dir.NONE;

        UpdateKeyboardAndMouseDown();

		bool test = OnLevelSelect();

        if (ValidInputToMove())
		{

            // VITAL for pausing to work properly
			if(test)
			{
			}
            // VITAL for pausing to work properly
			else if (inPauseMenu()){
			}
			// VITAL for score screen to work properly
			else if (inScoreScreen()){
			}
			else
			{
				checkScreen();
                checkKeys();
			}
		}

		checkResetRoom();
        
        //set the input of the movement
		GetComponent<DJ_Movement>().direction = inputDir;

		prevMouseDown = mouseDown;
        prevKeyDown = keyDown;
	}
	
    /// <summary>
    /// Updates the Keyboard and Mouse Lect Click Press boolean used for validInputToMove
    /// </summary>
    public void UpdateKeyboardAndMouseDown() 
    {
        mouseDown = Input.GetMouseButton(0);

        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
        {
            keyDown = true;
        }
        else
        {
            keyDown = false;
        }
    }

    /// <summary>
    /// Determines whether or not the player should be able to move.
    /// Needs to have pressed a button and previous button be false.
    /// </summary>
    /// <returns>Returns true if valid input</returns>
    public bool ValidInputToMove()
    {
        if ((mouseDown && !prevMouseDown) || (keyDown && !prevKeyDown))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	/// <summary>
    /// Checks to see if already in score screen
	/// </summary>
	/// <returns>Returns true if in score screen</returns>
	public static bool inScoreScreen(){
		GameObject scoreScreenContainer = GameObject.FindGameObjectWithTag("UI_ScoreScreen");
		 if (scoreScreenContainer != null && scoreScreenContainer.GetComponent<ScoreScreenScript>().isActivated == true){
			return true;
		}
		else return false;
	}

    /// <summary>
    /// Checks to see if already in the pause menu
    /// </summary>
    /// <returns>Returns true if in the pause menu</returns>
	public static bool inPauseMenu(){
		GameObject pauseContainer = GameObject.FindGameObjectWithTag(DJ_Tag.DJ_PauseOverlay.ToString());
		if (pauseContainer.GetComponent<isPausedScript> ().isPaused == true){
			return true;
		}

		//checks to see if you are trying to press the button.
		Camera cam = GameObject.FindGameObjectWithTag("UI_Camera").GetComponent<Camera>();
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		//get all the colliders hit by the raycast
		RaycastHit[] hit = Physics.RaycastAll(ray);

		//cycle through the colliders
		for(int r = 0; r < hit.Length; ++r)
		{
			//get the enemy id associated with the enemy
			string _tag = hit[r].collider.gameObject.tag;
			if(_tag.CompareTo("PlayPauseButton") == 0)
			{
				return true;
			}
		}
		return false;
	}

	public static bool OnLevelSelect()
	{

        if (DJ_CameraManager.startOfLevel)
        {
            if (Input.GetMouseButton(0))
            {
                DJ_CameraManager.speedThingsUp = true;
                return true;
            }

            if (Input.GetKeyDown("a") || Input.GetKeyDown("d") || Input.GetKeyDown("w") || Input.GetKeyDown("s")) //right
            {
                DJ_CameraManager.speedThingsUp = true;
                return true;
            }
        }

        if (!inLevelSelect)
            return false;

		TileTextScript.OnHover = false;
		//mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, .01f));
//		Ray ray = new Ray(mousePos, Camera.main.transform.forward);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		//get all the colliders hit by the raycast
		RaycastHit[] hit = Physics.RaycastAll(ray);

		//Debug.Log ("Number of hit objects " + hit.Length);

		//cycle through the colliders
		for(int r = 0; r < hit.Length; ++r)
		{
			//if the gameobject attached to the collider is an enemy
			//confirmed by checking the GOs tag string
			//if(hit[r].collider.transform.parent != null && hit[r].collider.transform.parent.gameObject.tag.CompareTo("DJ_Enemy") == 0)
			{

				//get the enemy id associated with the enemy
				string _tag = hit[r].collider.gameObject.tag;
				//Debug.Log("Input hit " + _tag);
				if(_tag.CompareTo("level_select_button") == 0)
				{
					TileTextScript _script = hit[r].transform.parent.GetComponent<TileTextScript>();
					//Debug.Log("Is active = " + _script.Active);
					//Debug.Log("CurrentPlayAnimationTime = " + _script.CurrentPlayAnimationTime);
					if(_script.Active && 
					   _script.CurrentPlayAnimationTime > 0.001f)
					{
						TileTextScript.OnHover = true;
						if(Input.GetMouseButton(0))
							_script.Selected = true;
						else
							_script.Selected = false;
						return true;
					}
				}
			}
		}

		return false;
	}

    /// <summary>
    /// Checks what quarter of the screen has been touched or clicked
    /// </summary>
	public void checkScreen()
	{
		//if(prevMouseDown || Input.GetMouseButton(0))
        //if (mousedown
		{
			//currentMouse.x = Input.mousePosition.x;
			currentMouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			//get the quadrant the mouse/touch was
			int area = GetAreaFromPosition(currentMouse);

			//now based off of the area the mouse pos/touch was on the screen,
			//set the direction of the player
			if(area == 0)
				inputDir = DJ_Dir.UP;
			else if(area == 1)
				inputDir = DJ_Dir.RIGHT;
			else if(area == 2)
				inputDir = DJ_Dir.DOWN;
			else if(area == 3)
				inputDir = DJ_Dir.LEFT;

			if(area != -1)
				onInput = true;
		}
	}

	//determines where on screen the touch/mouse pos is
	private static int GetAreaFromPosition(Vector2 pos)
	{
		int area = -1;

        bool onTop = false;
        bool onRight = false;

		float width = Screen.width;
		float height = Screen.height;

        onTop = pos.y >= height / 2;
        onRight = pos.x >= width / 2;


        if (onTop && onRight)
            area = 3;
        if (!onTop && onRight)
            area = 2;
        if (onTop && !onRight)
            area = 0;
        if (!onTop && !onRight)
            area = 1;
		
		return area;
	}

	public static Vector3 GetMousePos()
	{
		return Input.mousePosition;
	}

	public static Vector3 GetMouseWorldPos()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	//Checks the WASD keys for which direction to move
	public void checkKeys()
	{
		if(Input.GetKeyDown("a")) //right
		{
			inputDir = DJ_Dir.RIGHT;
			onInput = true;
		}
		if(Input.GetKeyDown("d")) //left
		{
			inputDir = DJ_Dir.LEFT;
			onInput = true;
		}
		if(Input.GetKeyDown("w")) //up
		{
			inputDir = DJ_Dir.UP;
			onInput = true;
		}
		if(Input.GetKeyDown("s")) //down
		{
			inputDir = DJ_Dir.DOWN;
			onInput = true;
		}
	}

    /// <summary>
    /// Checks the r key to restart the scene
    /// </summary>
    public void checkResetRoom()
    {
        // Commented out resettting the scene
        /*
        if (Input.GetKeyDown("r"))
        {
            //print("Hard resetting the level.");
            DJ_LevelManager.resetScene();
        }
        */
    }
}
