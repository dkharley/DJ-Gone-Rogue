using UnityEngine;
using System.Collections;

public class DJ_LevelSelectTeleporter : MonoBehaviour {

    private DJ_Point tilePos;
    public int recieverNumber;
    public GameObject teleportTo;

    public GameObject Ring;
    private LightRing TPRings;

	// Use this for initialization
	void Start () {
        tilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        TPRings = Ring.GetComponent<LightRing>();
	}
	
	// Update is called once per frame
	void Update () {
        if (DJ_TileManagerScript.tileMap.ContainsKey(tilePos))
        {
            if (DJ_PlayerManager.playerTilePos.X == tilePos.X && DJ_PlayerManager.playerTilePos.Y == tilePos.Y && !DJ_PlayerManager.player.GetComponent<DJ_Movement>().isLerping)
            {
                if ((TPRings != null && TPRings.BlastOff))
                {
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().canMove = true;
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = DJ_Dir.TP;
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.X = Mathf.RoundToInt(teleportTo.gameObject.transform.position.x);
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().targetTilePos.Y = Mathf.RoundToInt(teleportTo.gameObject.transform.position.z);
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = 10;
                }
            }
        }
	}
}
