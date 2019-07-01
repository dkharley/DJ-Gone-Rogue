using System.Collections;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

/// <summary>
/// DJ_Movement. A component that contains the variables
/// necessary for an entity to move. 
/// 
/// @author -Donnell Lu 1/16/2014
/// @modified -Jason Wang 1/27/2014
///     - Added void fallingPlayer
///     - Added playerHeight, which is go into the parameter in DJ_Util.LerpTrajectory that determines
///       the default y position of the sprite. This is mostly used for falling.
///     - changed contents of update to have a timer that activates once player begins to fall. At
///       the end of the timer, the scene will reset.
/// @modified - Jason Wang 1/31/2014
///     - Added checkEnemy function. A DJ_Point is passed into it and it returns a bool if an enemy's
///     position matches up with the bool.
///     - Modified update with to include checkEnemy. If player's target point returns true in
///     check enemy, then block the player's movement.
/// @editted - Donnell Lu 2/9/2014
/// 	- Refactored all of the player specific code and moved it outside of the movement component.
/// 		- Includes falling, landing on a tile, and starting position.
/// </summary>


public class DJ_Movement: MonoBehaviour
{

    // Used to check if the player should be falling for X amount of time
    public bool isFalling;

    // Tigger boolean used to reset a scene
    // If the entity is a player -> it'll reset the scene.
    public bool afterFalling;

    //Used to check if the entity just landed on a tile.
    // IF the entity is a player -> when the player lands on a tile, it should take damage.
    // BUT it shouldn't take damage AGAIN if the player landed ONBEAT.
    public bool justLanded;

    // Triggers boolean that allows for movement. 
    public bool canMove;

    // Boolean used to calculate if the entity just landed.
    public bool prevCanMove;

    // Previous tile position of this entity
    public DJ_Point prevTilePos;

    // Previous of the... previous tile. Only used to revert process if player is about to move onto enemy
    public DJ_Point prevPrevTilePos;

    // Position of the tile that the entity is currently on
    public DJ_Point currentTilePos;

    // Position of the tile that the entity will move to 
    public DJ_Point targetTilePos;

    // Max distance that the entity can move
    public int maxMoveDistance;

    // Height of the hop between tiles, adjusted via beat
    public float heightOfHop;

    // Is the player lerping at the moment?
    public bool isLerping;

    // Player's y axis location. Helps with falling
    public float playerHeight;

    // Death timer that will start once the player is falling
    private float elapsedDeathTime = 0;
    //private int deathTimeCounter = 0;

    // Speed of the movement between tiles, adjusted via beat
    public float animationLength;

    public float currAnimationTime = 0.0f;

    public Vector3 currentPosition;

    public DJ_Dir direction = DJ_Dir.NONE;
    public DJ_Dir prevDirection = DJ_Dir.NONE;

