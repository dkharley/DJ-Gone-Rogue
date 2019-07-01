using UnityEngine;
using System.Collections;

public class BGManagerScript : MonoBehaviour
{
	private GameObject c_o_l;
	private Vector3 _levelCenter;

	public GameObject[] BGTileGO;

	public Camera BGCamera;
	
	public BGType _CurrBGType;

	public static BGType CurrentBGType;

	// Use this for initialization
	void Start ()
	{
		c_o_l = GameObject.Find("COL");
		
		_levelCenter = new Vector3();

		if( (c_o_l == null && _CurrBGType == BGType.Tiles) )
		{
			Debug.Log("Some backgrounds not supported.\nNo center of level GO (\"COL\") defined in the scene.\nTurning off some features.");
			_CurrBGType = BGType.Default;
		}
        
		else
		{
			_CurrBGType = c_o_l.GetComponent<DJ_BackgroundSettings>().TypeOfBG;
			_levelCenter.x = c_o_l.transform.position.x;
			_levelCenter.y = c_o_l.transform.position.y;
			_levelCenter.z = c_o_l.transform.position.z;
		}

		//instantiate the bg effect
		switch (_CurrBGType)
		{
			case BGType.Tiles:
				//BGCamera.gameObject.SetActive(false);
				CreateTileBackground();
				break;
			default:
				break;
		}
	}
	
	void Awake()
	{

	}

	//create the tile bg.
	private void CreateTileBackground()
	{
		//grab the dimension values for the current level
		float minX = DJ_TileManagerScript.MinX;
		float minZ = DJ_TileManagerScript.MinZ;
		float maxX = DJ_TileManagerScript.MaxX;
		float maxZ = DJ_TileManagerScript.MaxZ;

		_currStrength = new float[BGTileGO.Length];
		_incStrengthAmount = new float[BGTileGO.Length];
		_decStrengthAmount = new float[BGTileGO.Length];

		for(int i = 0; i < _currStrength.Length; ++i)
			_currStrength[i] = .5f;

		for(int i = 0; i < _incStrengthAmount.Length; ++i)
			_incStrengthAmount[i] = .3f;

		for(int i = 0; i < _decStrengthAmount.Length; ++i)
			_decStrengthAmount[i] = .1f;


		//set the position of the tile "folder"
		SetTilesPosition();

		float levelX = (maxX - minX) + 20.0f;
		float levelZ = (maxZ - minZ) + 20.0f;

		float levelY = 20.0f;

		int numOfBGTiles = (int)(levelX * 2.0f + levelZ  * 2.0f);

		if(_CurrBGType == BGType.Tiles)
			numOfBGTiles += (int)levelY;

		for(int i = 0; i < numOfBGTiles; ++i)
		{
			//get a random position for the tile
			float x = Random.Range(_levelCenter.x - levelX / 2.0f, _levelCenter.x + levelX / 2.0f);
			float y = Random.Range(_levelCenter.y - 10.0f - levelY, _levelCenter.y - 10.0f);
			float z = Random.Range (_levelCenter.z - levelZ / 2.0f, _levelCenter.z + levelZ / 2.0f);

			Vector3 pos = new Vector3(x, y, z);

			//get a random tile (color)
			int tileInd = Random.Range(0, BGTileGO.Length);

			//make the tile
			GameObject tile = GameObject.Instantiate(BGTileGO[tileInd]) as GameObject;
			//get the Tiles child and add the  tiles to that "folder"
			tile.transform.position = pos;
			tile.transform.parent = c_o_l.transform;
		}
	}

	Vector3 _playerFloorPos = new Vector3();

	void SetTilesPosition()
	{
		//find the lookdir of the camera
		_playerFloorPos.x = DJ_PlayerManager.player.transform.position.x;
		_playerFloorPos.y = 0.0f;
		_playerFloorPos.z = DJ_PlayerManager.player.transform.position.z;

		Vector3 lookDir = BGCamera.transform.position - _playerFloorPos;

		lookDir.x *= 3.0f;
		lookDir.z *= 3.0f;

		//get the central position of the background tiles
		Vector3 tilesPos = _levelCenter - lookDir.normalized * 10.0f;
		//set the pos of the tile "folder"
		c_o_l.transform.position = tilesPos;
	}

