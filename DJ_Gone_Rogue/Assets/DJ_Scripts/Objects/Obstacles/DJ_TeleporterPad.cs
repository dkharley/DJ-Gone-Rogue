using UnityEngine;
using System.Collections;

public class DJ_TeleporterPad : MonoBehaviour {

    private DJ_Instrument onBeat;
    public GameObject layerOneTile, layerTwoTile, layerThreeTile, layerFourTile;
    public int teleportPadNumber;
    public int heightOfHop;
    public bool returnOnly;

    public bool instantTeleport;

    public GameObject Ring;

    private LightRing TPRings;

	// Use this for initialization
	void Start () 
    {
        heightOfHop = 8;
        TPRings = Ring.GetComponent<LightRing>();
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
                    if (layerOneTile != null)
                    {
                        if ( 
                            //!(DJ_PlayerManager.player.GetComponent<DJ_Movement>().prevPrevTilePos.X == layerOneTile.transform.position.x &&
                            //DJ_PlayerManager.player.GetComponent<DJ_Movement>().prevPrevTilePos.Y == layerOneTile.transform.position.z ) &&
                            (TPRings != null && TPRings.BlastOff) )
                        {
                            DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = DJ_Dir.TP;
                            DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.X = Mathf.RoundToInt(layerOneTile.transform.position.x);
                            DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.Y = Mathf.RoundToInt(layerOneTile.transform.position.z);
                            DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = 14;
                        }
                    }
                }
            }
        }
        // Used for teleporting during gameplay
        else
        {
            if (!returnOnly)
            {
                DJ_Point tilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
                if (DJ_TileManagerScript.tileMap.ContainsKey(tilePos))
                {
                    if (DJ_PlayerManager.playerTilePos.X == tilePos.X && DJ_PlayerManager.playerTilePos.Y == tilePos.Y && !DJ_PlayerManager.player.GetComponent<DJ_Movement>().isLerping)
                    {
                        onBeat = DJ_Util.activateWithSoundWithInstrument(gameObject.GetComponent<DJ_BeatActivation>());
                        switch (onBeat)
                        {
                            case DJ_Instrument.NONE:
                                break;
                            case DJ_Instrument.ONE:
                                if (layerOneTile != null)
                                {
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = DJ_Dir.TP;

                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.X = Mathf.RoundToInt(layerOneTile.transform.position.x);
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.Y = Mathf.RoundToInt(layerOneTile.transform.position.z);
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = 8;
                                }
                                break;
                            case DJ_Instrument.TWO:
                                if (layerTwoTile != null)
                                {
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = DJ_Dir.TP;
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.X = Mathf.RoundToInt(layerTwoTile.transform.position.x);
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.Y = Mathf.RoundToInt(layerTwoTile.transform.position.z);
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = 8;
                                }
                                break;
                            case DJ_Instrument.THREE:
                                if (layerThreeTile != null)
                                {
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = DJ_Dir.TP;

                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.X = Mathf.RoundToInt(layerThreeTile.transform.position.x);
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.Y = Mathf.RoundToInt(layerThreeTile.transform.position.z);
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = 8;
                                }
                                break;
                            case DJ_Instrument.FOUR:
                                if (layerFourTile != null)
                                {
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = DJ_Dir.TP;

                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.X = Mathf.RoundToInt(layerFourTile.transform.position.x);
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.Y = Mathf.RoundToInt(layerFourTile.transform.position.z);
                                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = 8;
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
