using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DJ_Ball : MonoBehaviour {
	public DJ_Directions direction;
	
	public float maxDistance;
	public bool isActive;
	private bool onBeat;
	
	public TweenParms origin = new TweenParms();
	public TweenParms target = new TweenParms();

    public Material safeShader, dangerShader;

    // determines whether the music layer is on
    private bool layerOn;

    private bool retracting;


    private DJ_BeatActivation _activationScript;

    private bool fadeOut;
    private float fadeOutSpeed;

    private float fadeOutAlpha;
    private float fadeOutGlow;

    private float currAlpha, currGlowStrength;
    private float lerpA, lerpG;

	// Use this for initialization
	void Start () {
		onBeat = false;
        retracting = false;

        transform.position = new Vector3(transform.position.x, transform.position.y -.5f, transform.position.z);

		//sets the original position
		origin.Prop("position", transform.position);
		origin.Ease(EaseType.Linear);
		
		//sets the target position
		switch(direction){
            case DJ_Directions.Up:
			target.Prop("position", transform.position + new Vector3(0, 0, -maxDistance));
			break;
            case DJ_Directions.Down:
			target.Prop("position", transform.position + new Vector3(0, 0, maxDistance));
			break;
            case DJ_Directions.Left:
			target.Prop("position", transform.position + new Vector3(maxDistance, 0, 0));
			break;
            case DJ_Directions.Right:
			target.Prop("position", transform.position + new Vector3(-maxDistance, 0, 0));
			break;
		default:
			target.Prop("position", transform.position + new Vector3(0, 0, -maxDistance));
			break;
		}
		target.Ease(EaseType.Linear);


        _activationScript = GetComponent<DJ_BeatActivation>();
        fadeOutAlpha = .3f;
        fadeOutGlow = 0.6f;
        gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 1);
        gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 2);

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

        if (_activationScript.instrument1)
        {
            fadeOutSpeed = DJ_BeatManager.GetNextLayerOneOn(); ;
        }
        if (_activationScript.instrument2)
        {
            fadeOutSpeed = DJ_BeatManager.GetNextLayerTwoOn(); ;
        }
        if (_activationScript.instrument3)
        {
            fadeOutSpeed = DJ_BeatManager.GetNextLayerThreeOn(); ;
        }
        if (_activationScript.instrument4)
        {
            fadeOutSpeed = DJ_BeatManager.GetNextLayerFourOn(); ;
        }

		onBeat = DJ_Util.activateWithSound(gameObject.GetComponent<DJ_BeatActivation>());
		if (onBeat)
		{
            gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 1);
            gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 2);
			//goes to target when it reaches its target then shrink
			HOTween.To(transform, .2f, target.OnComplete(Shrink));
            retracting = false;
            //gameObject.renderer.material = dangerShader;
		}

        if (fadeOut)
        {
            currAlpha = gameObject.GetComponent<Renderer>().material.GetFloat("_Alpha");
            lerpA = Mathf.Lerp(currAlpha, fadeOutAlpha, fadeOutSpeed * Time.deltaTime);
            currGlowStrength = gameObject.GetComponent<Renderer>().material.GetFloat("_GlowStrength");
            lerpG = Mathf.Lerp(currGlowStrength, fadeOutGlow, fadeOutSpeed * Time.deltaTime);
            gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", lerpA);
            gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", lerpG);
        }
	}
    

	
	private void Shrink()
	{
        if (gameObject.GetComponent<DJ_BeatActivation>().instrument1)
        {
            HOTween.To(transform, DJ_BeatManager.GetNextLayerOneOn(), origin);
        }
        else if (gameObject.GetComponent<DJ_BeatActivation>().instrument2)
        {
            HOTween.To(transform, DJ_BeatManager.GetNextLayerTwoOn(), origin);
        }
        else if (gameObject.GetComponent<DJ_BeatActivation>().instrument3)
        {
            HOTween.To(transform, DJ_BeatManager.GetNextLayerThreeOn(), origin);
        }
        else if (gameObject.GetComponent<DJ_BeatActivation>().instrument4)
        {
            HOTween.To(transform, DJ_BeatManager.GetNextLayerFourOn(), origin);
        }
        retracting = true;
        fadeOut = true;
        //gameObject.renderer.material = safeShader;
	}
	
	public void setDirection(string dir){
		switch(dir){
		case "up":
            direction = DJ_Directions.Up;
			break;
		case"down":
            direction = DJ_Directions.Down;
			break;
		case"left":
            direction = DJ_Directions.Left;
			break;
		case"right":
            direction = DJ_Directions.Right;
			break;
		default:
			break;
		}
	}
	
	public void OnCollisionEnter(Collision col)
	{
        if (col.gameObject.CompareTag(DJ_Tag.DJ_Player.ToString()) && !DJ_PlayerManager.player.GetComponent<DJ_Movement>().isLerping)
        {
            if (!retracting)
            {
                switch (direction)
                {
                    case DJ_Directions.Up:
                        //if (col.gameObject.transform.position.z < gameObject.transform.position.z)
                        {
                            col.gameObject.GetComponent<DJ_Movement>().direction = DJ_Dir.UP;
                            col.gameObject.GetComponent<DJ_Movement>().maxMoveDistance = (int)maxDistance;
                            col.gameObject.GetComponent<DJ_Movement>().canMove = true;
                        }
                        //Debug.Log("UP");
                        break;
                    case DJ_Directions.Down:
                        //if (col.gameObject.transform.position.z > gameObject.transform.position.z)
                        {
                            col.gameObject.GetComponent<DJ_Movement>().direction = DJ_Dir.DOWN;
                            col.gameObject.GetComponent<DJ_Movement>().maxMoveDistance = (int)maxDistance;
                            col.gameObject.GetComponent<DJ_Movement>().canMove = true;
                        }
                        //Debug.Log("DOWN");
                        break;
                    case DJ_Directions.Left:
                        //if (col.gameObject.transform.position.x > gameObject.transform.position.x)
                        {
                            col.gameObject.GetComponent<DJ_Movement>().direction = DJ_Dir.RIGHT;
                            col.gameObject.GetComponent<DJ_Movement>().maxMoveDistance = (int)maxDistance;
                            col.gameObject.GetComponent<DJ_Movement>().canMove = true;
                        }
                        //Debug.Log("LEFT");
                        break;
                    case DJ_Directions.Right:
                        //if (col.gameObject.transform.position.x < gameObject.transform.position.x)
                        {
                            col.gameObject.GetComponent<DJ_Movement>().direction = DJ_Dir.LEFT;
                            col.gameObject.GetComponent<DJ_Movement>().maxMoveDistance = (int)maxDistance;
                            col.gameObject.GetComponent<DJ_Movement>().canMove = true;
                        }
                        //Debug.Log("RIGHT");
                        break;

                }
            }
        }
	}
}
