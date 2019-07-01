using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// D j_ tile manager script. This script is responsible for managing
/// all the active and inactive tiles currently in the level and is
/// responsible for interfacing with other managers.
/// 
/// @author - Wyatt Sanders 1/9/2014
/// @edited - Fernando Carrillo 1/23/2014
/// @editted - Donnell Lu 2/9/2014
/// @Repo clean - Donnell Lu 5/29/2014
/// </summary>
public class DJ_TileManagerScript : MonoBehaviour
{

    public static Dictionary<int, List<GameObject>> teleporterTiles;// = new Dictionary<int, List<GameObject>>();
    public static Dictionary<int, GameObject> teleporterPads;// = new Dictionary<int, GameObject>();


    // Key refers to an integer that teleporterTiles will send them too.
    // Value refers to the actual tile object to get the position
    public static Dictionary<int, GameObject> receiverTile;
    public static Dictionary<int, List<GameObject>> levelSelectTeleporterTile;

	public static TileGraph tileGraph;
    private bool onBeat;

    public GameObject breakEffect;

    //float beatTime = 2.0f;
    //bool inEditorMode = false;

    public static List<DJ_Point> nearbyTiles = new List<DJ_Point>();

    /// <summary>
    /// The tile prefab.
    /// </summary>
    public GameObject m_tilePrefab, m_wallPrefab;

    private Texture[] m_tileTextures;

    /// <summary>
    /// The stack of pooled tiles that need o be reactivated after they disappear.
    /// </summary>
    public static Stack<TileNode> tilePool;

    /// <summary>
    /// The tile map. A dictionary of all the active tiles in the level.
    /// </summary>
    public static Dictionary<DJ_Point, TileNode> tileMap;

    /// <summary>
    /// A list of DJ_Points for checkpoints.
    /// </summary>
    public static List<DJ_Point> checkpointList;
    public static DJ_Point exitPoint;
    public static List<DJ_Point> levelselectList;

	public static float MinX = float.MaxValue,
						MinZ = float.MaxValue,
						MaxX = float.MinValue,
						MaxZ = float.MinValue;

	public static List<DJ_Point> _recentlyVisitedTiles;
    public static List<float> _tileGlowTimes;


    public static List<DJ_Point> _recentlyLaserVisitedTiles;
    public static Dictionary<DJ_Point, float> _tileLaserGlowTimes;

	// Called only once during it's lifetime.
	public void Awake()
	{
		
	}
	
	// Use this for initialization
	public void Start ()
	{
        teleporterTiles = new Dictionary<int, List<GameObject>>();
        teleporterPads = new Dictionary<int, GameObject>();
        receiverTile = new Dictionary<int, GameObject>();
        levelSelectTeleporterTile = new Dictionary<int, List<GameObject>>();
//		MinX = float.MaxValue;
//		MinZ = float.MaxValue;
//		MaxX = float.MinValue;
//		MaxZ = float.MinValue;

		//this is only going tobe attached to an empty object but center
		//it at the origin anyway
		gameObject.transform.position = Vector3.zero;

		_recentlyVisitedTiles = new List<DJ_Point>();
		_tileGlowTimes = new List<float>();

        _recentlyLaserVisitedTiles = new List<DJ_Point>();
        _tileLaserGlowTimes = new Dictionary<DJ_Point,float>();

		_prevPlayerPos = new DJ_Point(int.MaxValue, int.MaxValue);

		tilePool = new Stack<TileNode>();
		tileMap = new Dictionary<DJ_Point, TileNode>(DJ_PointComparer.Default);

        // Instantiate the checkpoint system
        checkpointList = new List<DJ_Point>();
        exitPoint = new DJ_Point(0, 0);
        levelselectList = new List<DJ_Point>();
        onBeat = false;

        //Instantiate level select points
		tileGraph = new TileGraph(this.gameObject, m_tilePrefab);
        LinkTeleporters();
        LinkLevelSelectTeleporters();
        tileMap = tileGraph.tileMap;
		//print (tilegraph);
	}

	private static DJ_Point _prevPlayerPos;

