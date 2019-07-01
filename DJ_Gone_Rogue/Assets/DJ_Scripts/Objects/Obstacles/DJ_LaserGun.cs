using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DJ_LaserGun : MonoBehaviour {

    // drag laser prefab here
    public GameObject laserBeamPrefab;

    // actual object ref
    public GameObject laser;

    public Transform lightSource, targetSource;

    // laser length
    public float laserLength;

    // starting direction of the laser gun
    public DJ_LaserDirection firingDirection;

    public DJ_Rotation startingRotation;

    // determines whether it is on beat or not
    private bool onBeat;

    // determines whether the music layer is on
    private bool layerOn;

    // USING HOTWEEEN

    public TweenParms origin = new TweenParms();
    public TweenParms target = new TweenParms();
    public Transform rotationVector;
    public float targetAngle;
    public float currentAngle;

    private DJ_Point laserTilePos = new DJ_Point(0, 0);

    public float lengthOfBeam = 1.5f;

	// Use this for initialization
	void Start () {
        targetAngle = 0;
        currentAngle = 0;
        onBeat = false;
        CreateLaser();
	}

    void CreateLaser()
    {
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
        laser = (GameObject.Instantiate(laserBeamPrefab) as GameObject);
        laser.transform.parent = transform;
        laser.transform.position = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z -.35f);
        laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y - 0.15f, laser.transform.localScale.z * laserLength);
        //Scale the object on X axis in local units
        if (laserLength == 1)
        {
            laserLength = 1.5f;
        }
        //Move the object on X axis in local units
        if (laserLength > 1)
        {
            laser.transform.position = new Vector3(laser.transform.position.x, laser.transform.position.y, laser.transform.position.z - .50f * laserLength);
        }
        // Edit the size of the hitbo as well
        laser.GetComponent<BoxCollider>().size = new Vector3(laser.GetComponent<BoxCollider>().size.x, (laser.GetComponent<BoxCollider>().size.y * laserLength), laser.GetComponent<BoxCollider>().size.z);
        laser.GetComponent<BoxCollider>().center = new Vector3(laser.GetComponent<BoxCollider>().center.x, laser.GetComponent<BoxCollider>().center.y, laser.GetComponent<BoxCollider>().center.z);
        rotationVector = laser.transform;
        
        switch (firingDirection)
        {
            case DJ_LaserDirection.NORTH:
                laser.transform.RotateAround(transform.position, Vector3.up, 0);
                break;
            case DJ_LaserDirection.NORTHEAST:
                laser.transform.RotateAround(transform.position, Vector3.up, 45);
                break;
            case DJ_LaserDirection.EAST:
                laser.transform.RotateAround(transform.position, Vector3.up, 90);
                break;
            case DJ_LaserDirection.SOUTHEAST:
                laser.transform.RotateAround(transform.position, Vector3.up, 135);
                break;
            case DJ_LaserDirection.SOUTH:
                laser.transform.RotateAround(transform.position, Vector3.up, 180);
                break;
            case DJ_LaserDirection.SOUTHWEST:
                laser.transform.RotateAround(transform.position, Vector3.up, 225);
                break;
            case DJ_LaserDirection.WEST:
                laser.transform.RotateAround(transform.position, Vector3.up, 270);
                break;
            case DJ_LaserDirection.NORTHWEST:
                laser.transform.RotateAround(transform.position, Vector3.up, 315);
                break;
        }
        laser.GetComponent<MeshRenderer>().enabled = false;
        laser.GetComponent<BoxCollider>().enabled = false;
        lightSource.GetComponent<ParticleRenderer>().enabled = false;
        lengthOfBeam = laserLength;
    }

	// Update is called once per frame
	void Update () 
    {
        laserTilePos.X = Mathf.RoundToInt(laser.transform.position.x);
        laserTilePos.Y = Mathf.RoundToInt(laser.transform.position.z);
        //Debug.Log("laser tile pos = " + laserTilePos);

        Vector3 _temp = lightSource.position;
        _temp.y = laser.transform.position.y;
        lightSource.position = _temp;

        Vector3 _dir = (laser.transform.position - lightSource.position).normalized;
        _dir.y = 0.0f;
        targetSource.position = lightSource.position + _dir * lengthOfBeam;

        layerOn = DJ_Util.isLayerOn(gameObject.GetComponent<DJ_BeatActivation>());

        if (layerOn)
        {
            //laser.GetComponent<MeshRenderer>().enabled = true;
            laser.GetComponent<BoxCollider>().enabled = true;
            lightSource.GetComponent<ParticleRenderer>().enabled = true;
        }
        
        onBeat = DJ_Util.activateWithNoSound(gameObject.GetComponent<DJ_BeatActivation>());

        //if (DJ_BeatManager.thresholdStartNote)
        if (onBeat)
        {
            changeLaserDirection();
        }
        else
        {
            //laser.transform.rotation = Quaternion.Slerp(laser.transform.rotation, rotationVector.transform.rotation, Time.deltaTime);
            //var newRotation = Quaternion.LookRotation(laser.transform.position + rotationVector.position, Vector3.up);
            //laser.transform.rotation = Quaternion.Slerp(laser.transform.rotation, newRotation, Time.deltaTime);
            
            /*
            if (currentAngle <= targetAngle)
            {
                laser.transform.RotateAround(transform.position, Vector3.up, 3);
                currentAngle += 3;
                if (currentAngle > 360)
                    currentAngle = 0;
                Debug.Log("Current angle is " + currentAngle);
            }
            else
            {
                Debug.Log("DONE");
            }
            */
        }

        
        if (DJ_TileManagerScript.tileMap.ContainsKey(laserTilePos))
        {
            if (laser.GetComponent<BoxCollider>().enabled)
            {
                DJ_TileManagerScript.tileMap[laserTilePos].tile.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_GlowStrength", 10.5f);
                //DJ_TileManagerScript._recentlyLaserVisitedTiles.Add(laserTilePos); //set the time remaining of the effect
                //DJ_TileManagerScript._tileLaserGlowTimes.Add(laserTilePos, 1.5f);
            }
        }
        
	}


    void changeLaserDirection()
    {
        //rotationVector.RotateAround(laser.transform.position, Vector3.up, 45);
        /*
        targetAngle += 45;
        if (targetAngle > 360)
        {
            targetAngle = 0;
        }
        Debug.Log("target angle is " + targetAngle);
        */
        switch (startingRotation)
        {
            case DJ_Rotation.CLOCKWISE:
                laser.transform.RotateAround(transform.position, Vector3.up, 45);
                break;
            case DJ_Rotation.COUNTERCLOCKWISE:
                laser.transform.RotateAround(transform.position, Vector3.up, -45);
                break;
        }
        
        //rotationVector += new Vector3(0, 45, 0);
        //target.Prop("rotation", rotationVector);
    }

}
