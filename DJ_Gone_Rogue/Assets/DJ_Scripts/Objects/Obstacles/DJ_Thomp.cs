using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DJ_Thomp : MonoBehaviour
{
    public float maxDistance;
    private bool onBeat;

    public TweenParms origin = new TweenParms();
    public TweenParms target = new TweenParms();

    public Material safeShader, dangerShader;

    private DJ_BeatActivation _activationScript;

    private bool retracting;

    private bool fadeOut;
    private float fadeOutSpeed;

    private float fadeOutAlpha;
    private float fadeOutGlow;

    private float currAlpha, currGlowStrength;
    private float lerpA, lerpG;

    // Use this for initialization
    void Start()
    {
        onBeat = false;
        retracting = false;

        transform.position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);

        //sets the original position
        origin.Prop("position", transform.position);
        origin.Ease(EaseType.Linear);


        target.Prop("position", transform.position + new Vector3(0, -maxDistance, 0));
  
        target.Ease(EaseType.Linear);
        _activationScript = GetComponent<DJ_BeatActivation>();
        fadeOutAlpha = 0.3f;
        fadeOutGlow = 0.6f;
        gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", fadeOutAlpha);
        gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", fadeOutGlow);
    }

    // Update is called once per frame
    void Update()
    {
        onBeat = DJ_Util.activateWithSound(gameObject.GetComponent<DJ_BeatActivation>());

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

        if (onBeat)
        {
            //goes to target when it reaches its target then shrink
            HOTween.To(transform, .1f, target.OnComplete(Shrink));
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
        gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 1);
        gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 2);
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

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(DJ_Tag.DJ_Player.ToString()) && DJ_PlayerManager.player.transform.position.y < 0.6f)/*!DJ_PlayerManager.player.GetComponent<DJ_Movement>().isLerping*/
        {
            if (!retracting)
            {
                col.gameObject.GetComponent<DJ_Damageable>().isAlive = false;
                col.gameObject.GetComponent<DJ_Damageable>().deathBy = DJ_Death.FLATTEN;
                if (DJ_Animation.isInThompAnimation == false)
                {
                    DJ_Animation.thompPlayedOnce = true;
                }
            }
        }
    }
}
