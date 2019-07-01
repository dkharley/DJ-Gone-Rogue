using UnityEngine;
using System.Collections;

/// <summary>
/// DJ_DJGoneRogueGame.cs: This script is responsible for instantiating
/// all of the managers correctly and in order. 
/// 
/// @author - Fernando Carrillo 1/23/2014
/// @Repo Clean - Donnell Lu 5/29/2014
/// </summary>
public class DJ_DJGoneRogueGame : MonoBehaviour {
	//make sure the right scripts are added to these names in the editor
    public GameObject m_DJ_LevelManager,
                        m_DJ_TileManager,
                        m_DJ_BGManager,
                        m_DJ_PlayerManager,
                        m_DJ_CameraManager,
                        m_DJ_EffectsManager,
                        m_DJ_UIManager,
                        m_DJ_LoadingManager,
                        m_DJ_BeatManager;
    
    // These are for the BeatManager. 
    // @author - Peter Kong 5/7/2014
    public string songname;
    public float bpm;


	// Use this for initialization
	void Start ()
	{

        InstantiateManager(m_DJ_BeatManager);

		//Makes and Calls the DJ_Level Manager
		InstantiateManager (m_DJ_LevelManager);

		//Makes and Calls the DJ_Tile Manager
        InstantiateManager(m_DJ_TileManager);

		//Makes and Calls the DJ_Player Manager
		InstantiateManager (m_DJ_PlayerManager);

        //Makes and Calls the DJ_Camera Manager
		InstantiateManager (m_DJ_CameraManager);

        //Makes and Calls the DJ_Effects Manager
		InstantiateManager (m_DJ_EffectsManager);

        if (Application.platform != RuntimePlatform.Android ||
            Application.platform != RuntimePlatform.IPhonePlayer ||
            Application.platform != RuntimePlatform.WP8Player)
        {
            //InstantiateManager(m_DJ_BGManager);
        }
       
#if UNITY_EDITOR
        //InstantiateManager(m_DJ_BGManager);
#endif

        InstantiateManager(m_DJ_UIManager);


        InstantiateManager(m_DJ_LoadingManager);
	}

	void InstantiateManager (GameObject managerPrefab)
	{
		GameObject manager = (GameObject.Instantiate (managerPrefab)) as GameObject;
		manager.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}