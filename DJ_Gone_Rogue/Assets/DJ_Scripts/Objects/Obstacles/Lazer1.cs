using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class Lazer1 : MonoBehaviour
{
    public GameObject laserGO;

    /// <summary>
    /// The walls that the laser will come out of.
    /// </summary>
    public GameObject Wall1, Wall2;

    public float heightOffGround;

    /// <summary>
    /// the length of the gap between walls. this is where the laser
    /// will be.
    /// </summary>
    public float gapDist;

    /// <summary>
    /// the speed at which this effect takes place.
    /// </summary>
    public float animationTime;

    /// <summary>
    /// Gameobjects that represent the start position and end position
    /// of the animation. Only visible in the editor.
    /// </summary>
    public GameObject startPosGO, endPosGO;

    public Transform lightSource, target;

    public Vector3 startPos, endPos;

    public float currAnimationTime = 0.0f;

    public bool easeOut = false;
    public bool easeIn = false;

    private bool onBeat, moving;

    private float timeTillNextBeat;

    private bool layerOn;

    private DJ_Point laserTilePos = new DJ_Point(0, 0);


    public Vector3 Position;

    public Vector3 Dir
    {
        get { return (endPos - startPos).normalized; }
    }

    // Use this for initialization
    void Start()
    {
        laserGO.transform.parent = this.transform;
        Wall1.transform.parent = this.transform;
        Wall2.transform.parent = this.transform;

        //Position = (endPos + startPos) / 2;
        Position = startPos;
        moving = false;
        timeTillNextBeat = 0;
        laserGO.GetComponent<MeshRenderer>().enabled = false;
        laserGO.GetComponent<CapsuleCollider>().enabled = false;
        lightSource.GetComponent<ParticleRenderer>().enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        Animate();
    }

    //void OnEnable()
    //{
    //    //UnityEditor.EditorApplication.update += Animate;
    //    laserGO.transform.parent = this.transform;
    //    Wall1.transform.parent = this.transform;
    //    Wall2.transform.parent = this.transform;

    //    Position = (EndPos + StartPos) / 2;

    //    Debug.Log("Enabling lazer animation");
    //}

    //void OnDisable()
    //{
    //    UnityEditor.EditorApplication.update -= Animate;
    //    Debug.Log("Disabling lazer animation");
    //}

    /// <summary>
    /// do the in editor animation
    /// </summary>
    public void Animate()
    {
        //calc the vector perpendicular to the direction of the animation path
        Vector3 right = Vector3.Cross(Dir, Vector3.up);

        //change the positions of the walls based on the gap distance
        //and offset part of the scale
        Wall1.transform.position = Position - right * (gapDist / 2 + Wall1.transform.localScale.x / 2)
                                   + Vector3.up * heightOffGround;
        Wall2.transform.position = Position + right * (gapDist / 2 + Wall2.transform.localScale.x / 2)
                                   + Vector3.up * heightOffGround;

        lightSource.position = Wall1.transform.position;
        target.position = Wall2.transform.position;

        //make sure the laser object is centered between the walls
        laserGO.transform.position = (Wall2.transform.position + Wall1.transform.position) / 2;


        //set the scale of the laser so that it spans the distance betweens the  walls
        Vector3 scale = laserGO.transform.localScale;

        scale.y = (Vector3.Distance(Wall1.transform.position, Wall2.transform.position) - Wall1.transform.localScale.x) / 2;

        laserGO.transform.localScale = scale;

        laserTilePos.X = Mathf.RoundToInt(laserGO.transform.position.x);
        laserTilePos.Y = Mathf.RoundToInt(laserGO.transform.position.z);

        layerOn = DJ_Util.isLayerOn(gameObject.GetComponent<DJ_BeatActivation>());
        if (layerOn)
        {
            laserGO.GetComponent<MeshRenderer>().enabled = true;
            laserGO.GetComponent<CapsuleCollider>().enabled = true;
            lightSource.GetComponent<ParticleRenderer>().enabled = true;
        }

        onBeat = DJ_Util.activateWithNoSound(gameObject.GetComponent<DJ_BeatActivation>());
        if (moving == false && onBeat == true)
        {
            moving = true;

            if (gameObject.GetComponent<DJ_BeatActivation>().instrument1)
            {
                timeTillNextBeat = DJ_BeatManager.GetNextLayerOneOn();
            }
            else if (gameObject.GetComponent<DJ_BeatActivation>().instrument2)
            {
                timeTillNextBeat = DJ_BeatManager.GetNextLayerTwoOn();
            }
            else if (gameObject.GetComponent<DJ_BeatActivation>().instrument3)
            {
                timeTillNextBeat = DJ_BeatManager.GetNextLayerThreeOn();
            }
            else if (gameObject.GetComponent<DJ_BeatActivation>().instrument4)
            {
                timeTillNextBeat = DJ_BeatManager.GetNextLayerFourOn();
            }
        }
        
        if(moving)
        {
            currAnimationTime += Time.deltaTime;
            // Go to starting Position
            if (easeOut)
            {
                Position = Vector3.Lerp(Position, startPos, currAnimationTime * timeTillNextBeat);
                if (Vector3.Distance(Position, startPos) < .001f)
                //if (currAnimationTime > animationTime)
                {
                    currAnimationTime = 0.0f;
                    easeOut = false;
                    easeIn = true;
                    Position = startPos;
                    moving = false;
                }

            }
            // go to ending position
            else if (easeIn)
            {
                Position = Vector3.Lerp(Position, endPos, currAnimationTime * timeTillNextBeat);
                if (Vector3.Distance(Position, endPos) < .001f)
                //if (currAnimationTime > animationTime)
                {
                    currAnimationTime = 0.0f;
                    easeOut = true;
                    easeIn = false;
                    Position = endPos;
                    moving = false;
                }
            }
        }

        // GLOW when laser hovers over
        
        if (DJ_TileManagerScript.tileMap.ContainsKey(laserTilePos))
        {
            if (laserGO.GetComponent<CapsuleCollider>().enabled)
            {
                DJ_TileManagerScript.tileMap[laserTilePos].tile.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_GlowStrength", 10.5f);
            }
        }
        
        /*
        if (prevLaserTilePos.X > laserTilePos.X)
        {
            int distanceX = prevLaserTilePos.X - laserTilePos.X;
            if (distanceX > 1)
            {
                Debug.Log("distanceX1 = " + distanceX);
                for (int j = 0; j < distanceX; j++)
                {
                    DJ_Point temp = new DJ_Point(prevLaserTilePos.X + j + 1, prevLaserTilePos.Y);
                    if (DJ_TileManagerScript.tileMap.ContainsKey(temp))
                    {
                        DJ_TileManagerScript.tileMap[temp].tile.transform.GetChild(0).renderer.material.SetFloat("_GlowStrength", 10.5f);
                    }
                }
            }
        }
        else if (laserTilePos.X > prevLaserTilePos.X)
        {
            int distanceX = laserTilePos.X - prevLaserTilePos.X;
            if (distanceX > 1)
            {
                Debug.Log("distanceX2 = " + distanceX);
                for (int j = 0; j < distanceX; j++)
                {
                    DJ_Point temp = new DJ_Point(laserTilePos.X + j + 1, laserTilePos.Y);
                    if (DJ_TileManagerScript.tileMap.ContainsKey(temp))
                    {
                        DJ_TileManagerScript.tileMap[temp].tile.transform.GetChild(0).renderer.material.SetFloat("_GlowStrength", 10.5f);
                    }
                }
            }
        }

        if (prevLaserTilePos.Y > laserTilePos.Y)
        {
            int distanceZ = prevLaserTilePos.Y - laserTilePos.Y;
            Debug.Log("distanceZ1 = " + distanceZ);
            if (distanceZ > 1)
            {
                Debug.Log("distanceZ1 = " + distanceZ);
                for (int j = 0; j < distanceZ; j++)
                {
                    DJ_Point temp = new DJ_Point(prevLaserTilePos.X, prevLaserTilePos.Y + j + 1);
                    if (DJ_TileManagerScript.tileMap.ContainsKey(temp))
                    {
                        DJ_TileManagerScript.tileMap[temp].tile.transform.GetChild(0).renderer.material.SetFloat("_GlowStrength", 10.5f);
                    }
                }
            }
        }
        else if (laserTilePos.Y > prevLaserTilePos.Y)
        {
            int distanceZ = laserTilePos.Y - prevLaserTilePos.Y;
            Debug.Log("distanceZ1 = " + distanceZ);
            if (distanceZ > 1)
            {
                Debug.Log("distanceZ2 = " + distanceZ);
                for (int j = 0; j < distanceZ; j++)
                {
                    DJ_Point temp = new DJ_Point(laserTilePos.X, laserTilePos.Y + j + 1);
                    if (DJ_TileManagerScript.tileMap.ContainsKey(temp))
                    {
                        DJ_TileManagerScript.tileMap[temp].tile.transform.GetChild(0).renderer.material.SetFloat("_GlowStrength", 10.5f);
                    }
                }
            }
        }
        Debug.Log("previous = " + prevLaserTilePos);
        prevLaserTilePos = laserTilePos;

        Debug.Log("current = " + laserTilePos);
        */
    }
}
