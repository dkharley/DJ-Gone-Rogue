using UnityEngine;
using System.Collections;

public class DJ_Spring : MonoBehaviour {

    public DJ_Dir direction;
    public float height;
    public int distance;
    private bool onBeat;
    public Material safeShader, dangerShader;
	private DJ_Flipper _flipper;

	// Use this for initialization
	void Start () {
		_flipper = gameObject.GetComponentInChildren<DJ_Flipper>();
	}
	
	// Update is called once per frame
	void Update () {
        // spring activates on beat
        onBeat = DJ_Util.activateWithSound(gameObject.GetComponent<DJ_BeatActivation>());
        if (onBeat)
        {
			_flipper.Flip();
            DJ_Point tilePos = new DJ_Point(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
            if (DJ_TileManagerScript.tileMap.ContainsKey(tilePos))
            {
                if (DJ_PlayerManager.playerTilePos.X == tilePos.X && DJ_PlayerManager.playerTilePos.Y == tilePos.Y && !DJ_PlayerManager.player.GetComponent<DJ_Movement>().isLerping)
                {
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().heightOfHop = height;
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().direction = direction;
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().maxMoveDistance = distance;
                    DJ_PlayerManager.player.GetComponent<DJ_Movement>().canMove = true;
                }
            }
            gameObject.GetComponentInChildren<MeshRenderer>().GetComponent<Renderer>().material = dangerShader;
        }
        else
        {
            gameObject.GetComponentInChildren<MeshRenderer>().GetComponent<Renderer>().material = safeShader;
        }
	}
}
