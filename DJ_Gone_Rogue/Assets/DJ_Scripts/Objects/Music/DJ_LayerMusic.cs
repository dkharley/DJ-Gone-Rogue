using UnityEngine;
using System.Collections;

public class DJ_LayerMusic : MonoBehaviour {

    public bool layer1;
    public bool layer2;
    public bool layer3;
    public bool layer4;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(DJ_Tag.DJ_Player.ToString()))
        {

            if (layer1)
                DJ_BeatManager.ActivateLayerOne();
            if (layer2)
                DJ_BeatManager.ActivateLayerTwo();
            if (layer3)
                DJ_BeatManager.ActivateLayerThree();
            if (layer4)
                DJ_BeatManager.ActivateLayerFour();
            gameObject.SetActive(false);
        }
    }
}
