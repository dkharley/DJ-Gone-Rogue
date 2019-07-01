using UnityEngine;
using System.Collections;

public class DJ_TeleporterTile : MonoBehaviour {

    public int teleporterPad;
    private bool onBeat;
    public GameObject teleporter;

    public bool instantTeleport;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        // Used for instant teleporting in the level select
        if (instantTeleport)
        {
            DJ_Point tilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
            if (DJ_TileManagerScript.tileMap.ContainsKey(tilePos))

            {
                if (DJ_PlayerManager.playerTilePos.X == tilePos.X && DJ_PlayerManager.playerTilePos.Y == tilePos.Y && !DJ_PlayerManager.player.GetComponent<DJ_Movement>().isLerping)
                {
                    if ( !(DJ_PlayerManager.player.GetComponent<DJ_Movement>().prevPrevTilePos.X == teleporter.gameObject.transform.position.x &&
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().prevPrevTilePos.Y == teleporter.gameObject.transform.position.z))
                    {
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = DJ_Dir.TP;
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.X = Mathf.RoundToInt(teleporter.gameObject.transform.position.x);
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.Y = Mathf.RoundToInt(teleporter.gameObject.transform.position.z);
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = 14;
                    }
                }
            }
        }
          
        // Used for teleporting during gameplay
        else
        {
            onBeat = DJ_Util.activateWithSound(gameObject.GetComponent<DJ_BeatActivation>());
            if (onBeat)
            {
                DJ_Point tilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
                if (DJ_TileManagerScript.tileMap.ContainsKey(tilePos))
                {
                    if (DJ_PlayerManager.playerTilePos.X == tilePos.X && DJ_PlayerManager.playerTilePos.Y == tilePos.Y && !DJ_PlayerManager.player.GetComponent<DJ_Movement>().isLerping)
                    {
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = DJ_Dir.TP;
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.X = Mathf.RoundToInt(teleporter.gameObject.transform.position.x);
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.Y = Mathf.RoundToInt(teleporter.gameObject.transform.position.z);
                        DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = 8;
                    }
                }
            }
        }
    }


}
