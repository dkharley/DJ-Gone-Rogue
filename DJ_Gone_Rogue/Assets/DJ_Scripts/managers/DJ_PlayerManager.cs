using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;



/// <summary>
/// DJ_PlayerManager script. This script is responsible for managing
/// the player information such as TilePosition, if they get
/// attacked, and if the player dies.
/// 
/// @author - Donnell Lu 2/9/2014
/// @Edditted - Wyatt Sanders
/// </summary>
/// 


public class DJ_PlayerManager : MonoBehaviour
{
    private int earnedStars;
    private int fileStars;
    private bool isTutorialBeaten;
    private DJ_Point hashTablePosition;
    //TODO - will be used later for level editor
    //bool inEditorMode = false;

    /// <summary>
    /// Player ID.
    /// </summary>
    public static int playerID;

    /// <summary>
    /// Player GameObject. Static.
    /// </summary>
    public static GameObject player;

    /// <summary>
    /// The m_player prefab.
    /// </summary>
    public GameObject m_playerPrefab;

    public static DJ_Point spawnPoint;
    public static DJ_Point tutorialPoint;

    public static bool onLevelSelectPoint = false;
    public bool levelSelectPlayOnce = true;
    public int deathCount = 0;

    public int level;
    public string sceneName;

    bool prevAlive = false;
    bool allowRespawn = false;

	bool checkForDeath = false;
	// Called only once during it's lifetime.
	public void Awake()
	{
		checkForDeath = false;
	}

	// Use this for initialization
	public void Start ()
	{
        //GameObject.FindGameObjectWithTag(DJ_Tag.DJ_LevelManager.ToString()).GetComponent<DJ_SaveLoad>().Save(null);
		createPlayer ();
		PlayerReachedEndOfLevel = false;
	}

	int count = 0;

	public void Update ()
	{ 	
		/*
		* DL - 
		* Update's the player's Tile Position based on the player transform matrix.
		* This occurs only when a player is ON a tile. Not during the LERP/JUMP.
		*/
        //Debug.Log("UPDATE Player transform position is " + player.transform.position);

        if (player.GetComponent<DJ_Movement>().canMove)
        {
            playerTilePos.X = Mathf.RoundToInt(player.transform.position.x);
            playerTilePos.Y = Mathf.RoundToInt(player.transform.position.z);
        }
        
        // Checks to see if the player reaches a checkpoint tile.
        //if (DJ_TileManagerScript.tileGraphFinishedLoading)
        //{
            SaveCheckPoint();
        //}

        // Checks to see if the player reaches the end of the level.
        // TODO: Needs to be a tile not a door.
        ReachEndLevel();

        // Checks to see if the player dies. If so, respawn them.
        RespawnCharacter();

		if(player.GetComponent<DJ_Movement>().isFalling && !checkForDeath)
		{
			count++;
			if(count > 16)
			{
				count = 0;
				checkForDeath = true;
				Vector3 v = new Vector3();
				v.x = player.GetComponent<DJ_Movement>().targetTilePos.X;
				v.z = player.GetComponent<DJ_Movement>().targetTilePos.Y;
				//EffectsManager.PlayEffect("playerDeath", v);
			}
		}
	}

	public static void DamagePlayer(float damage)
	{
		player.GetComponent<DJ_Damageable>().damageOfLastHit = damage;
	}

	public static void HealPlayer(float heal)
	{
		player.GetComponent<DJ_Damageable>().healedAmount = heal;
	}

	public static void Dispose ()
	{
		
	}

	public static DJ_Point playerTilePos
	{
		get;
		private set;
	}
	
	public static DJ_Point prevPlayerTilePos
	{
		get;
		private set;
	}

    /// <summary>
    /// Creates the player gameObject, either Male or Female. Places them in the right location.
    /// </summary>
	public void createPlayer()
    {
        player = (GameObject.Instantiate(m_playerPrefab) as GameObject);
        playerID = player.GetInstanceID();

        // This means you're on level select
        hashTablePosition = new DJ_Point(PlayerPrefs.GetInt("PlayerX"), PlayerPrefs.GetInt("PlayerY"));
        if (Application.loadedLevelName.Equals("levelSelectStage") && DJ_TileManagerScript.tileMap.ContainsKey(hashTablePosition))
        {
            //Debug.Log("Exists");
            player.transform.position = new Vector3(hashTablePosition.X, 0.0f, hashTablePosition.Y);
        }
        else 
        {
            //Debug.Log("Spawn point spawn");
            player.transform.position = new Vector3(spawnPoint.X, 0.0f, spawnPoint.Y);
        }

        prevPlayerTilePos = new DJ_Point(int.MaxValue, int.MaxValue);

        playerTilePos = new DJ_Point(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.z));
        //Debug.Log("player tilePos in creation " + playerTilePos);
        
        player.transform.parent = transform;

