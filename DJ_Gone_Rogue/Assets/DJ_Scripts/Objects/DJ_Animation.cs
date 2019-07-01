using UnityEngine;
using System.Collections;
using SmoothMoves;

public class DJ_Animation : MonoBehaviour {
    public BoneAnimation mcAnimation;
    public BoneAnimation Animate;

    public bool isLerping;
    public bool isDancing;
    public bool dancingPlayedOnce;
    public bool lerpingPlayedOnce;
    public bool landingPlayedOnce;

    public static bool shockPlayedOnce;
    public static bool thompPlayedOnce;

    public static bool isInThompAnimation;
    public static bool isInShockAnimation;

    public bool thomp;

	// Use this for initialization
	void Start () {
        mcAnimation = GetComponent<BoneAnimation>();
        lerpingPlayedOnce = false;
        landingPlayedOnce = false;
        shockPlayedOnce = false;
        isDancing = false;
        dancingPlayedOnce = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        thomp = isInThompAnimation;

        if (transform.parent.parent.GetComponent<DJ_Damageable>().isAlive == true)
        {
            if (!DJ_PlayerManager.PlayerReachedEndOfLevel)
            {
                checkJumpLandBools();
                checkShockBool();
                checkThompBool();
                landingIntoIdleAnimation();
                goIntoJumpAnimation();
            }
            else
            {
                playDancing();
            }
        }

        if (transform.parent.parent.GetComponent<DJ_Damageable>().isAlive == false)
        {
            //checkJumpLandBools();
            checkShockBool();
            checkThompBool();
            shockAnimation();
            squashAnimation();

        }






	}



    void checkJumpLandBools()
    {
        
        if (transform.parent.parent.GetComponent<DJ_Movement>().isLerping)
        {
            // check if player is not in transitioning jump animation
            if (!mcAnimation.IsPlaying("TransitionJumpAnimation"))
            {
                // if player is currently not in a jump animation
                if (!mcAnimation.IsPlaying("JumpAnimation"))
                    lerpingPlayedOnce = true;
            }
        }
        else
        {
            // if player is in a landing animation
            if (!mcAnimation.IsPlaying("LandingAnimation"))
            {
                // check if currently in idle animation
                if (!mcAnimation.IsPlaying("IdleAnimation"))
                    landingPlayedOnce = true;
            }
        }
    }

    void checkThompBool()
    {
        if (mcAnimation.IsPlaying("SquashAnimation") || mcAnimation.IsPlaying("IdleSquash"))
        {
            isInThompAnimation = true;
        }
        else
        {
            isInThompAnimation = false;
        }
    }

    void checkShockBool()
    {
        if (mcAnimation.IsPlaying("ShockAnimation") || mcAnimation.IsPlaying("DissipateAnimation") || mcAnimation.IsPlaying("IdleDeathShock"))
        {
            isInShockAnimation = true;
        }
        else
        {
            isInShockAnimation = false;
        }
    }

    void goIntoJumpAnimation()
    {
        if (lerpingPlayedOnce == true)
        {
            mcAnimation.Stop();
            mcAnimation.Play("TransitionJumpAnimation");
            mcAnimation.PlayQueued("JumpAnimation", QueueMode.CompleteOthers);
            lerpingPlayedOnce = false;
        }
    }

    void landingIntoIdleAnimation()
    {
        if (landingPlayedOnce == true)
        {
            mcAnimation.Stop();
            mcAnimation.Play("LandingAnimation");
            mcAnimation.PlayQueued("IdleAnimation", QueueMode.CompleteOthers);
            landingPlayedOnce = false;
        }
    }

    void shockAnimation()
    {
        if (shockPlayedOnce == true)
        {
            mcAnimation.Stop();
            mcAnimation.Play("ShockAnimation");
            mcAnimation.PlayQueued("DissipateAnimation", QueueMode.CompleteOthers);
            mcAnimation.PlayQueued("IdleDeathShock", QueueMode.CompleteOthers);
            shockPlayedOnce = false;
        }
    }

    void squashAnimation()
    {
        if (thompPlayedOnce == true)
        {
            if (!mcAnimation.IsPlaying("SquashAnimation") || !mcAnimation.IsPlaying("IdleSquash"))
            {
                mcAnimation.Stop();
                mcAnimation.Play("SquashAnimation");
                mcAnimation.PlayQueued("IdleSquash", QueueMode.CompleteOthers);
                thompPlayedOnce = false;
            }
        }
    }

    void playDancing()
    {
        isDancing = true;
        lerpingPlayedOnce = false;
        landingPlayedOnce = false;
        if (mcAnimation.IsPlaying("IdleAnimation"))
        {
            if (dancingPlayedOnce == false)
            {
                mcAnimation.Stop();
                int randomNumber = Random.Range(1, 4);
                if (randomNumber == 1)
                {
                    mcAnimation.PlayQueued("WinAnimation1");
                }
                if (randomNumber == 2)
                {
                    mcAnimation.PlayQueued("WinAnimation2");
                }
                if (randomNumber == 3)
                {
                    mcAnimation.PlayQueued("WinAnimation3");
                }
                if (randomNumber == 4)
                {
                    mcAnimation.PlayQueued("WinAnimation4");
                }
                dancingPlayedOnce = true;
            }
        }
    }

}