	Vector3 campos = new Vector3();

//	private Quaternion _rot = Quaternion.identity;
	public float[] _currStrength;
	public float _maxStrength = 1.4f;
	public float _minStrength = .3f;
	public float[] _decStrengthAmount;
	public float[] _incStrengthAmount;

	public float _amount = 1.0f;

	// Update is called once per frame
	void Update ()
	{
		CurrentBGType = _CurrBGType;

		campos.x = Camera.main.transform.position.x;
		campos.y = Camera.main.transform.position.y;
		campos.z = Camera.main.transform.position.z;

		if(_CurrBGType == BGType.Tiles)
		{
			//set position of tile "folder"
			SetTilesPosition();

			for(int i = 0; i < _currStrength.Length; ++i)
			{
				_currStrength[i] -= _decStrengthAmount[i];
			}

            if ((DJ_BeatManager.synth1 /*|| DJ_BeatManager.synth1Threshold*/) && DJ_BeatManager.layerThreeActive) 
			{
				_currStrength[0] += _incStrengthAmount[0];
				_incStrengthAmount[0] = 1.0f / DJ_BeatManager.GetNextLayerThreeOn();
				_decStrengthAmount[0] = _incStrengthAmount[0] / 100.0f;
			}
            if ((DJ_BeatManager.synth2 /*|| DJ_BeatManager.synth2Threshold*/) && DJ_BeatManager.layerFourActive)
			{
				_currStrength[1] += _incStrengthAmount[1];
				_incStrengthAmount[1] = 1.0f / DJ_BeatManager.GetNextLayerFourOn();
				_decStrengthAmount[1] = _incStrengthAmount[1] / 100.0f;
			}
            if ((DJ_BeatManager.snare /*|| DJ_BeatManager.snareThreshold*/) && DJ_BeatManager.layerTwoActive)
			{
				_currStrength[2] += _incStrengthAmount[2];
				_incStrengthAmount[2] = 1.0f / DJ_BeatManager.GetNextLayerTwoOn();
				_decStrengthAmount[2] = _incStrengthAmount[2] / 100.0f;
			}

			for(int i = 0; i < _currStrength.Length; ++i)
			{
				_currStrength[i] = Mathf.Clamp(_currStrength[i], _minStrength, _maxStrength);
			}

			//update the values in the shader
			for(int i = 0; i < _currStrength.Length; ++i)
				BGTileGO[i].GetComponent<Renderer>().sharedMaterial.SetFloat("_GlowStrength", _currStrength[i]);

			//_rot.y += .00005f;

			//c_o_l.transform.rotation = _rot;
            campos.y = -campos.y;
			BGCamera.transform.position = campos;
			BGCamera.transform.localRotation = Camera.main.transform.localRotation;
            //Quaternion _r = BGCamera.transform.localRotation;
            //float dist = 180 - _r.x;
            //_r.x = 180 + dist;
            //BGCamera.transform.localRotation = _r;
            Vector3 _target = DJ_PlayerManager.player.transform.position;
            _target.y = 0.0f;
            BGCamera.transform.rotation = Quaternion.LookRotation(_target - BGCamera.transform.position);
            //Quaternion _r2 = BGCamera.transform.rotation;
            //float _dist2 = 180 - _r2.x;
            //_r2.x = 180 + _dist2;
            //BGCamera.transform.rotation = _r2;
			BGCamera.fieldOfView = Camera.main.fieldOfView;

			Camera.main.clearFlags = CameraClearFlags.Nothing;
		}
		else
		{
			Camera.main.clearFlags = CameraClearFlags.Nothing;
		}
	}
}