	// Update is called once per frame
	public void Update ()
	{	
		//get a ref to the player's tile position
        if (DJ_PlayerManager.player != null)
        {
            DJ_Point playerTilePos = (DJ_PlayerManager.player.GetComponent(typeof(DJ_Movement)) as DJ_Movement).currentTilePos;

//			if(DJ_PlayerManager.player.GetComponent<DJ_Movement>().justLanded)
//			{
//				if(tileMap.ContainsKey[playerTilePos])
//					tileMap [playerTilePos].tile.transform.GetChild(0).renderer.material.SetFloat("_GlowStrength", 1.5f);
//			}

			DJ_Movement _movement = DJ_PlayerManager.player.GetComponent<DJ_Movement>();
			if(_movement.justLanded && DJ_PlayerManager.player.transform.position.y >= 0.0f)
			{
				if(tileMap.ContainsKey(playerTilePos))
				{
					DJ_Point newPos = new DJ_Point(playerTilePos.X, playerTilePos.Y);

					_recentlyVisitedTiles.Add(newPos); //set the time remaining of the effect
					_tileGlowTimes.Add(1.5f);

					_prevPlayerPos.X = playerTilePos.X;
					_prevPlayerPos.Y = playerTilePos.Y;
				}
			}
        }
        
		for(int i = 0; i < _recentlyVisitedTiles.Count; ++i)
		{
			DJ_Point key = _recentlyVisitedTiles[i];

			if(_tileGlowTimes[i] < 0.0f)
			{
				if(tileMap.ContainsKey(key))
					tileMap[key].tile.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_GlowStrength", .5f);
				_recentlyVisitedTiles.RemoveAt(i);
				_tileGlowTimes.RemoveAt(i);
			}
			else
			{
				float _time = _tileGlowTimes[i];
                //Debug.Log("_time = " + _time);
				if(tileMap.ContainsKey(key))
					tileMap[key].tile.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_GlowStrength", .5f + 5.5f * (_time / 1.5f));
                //Debug.Log("_time = " + _time);
                _time -= Time.deltaTime;
				_tileGlowTimes[i] = _time;
			}
		}
        
        /*
        for (int j = 0; j < _recentlyLaserVisitedTiles.Count; ++j)
        {
            DJ_Point key = _recentlyLaserVisitedTiles[j];
            if (_tileLaserGlowTimes.ContainsKey(key))
            {
                if (_tileLaserGlowTimes[key] < 0.0f)
                {
                    if (tileMap.ContainsKey(key))
                        tileMap[key].tile.transform.GetChild(0).renderer.material.SetFloat("_GlowStrength", .5f);
                    _recentlyLaserVisitedTiles.RemoveAt(j);
                    _tileLaserGlowTimes.Remove(key);
                }
                else
                {
                    float _time = _tileLaserGlowTimes[key];
                    if (tileMap.ContainsKey(key))
                        tileMap[key].tile.transform.GetChild(0).renderer.material.SetFloat("_GlowStrength", .5f + 5.5f * (_time / 1.5f));

                    _time -= Time.deltaTime;
                    //Debug.Log("time is = " + _time);
                    _tileLaserGlowTimes[key] = _time;
                }
            }
        }
        */

        RespawnTile();

		//render debug data about the tile graph
		//tileGraph.Render();
	
	}

    // TODO: DELETION?
    public void RespawnTile()
    {
        onBeat = DJ_Util.activateWithNoSound(gameObject.GetComponent<DJ_BeatActivation>());
        if (onBeat)
        {
            while (tilePool.Count > 0)
            {
                TileNode poolTile = tilePool.Pop();
                poolTile.tile.SetActive(true);

                tileMap.Add(new DJ_Point((int)poolTile.tile.transform.position.x, (int)poolTile.tile.transform.position.z), poolTile);
                poolTile.tile.GetComponent<DJ_TileScript>().fadeIn = true;
                (poolTile.tile.transform.GetChild(0).GetComponent(typeof(MeshRenderer)) as MeshRenderer).GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", 0.0f);
                (poolTile.tile.transform.GetChild(0).GetComponent(typeof(MeshRenderer)) as MeshRenderer).GetComponent<Renderer>().sharedMaterial.SetFloat("_GlowStrength", 0.5f);
                
            }
        }

    }
	
	/// <summary>
	/// Resolves the player tile collision. 
	/// Used to break tiles the player is on during the beat.
	/// Used to break tiles that the player lands on.
	/// </summary>
	/// <param name="playerTilePos">Player tile position.</param>
	
	
	public static void Dispose ()
	{

	}