        (player.GetComponent(typeof(DJ_Movement)) as DJ_Movement).currentTilePos = playerTilePos;
        (player.GetComponent(typeof(DJ_Movement)) as DJ_Movement).targetTilePos = playerTilePos;
        (player.GetComponent(typeof(DJ_Movement)) as DJ_Movement).prevTilePos = playerTilePos;
        (player.GetComponent(typeof(DJ_Movement)) as DJ_Movement).prevPrevTilePos = playerTilePos;

    }

    /// <summary>
    /// Checks to see if the player reaches a checkpoint tile. Sets the current checkpoint and next one.
    /// @author - Donnell Lu
    /// </summary>
    public void SaveCheckPoint()
    {
        if (DJ_LevelManager.currentLevel != -1)
        {
            for (int i = 0; i < DJ_TileManagerScript.checkpointList.Count; i++)
            {
                if (DJ_PlayerManager.player.GetComponent<DJ_Movement>().currentTilePos.Equals(DJ_TileManagerScript.checkpointList[i]))
                {
                    spawnPoint = DJ_TileManagerScript.checkpointList[i];
                }
            }
        }
		
        //debug draw of checkpoints
		/*
        for (int i = 0; i < DJ_TileManagerScript.checkpointList.Count; ++i)
		{
            DJ_Point p = DJ_TileManagerScript.checkpointList[i];
			if(DJ_TileManagerScript.tileMap.ContainsKey(p))
                DJ_TileManagerScript.tileMap[DJ_TileManagerScript.checkpointList[i]].tile.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
		}
        if (DJ_TileManagerScript.tileMap.ContainsKey(DJ_TileManagerScript.exitPoint))
        {
            DJ_TileManagerScript.tileMap[DJ_TileManagerScript.exitPoint].tile.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        }
        */
	}

	public static bool PlayerReachedEndOfLevel = false;

    /// <summary>
    /// Checks to see if the player reaches the end of the level. Loads 'scorescreen'.
    /// @author - Donnell Lu
    /// </summary>
    public void ReachEndLevel()
    {
        if (DJ_PlayerManager.playerTilePos.X == DJ_TileManagerScript.exitPoint.X &&
            DJ_PlayerManager.playerTilePos.Y == DJ_TileManagerScript.exitPoint.Y)
        {
            if (!PlayerReachedEndOfLevel)
            {
                if (DJ_LevelManager.currentLevel == 0)
                {
                    PlayerPrefs.SetInt("PlayerX", 46);
                    PlayerPrefs.SetInt("PlayerY", 79);
                    PlayerPrefs.Flush();
                }

                PlayerReachedEndOfLevel = true;
                GameObject scoreScreen = GameObject.FindGameObjectWithTag("UI_ScoreScreen");
                if (scoreScreen.GetComponent<ScoreScreenScript>().isActivated == false)
                {
                    scoreScreen.GetComponent<ScoreScreenScript>().Activate(deathCount, DJ_LevelManager.timeElapsed);
                }

                fileStars = PlayerPrefs.GetInt("stars" + DJ_LevelManager.currentLevel.ToString());

                earnedStars = 0;
                // reaching under death count
                if (deathCount <= PlayerPrefs.GetInt("maxDeaths" + DJ_LevelManager.currentLevel.ToString()))
                {
                    earnedStars++;
                }
                // reachinger under timer count
                if (DJ_LevelManager.timeElapsed <= PlayerPrefs.GetFloat("starTime" + DJ_LevelManager.currentLevel.ToString()))
                {
                    earnedStars++;
                }
                // reaching end of level
                earnedStars++;

                // if you beat your previous score.
                if (earnedStars > fileStars)
                {
                    // set to the save data
                    PlayerPrefs.SetInt("stars" + DJ_LevelManager.currentLevel.ToString(), earnedStars);
                    PlayerPrefs.Flush();
                }
                if (DJ_LevelManager.currentLevel == 0)
                {
                    PlayerPrefs.SetBool("TutorialBeaten", true);
                }
                //Application.LoadLevel("scoreScreen");
            }
        }
	}

    /// <summary>
    /// Checks to see if the player has died. Respawns the character to the correct checkpoint.
    /// </summary>
    public void RespawnCharacter()
    {
        if (!player.GetComponent<DJ_Damageable>().isAlive)
		{
			player.GetComponent<DJ_Movement>().canMove = false;
			if(prevAlive)
			{
				Camera.main.GetComponent<DeathEffectScript>().Active = true;
				Camera.main.GetComponent<DeathEffectScript>().CurrentEffectTime = 0.0f;
			}
			if(!allowRespawn && Camera.main.GetComponent<DeathEffectScript>().CurrentEffectTime > Camera.main.GetComponent<DeathEffectScript>().EffectDuration * .9f)
			{
				allowRespawn = true;
	            //DJ_LevelManager.resetScene();
                if (Application.loadedLevelName.Equals("levelSelectStage"))
                {
                    player.transform.position = new Vector3(PlayerPrefs.GetInt("PlayerX"), 0.0f, PlayerPrefs.GetInt("PlayerY"));
                }
                else
                {
                    player.transform.position = new Vector3(spawnPoint.X, 0.0f, spawnPoint.Y);
                }

	            player.gameObject.GetComponent<DJ_Movement>().targetTilePos.Set(DJ_Util.GetTilePos(player.transform.position));
	            player.gameObject.GetComponent<DJ_Movement>().currentTilePos.Set(player.gameObject.GetComponent<DJ_Movement>().targetTilePos);
                // This was breaking teleporting in the level select
	            //player.gameObject.GetComponent<DJ_Movement>().prevPrevTilePos = player.gameObject.GetComponent<DJ_Movement>().targetTilePos;
                player.gameObject.GetComponent<DJ_Movement>().prevTilePos.Set(player.gameObject.GetComponent<DJ_Movement>().targetTilePos);
				player.GetComponent<DJ_Movement>().Reset();
				player.GetComponent<DJ_Movement>().canMove = false;
	            player.GetComponent<DJ_Damageable>().isAlive = true;
                player.GetComponent<DJ_Damageable>().deathBy = DJ_Death.NONE;
	            deathCount++;
			}
			if(allowRespawn && Camera.main.GetComponent<DeathEffectScript>().CurrentEffectTime > Camera.main.GetComponent<DeathEffectScript>().EffectDuration * 2.0f * .75f)
			{
				player.GetComponent<DJ_Movement>().canMove = true;
				allowRespawn = false;
			}
        }
		prevAlive = player.GetComponent<DJ_Damageable>().isAlive;
    }
    
	public static bool isTileAvailable(DJ_Point pos)
	{
		return !playerTilePos.Equals(pos);
	}

}

