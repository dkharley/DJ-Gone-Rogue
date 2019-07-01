using UnityEngine;
using System.Collections;

/// <summary>
/// D j_ tile script. Characterizes the tile entity. Extends DJ_EntityScript.
/// 
/// @author Wyatt Sanders 1/9/2014
/// </summary>
public class DJ_TileScript : DJ_EntityScript
{
    // Level selection stuff. Doesn't matter if levelselect is false



	public bool startVisible;

    public System.Collections.Generic.List<Texture> m_tileTextures;

	public bool spawnTile, tutorialTile, checkpointTile, exitTile, fadingTile, teleportTile, teleportPad;

    public bool recieverPad, levelSelectTeleporterTile;
    public int recieverNumber;

    public bool levelselectTile;

    public int level;
    public int stars;
    public int difficulty;

	public bool layer1, layer2, layer3, layer4;

    private int m_LifeRemaining;

    //this is the mesh that represents the visual aspect of the tile
    //this should only be used to reflect the visual state of the tile
    private Transform m_tileMeshTransform;

    // determines whether the music layer is on
    private bool layerOn;

    Vector3 currentPosition;

    public bool neighboringTile;
    public bool normalTile;
    public bool currentTile;

    private float timerDelayMax;
    private float currentTimer;
    private bool startDelayTimer;

    public bool fadeIn;
    public Color originalColor;
    public float fadeInSpeed;

    // Speed of the movement between tiles, adjusted via beat
    public float animationLength;

    public float currAnimationTime = 0.0f;

    // Used only for a reference within this class to avoid a long statement.
    private Material tileMaterial;

    private bool onBeat;
    private bool activateNextBeat;
    private MeshRenderer _meshRender;

    private bool on;

	public DJ_TileScript()
		: base()
	{
        fadeInSpeed = 7;
        onBeat = false;
        // This should be .1 seconds
        timerDelayMax = 0.1f;
	}

	// Use this for initialization
	public override void Start ()
	{
		base.Start();

        fadeIn = false;

        m_tileMeshTransform = transform.GetChild(0);

		//rotate the tile so that it faces up. This is the only
		//entity that does not use the cameras forward to determine
		//its orientation.
		transform.rotation = Quaternion.FromToRotation(transform.up, new Vector3(0.0f, 1.0f, 0.0f));

		//change the position of the attached cube mesh so that the origin/position of the tile
		//object is at the center of the surface of the mesh
		m_tileMeshTransform.position =
			new Vector3(m_tileMeshTransform.position.x,
			            m_tileMeshTransform.position.y - (m_tileMeshTransform.GetComponent(typeof(MeshRenderer)) as MeshRenderer).bounds.size.y / 2f,
			            m_tileMeshTransform.position.z);

		int x = Random.Range(0, m_tileTextures.Count);
        tileMaterial = (transform.GetChild(0).GetComponent(typeof(MeshRenderer)) as MeshRenderer).material;
        tileMaterial.mainTexture = m_tileTextures[x];

		currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        originalColor = tileMaterial.color;

		direction = DJ_Dir.NONE;

        _meshRender = gameObject.GetComponentInChildren<MeshRenderer>();

        if (fadingTile || !startVisible)
        {
            _meshRender.enabled = false;
        }
	}

	// Update is called once per frame
	public override void Update ()
	{
        // GLOW when laser hovers over
        
        if (_meshRender.GetComponent<Renderer>().material.GetFloat("_GlowStrength") > .5f)
        {
            float gStrength = transform.GetChild(0).GetComponent<MeshRenderer>().GetComponent<Renderer>().sharedMaterial.GetFloat("_GlowStrength");
            //tileMaterial.color = Color.Lerp(tileMaterial.color, originalColor, fadeInSpeed * Time.deltaTime);
            float gs = Mathf.Lerp(gStrength, 1.0f, 3 * Time.deltaTime);
            _meshRender.GetComponent<Renderer>().material.SetFloat("_GlowStrength", gs);

        }
        else
        {
            _meshRender.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.5f);
        }
        

		base.Update();
        if (!gameObject.GetComponent<DJ_Damageable>().invulnerable)
        {
            onBeat = DJ_Util.activateWithNoSound(gameObject.GetComponent<DJ_BeatActivation>());
            if (onBeat)
            {
                //DJ_TileManagerScript.tilePool.Push(DJ_TileManagerScript.tileMap[new DJ_Point((int)transform.position.x, (int)transform.position.z)]);
                //DJ_TileManagerScript.tileMap.Remove(new DJ_Point((int)transform.position.x, (int)transform.position.z));
                //gameObject.SetActive(false);
                startDelayTimer = true;
            }
        }
        if (startDelayTimer)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > timerDelayMax)
            {
                DJ_TileManagerScript.tilePool.Push(DJ_TileManagerScript.tileMap[new DJ_Point((int)transform.position.x, (int)transform.position.z)]);
                DJ_TileManagerScript.tileMap.Remove(new DJ_Point((int)transform.position.x, (int)transform.position.z));
                gameObject.SetActive(false);
                startDelayTimer = false;
                currentTimer = 0;
            }
        }

            
	    //save the current position of the GO so that we  can
	    //modify it and  set the transform.position equal to
	    //the  modified position
	    currentPosition = transform.position;

        if (fadeIn)
        {
            if (fadingTile)
            {
                if (transform.GetChild(0).GetComponent<MeshRenderer>().enabled == false)
                {

                    if (DJ_Util.isLayerOn(gameObject.GetComponent<DJ_BeatActivation>()))
                    {
                        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }

            float currAlpha = transform.GetChild(0).GetComponent<MeshRenderer>().GetComponent<Renderer>().sharedMaterial.GetFloat("_Alpha");
            //tileMaterial.color = Color.Lerp(tileMaterial.color, originalColor, fadeInSpeed * Time.deltaTime);
            float a = Mathf.Lerp(currAlpha, 1.0f, fadeInSpeed * Time.deltaTime);

            _meshRender.GetComponent<Renderer>().sharedMaterial.SetFloat("_Alpha", a);
            
			if (currAlpha == 1)
            {
				fadeIn = false;
			}
		}

        transform.position = currentPosition;
	}

	public override void Reset ()
	{
		base.Reset ();
	}
}