    //determines whether this object is subjected to gravity
    public bool ignoreGravity;
	// Used this for initialization
	public void Start() 
	{
		//DJ_Input.inputDir = DJ_Dir.NONE;
		isFalling = false;
		afterFalling = false;
		prevCanMove = false;
		canMove = true;

		// Sets the positions based on the transform of the matrix.
		targetTilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.z));
		currentTilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.z));
		prevTilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.z));
		prevPrevTilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.z));
	}

	public void Update()
	{
		//TODO: Peter's Temp fix
		//animationLength = 0.5f * .75f;
//		animationLength = DJ_BeatManager.metronome.GetInterval() * .75f;
            
		//update the current animation time
	    currAnimationTime += Time.deltaTime;

	    //save the current position of the GO so that we  can
	    //modify it and  set the transform.position equal to
	    //the  modified position
	    currentPosition = transform.position;
		// sets the previous move
		prevCanMove = canMove;

	    DJ_Util.GetTilePos(currentPosition, currentTilePos);
        //playerHeight = currentPosition.y;

        // Checks to see if the player should be falling
        checkFalling();

        // If true, activate falling to death
        if (isFalling)
        {
            fallingScript();
        }
        if (DJ_PlayerManager.player.GetComponent<DJ_Damageable>().isAlive)
        {
            // If the entity can move then apply a lerp based on the direction.
            if (canMove)
            {
                justLanded = false;
                if (direction != DJ_Dir.NONE)
                {
                    if (direction == DJ_Dir.TP)
                    {
                        isLerping = true;
                        canMove = false;
                        prevPrevTilePos.Set(prevTilePos);
                        direction = DJ_Dir.NONE;
                    }
                    else
                    {
                        isLerping = true;
                        prevDirection = direction;
                        canMove = false;
                        prevPrevTilePos.Set(prevTilePos);
                        prevTilePos.Set(currentTilePos);
                        DJ_Util.GetNeighboringTilePos(prevTilePos, direction, maxMoveDistance, targetTilePos);
                    }
                }
                currAnimationTime = 0.0f;
            }
            if (currAnimationTime > animationLength)
            {
                maxMoveDistance = 1;
                heightOfHop = 1;
                canMove = true;
                isLerping = false;
                //direction = DJ_Dir.NONE;
                justLanded = true;
                //snap the position
                prevTilePos.Set(targetTilePos);

                // Only update the player's position in the playerPref if in the level select
                // Used to respawn in the correction position whenever they die or come back.

                if (Application.loadedLevelName.Equals("levelSelectStage") && DJ_TileManagerScript.tileMap.ContainsKey(targetTilePos))
                {
                    PlayerPrefs.SetInt("PlayerX", targetTilePos.X);
                    PlayerPrefs.SetInt("PlayerY", targetTilePos.Y);
                    //Debug.Log("Flush this to hash table");
                    //Debug.Log("Flush Player hash table = " + targetTilePos);
                    PlayerPrefs.Flush();
                }

            }

            //else
            {
                DJ_Util.LerpTrajectory(ref currentPosition, prevTilePos, targetTilePos,
                    heightOfHop, currAnimationTime, animationLength, playerHeight);
            }
            switch (DJ_PlayerManager.player.GetComponent<DJ_Damageable>().deathBy)
            {
                case DJ_Death.NONE: transform.position = currentPosition;
                    break;
                case DJ_Death.FALLING: transform.position = currentPosition;
                    break;
                case DJ_Death.FLATTEN: transform.position = new Vector3(currentPosition.x, 0, currentPosition.z);
                    break;
                case DJ_Death.ELECTROCUTED: transform.position = currentPosition;
                    break;
            }
            
            //transform.position = currentPosition;
        }
        switch (DJ_PlayerManager.player.GetComponent<DJ_Damageable>().deathBy)
        {
            case DJ_Death.NONE: transform.position = currentPosition;
                break;
            case DJ_Death.FALLING: transform.position = currentPosition;
                break;
            case DJ_Death.FLATTEN: transform.position = new Vector3(currentPosition.x, 0, currentPosition.z);
                break;
            case DJ_Death.ELECTROCUTED: transform.position = currentPosition;
                break;
        }
	}

	/// <summary>
	/// Changes the player height to make them fall.
	/// Increase playerHeight in the update() rapidly and disable movement.
	/// </summary>
    void fallingPlayer()
    {
        canMove = false;
        if (isLerping == false)
        {
            playerHeight += -0.5f;
        }
    }

	/// <summary>
	/// Fallings the script.
	/// Makes the entity fall. After falling for X time, afterFalling gets triggered to true.
	/// </summary>
    void fallingScript()
    {
        fallingPlayer();

	    if (elapsedDeathTime >= 1)
	    {
			GetComponent<DJ_Damageable>().isAlive = false;
	    }
	    else
	    {
	        elapsedDeathTime += Time.deltaTime;
	    }
    }

    /// <summary>
    /// Checks to see if the player should be falling
    /// </summary>
    void checkFalling()
    {
        // Checks if the target tile that the player is moving to actually exists.
        if (DJ_TileManagerScript.tileMap.ContainsKey(targetTilePos))
        {
            // Makes sure that the target tile is actually rendered
            if (DJ_TileManagerScript.tileMap[targetTilePos].tile.transform.GetChild(0).GetComponent<MeshRenderer>().enabled)
            {
                // Checks if the player position is above a certain point
                if (currentPosition.y > -0.1f)
                {
                    isFalling = false;
                }
                // If they aren't, make sure the player isn't just jumping.
                else
                {
                    if (!isLerping)
                    {
                        isFalling = true;
                    }
                }
            }
            // If the target tile isn't rendered, then make the player fall.
            else
            {
                isFalling = true;
            }
        }
        // If the target tile doesn't actually exist
        else
        {
            // Make sure the player isnt just jumping, then make them fall
            if (!isLerping)
            {
                isFalling = true;
            }
        }
    }

	public void Reset()
	{
		elapsedDeathTime = 0.0f;
		isLerping = false;
		justLanded = false;
		playerHeight = 0.0f;
		isFalling = false;
		currAnimationTime = 0.0f;
		direction = DJ_Dir.NONE;
        prevCanMove = false;
        canMove = true;
        maxMoveDistance = 1;
        direction = DJ_Dir.NONE;
		//deathTimeCounter++;
	}

    bool checkTileUnder()
    {
        //if (DJ_TileManagerScript.tileMap.ContainsKey(player) && DJ_TileManagerScript.tileMap[player].tile.activeInHierarchy)
        if (gameObject.transform.position.y > -0.001)
        {
            //print("You are stepping on a tile");
            return true;
        }
        else
        {
            //print("ENTERING DANGEROUS AREA!");
            return false;
        }
    }

}