using UnityEngine;
using System.Collections;

public class DJ_ElectricShock : MonoBehaviour {

    private bool onBeat;
    public Material safeShader, dangerShader;

    // determines whether the music layer is on
    private bool layerOn;

	// Use this for initialization
	void Start () {

        /*
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        */

	}
	
	// Update is called once per frame
	void Update () {

        /*
        layerOn = DJ_Util.isLayerOn(gameObject.GetComponent<DJ_BeatActivation>());
        if (layerOn)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        onBeat = DJ_Util.activateWithNoSound(gameObject.GetComponent<DJ_BeatActivation>());
        */

        onBeat = DJ_Util.activateWithSound(gameObject.GetComponent<DJ_BeatActivation>());
        if (onBeat)
        {
            if (gameObject.GetComponent<Collider>().enabled)
            {
                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponent<Renderer>().material = safeShader;
                //gameObject.renderer.material
            }
            else
            {
                gameObject.GetComponent<Collider>().enabled = true;
                gameObject.GetComponent<Renderer>().material = dangerShader;
            }
        }
	}

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(DJ_Tag.DJ_Player.ToString()) && !DJ_PlayerManager.player.GetComponent<DJ_Movement>().isLerping) 
        {
            if (col.gameObject != this)
                col.gameObject.GetComponent<DJ_Damageable>().isAlive = false;
        }
    }
}