	//function for determining if tile exists
	public static bool isTileAvailable (DJ_Point pos)
	{
		return tileMap.ContainsKey(pos);
	}	
	
	/// <summary>
	/// Accesses the tiles that are inside of DJ_LevelParser to instantiate tiles. 
	/// </summary>
	/// <param name="_num">_num.</param>
	
    // Debug: Highlights adjacent tiles green.
	void getTilesNearLocation (DJ_Point playerTilePos)
	{
		//if the player has moved tiles, we need to update the closest ones
		if (!DJ_PlayerManager.prevPlayerTilePos.Equals (playerTilePos))
		{	
			//reset the tile broke flag
			//tileBroke = false;
			
			DJ_PlayerManager.prevPlayerTilePos.X = playerTilePos.X;
			DJ_PlayerManager.prevPlayerTilePos.Y = playerTilePos.Y;
			
			//now get a ref to  the tiles around the  player
			//getNearbyTiles (playerTilePos, nearbyTiles, 1.0f);
			DJ_Util.GetNearbyTiles(playerTilePos, 1.0f, ref nearbyTiles);
			
			//Debugging view to see if tiles update correctly
			//			for (int i = 0; i < nearbyTiles.Count; ++i) {
			//				DJ_Point p = nearbyTiles [i];
			//				if (tileMap.ContainsKey (p)) {
			//					GameObject tile = tileMap [p];
			//					//(tile.GetComponentInChildren(typeof(MeshRenderer)) as MeshRenderer).material.color = Color.green;
			//				}
			//			}
			//(tileMap[playerTilePos].GetComponentInChildren(typeof(MeshRenderer)) as MeshRenderer).material.color = Color.red;
		}
	}

    private void LinkTeleporters()
    {
        for (int i = 0; i < teleporterTiles.Count; i++)
        {
            for (int j = 0; j < teleporterTiles[i].Count; j++)
            {
                if (teleporterTiles[i][j].GetComponent<DJ_BeatActivation>().instrument1 || teleporterTiles[i][j].GetComponent<DJ_BeatActivation>().instrumentThreshold1)
                {
                    teleporterPads[i].GetComponent<DJ_TeleporterPad>().layerOneTile = teleporterTiles[i][j];
                    teleporterTiles[i][j].GetComponent<DJ_TeleporterTile>().teleporter = teleporterPads[i];
                }
                else if (teleporterTiles[i][j].GetComponent<DJ_BeatActivation>().instrument2 || teleporterTiles[i][j].GetComponent<DJ_BeatActivation>().instrumentThreshold2)
                {
                    teleporterPads[i].GetComponent<DJ_TeleporterPad>().layerTwoTile = teleporterTiles[i][j];
                    teleporterTiles[i][j].GetComponent<DJ_TeleporterTile>().teleporter = teleporterPads[i];
                }
                else if (teleporterTiles[i][j].GetComponent<DJ_BeatActivation>().instrument3 || teleporterTiles[i][j].GetComponent<DJ_BeatActivation>().instrumentThreshold3)
                {
                    teleporterPads[i].GetComponent<DJ_TeleporterPad>().layerThreeTile = teleporterTiles[i][j];
                    teleporterTiles[i][j].GetComponent<DJ_TeleporterTile>().teleporter = teleporterPads[i];
                }
                else if (teleporterTiles[i][j].GetComponent<DJ_BeatActivation>().instrument4 || teleporterTiles[i][j].GetComponent<DJ_BeatActivation>().instrumentThreshold4)
                {
                    teleporterPads[i].GetComponent<DJ_TeleporterPad>().layerFourTile = teleporterTiles[i][j];
                    teleporterTiles[i][j].GetComponent<DJ_TeleporterTile>().teleporter = teleporterPads[i];
                }
            }
        }
    }

    private void LinkLevelSelectTeleporters()
    {
        //Debug.Log("levelSelectTeleporterTile count = " + levelSelectTeleporterTile.Count);
        foreach (var i in levelSelectTeleporterTile)
        {
            //Debug.Log("levelSelectTeleporterTile count at "  + i + " = " + levelSelectTeleporterTile[i].Count);
            foreach (var j in i.Value)
            {
                j.GetComponent<DJ_LevelSelectTeleporter>().teleportTo = receiverTile[j.GetComponent<DJ_LevelSelectTeleporter>().recieverNumber];
            }
        }
    }
}