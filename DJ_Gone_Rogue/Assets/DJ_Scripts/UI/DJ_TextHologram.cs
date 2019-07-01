using UnityEngine;
using System.Collections;

public class DJ_TextHologram : MonoBehaviour
{
    #region fields

    public bool Active = false;

    public bool WasActive = false;

    public Texture2D bgTexture;

	public Texture2D TextTexture;

	public float TextScale;

	private float _aspectRatio;

    private bool _selected = false;

    public bool Selected
    {
        get { return _selected; }
        set
        {
            _selected = value;
        }
    }

    [Range(0.0f, 10.0f)]
    public float PlayAnimationDuration;

    public float CurrentPlayAnimationTime;

    public Transform LabelTransform;

    [Range(0.0f, 2.0f)]
    public float MaxLabelGlow;

    public float MaxLabelAlpha;

    [Range(0.0f, 5.0f)]
    public float MaxHeight;

    [Range(0.0f, 5.0f)]
    public float MinHeight;

    public float MaxLabelScaleX;

    public float MinLabelScaleX;

    public float MaxLabelScaleZ;

    public float MinLabelScaleZ;

    public float CurrentEffectTime;

    [Range(0.01f, 5.0f)]
    public float EffectDuration;

    private Vector3 _labelPos;

    private DJ_Point _tilePos;
    #endregion

    public Transform BGTransform;

    public Transform QuestionMark;

    // Use this for initialization
    void Start()
    {
        _labelPos = LabelTransform.position;

        _labelPos.y = 0.0f;

        LabelTransform.position = _labelPos;

        CurrentEffectTime = 0.0f;

        _tilePos = new DJ_Point(0, 0);

        DJ_Util.GetTilePos(this.transform.position, _tilePos);

        LabelTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
        LabelTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);

        BGTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
        BGTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);

		_aspectRatio = TextTexture.width / TextTexture.height;

        //Quaternion _r = QuestionMark.rotation;
        //_r.y = LabelTransform.rotation.y;

        //QuestionMark.rotation = _r;

		SetupTextTexture();

        currQuestionAngle = (float)Random.Range(0.0f, Mathf.PI * 2.0f);
	}
	
	private void SetupTextTexture()
	{
		if(null != TextTexture)
		{
			LabelTransform.GetComponent<Renderer>().material.SetTexture("_MainTex", TextTexture);
			LabelTransform.GetComponent<Renderer>().material.SetTexture("_GlowTex", TextTexture);
			LabelTransform.GetComponent<Renderer>().material.SetTexture("_Illum", TextTexture);

            BGTransform.GetComponent<Renderer>().material.SetTexture("_MainTex", bgTexture);
            BGTransform.GetComponent<Renderer>().material.SetTexture("_GlowTex", bgTexture);
            BGTransform.GetComponent<Renderer>().material.SetTexture("_Illum", bgTexture);
			
			_aspectRatio = ((float)TextTexture.width) / ((float)TextTexture.height);
		}
	}

    private void Reset()
    {

    }


    public static bool OnHover = false;

    // Update is called once per frame
    void Update()
    {
        //if the player is close enough to the ground (after a jump for example), and on this level text tile,
        //then activate this effect
        if (DJ_PlayerManager.playerTilePos.X == _tilePos.X && DJ_PlayerManager.playerTilePos.Y == _tilePos.Y
           && DJ_PlayerManager.player.transform.position.y < .1f)
        {
            Active = true;
        }
        else
        { //otherwise, don't
            if (Active)
                WasActive = true;
            Active = false;
        }

        if (Active || (!Active && WasActive))
        {
            if (CurrentQuestionTime <= .01f)
            {


                //UpdateHologram();

                currQuestionAngle = (float)Random.Range(0.0f, Mathf.PI * 2.0f);
                UpdateEffectTime();
            }
        }
        
        
        if (!Active || (Active && !WasActive))
        {
            if (CurrentEffectTime <= .01f)
            {
                x = 1.0f;
                UpdateQuestionTime();
            }
        }

        UpdateTextLabel();
        UpdateQuestionMark();
        //if (!Active && !WasActive)
        //{
        //    CurrentEffectTime = 0.0f;
        //    CurrentQuestionTime = EffectDuration;
        //}
        //else
        //{

        //}
    }

    public float CurrentQuestionTime;
    private float currQuestionAngle = 0.0f;

    [Range(0.0f, Mathf.PI * 2.0f)]
    public float dAngle = Mathf.PI / 60.0f;

    void UpdateQuestionMark()
    {
        //update the position of the label
        _labelPos = QuestionMark.position;
        _labelPos.y = (MaxHeight * .1f) * Mathf.Cos(currQuestionAngle) + MaxHeight / 4.0f + (MaxHeight / 4.0f * .35f / EffectDuration) * CurrentQuestionTime;
        //Set pos of label text
        QuestionMark.position = _labelPos;

        scale = QuestionMark.localScale;

        scale.x = CurrentQuestionTime / EffectDuration;
        scale.y = CurrentQuestionTime / EffectDuration;
        scale.z = CurrentQuestionTime / EffectDuration;

        QuestionMark.localScale = scale;

        //update the material properties
        QuestionMark.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 1.0f * (1.0f / (EffectDuration * EffectDuration * EffectDuration)) * CurrentQuestionTime * CurrentQuestionTime * CurrentQuestionTime);
        QuestionMark.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 3.0f * (MaxLabelGlow / (EffectDuration * EffectDuration * EffectDuration)) * CurrentQuestionTime * CurrentQuestionTime * CurrentQuestionTime);

        
    }

    void UpdateQuestionTime()
    {
        if (!Active)
            CurrentQuestionTime += Time.deltaTime;
        else
            CurrentQuestionTime -= Time.deltaTime;

        if (CurrentQuestionTime < 0.0f)
            CurrentQuestionTime = 0.0f;
        else if (CurrentQuestionTime >= EffectDuration)
        {
            WasActive = false;
            CurrentQuestionTime = EffectDuration;
        }

        currQuestionAngle += dAngle * Time.deltaTime;
        currQuestionAngle = currQuestionAngle % (Mathf.PI * 2.0f);
    }

    Vector3 scale = new Vector3();

    void UpdateEffectTime()
    {

        //if the effect is active, increment the current animation time
        //and cap it at the max effect duration
        if (Active)
        {
            if (!EaseInPlay)
            {
                CurrentEffectTime += Time.deltaTime;

                if (CurrentEffectTime > EffectDuration)
                {
                    CurrentEffectTime = EffectDuration;

                    EaseInPlay = true;
                }
            }
            else
            {

                CurrentPlayAnimationTime += Time.deltaTime;

                if (CurrentPlayAnimationTime > PlayAnimationDuration)
                {
                    CurrentPlayAnimationTime = PlayAnimationDuration;
                }
            }
        }
        //if it is not active, decrement the current animation time
        //and clamp it at 0 if it goes below 0
        else if (WasActive)
        {
            if (!EaseInPlay)
            {
                CurrentEffectTime -= Time.deltaTime;

                if (CurrentEffectTime <= 0.0f)
                {
                    CurrentEffectTime = 0.0f;
                    WasActive = false;
                    Reset();
                }
            }
            else
            {
                CurrentPlayAnimationTime -= Time.deltaTime;

                if (CurrentPlayAnimationTime < 0.0f)
                {
                    CurrentPlayAnimationTime = 0.0f;
                    EaseInPlay = false;
                }
            }
        }
    }

    bool EaseInPlay = false;

	void UpdateTextLabel ()
	{
		//update the position of the label
		_labelPos = LabelTransform.position;
		_labelPos.y = MinHeight + (MaxHeight / EffectDuration) * CurrentEffectTime;
		//Set pos of label text
		LabelTransform.position = _labelPos;
		
		//update the material properties
		LabelTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 1.0f * (1.0f / (EffectDuration * EffectDuration * EffectDuration)) * CurrentEffectTime * CurrentEffectTime * CurrentEffectTime);
		LabelTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", (MaxLabelGlow / (EffectDuration * EffectDuration * EffectDuration)) * CurrentEffectTime * CurrentEffectTime * CurrentEffectTime);
        //Set pos of label text
        _labelPos.x -= .001f;
        _labelPos.y -= .001f;
        _labelPos.z -= .001f;
        BGTransform.position = _labelPos;

        //update the material properties
        BGTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", MaxLabelAlpha * (1.0f / (EffectDuration * EffectDuration * EffectDuration)) * CurrentEffectTime * CurrentEffectTime * CurrentEffectTime);
        BGTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", (MaxLabelGlow / (EffectDuration * EffectDuration * EffectDuration)) * CurrentEffectTime * CurrentEffectTime * CurrentEffectTime);
		
		//This kinda works well enough
		/*
		float pos = Camera.main.nearClipPlane + .02f;
		//LabelTransform.position = Camera.main.transform.position + Camera.main.transform.forward * pos;
		float h = Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad * .5f) * pos * 2f;
		LabelTransform.localScale = new Vector3(h * Camera.main.aspect * TextScale * TextTexture.width, 1.0f,  h * TextScale * TextTexture.height);
		*/
		ScaleText ();
	}

    [Range(0.01f, Mathf.PI * 8.0f)]
    public float bubbleBounceSpeed;
    private float currBubbleBounceSpeed;
    [Range(0.01f, 10.0f)]
    public float bubbleBounceDuration;
    private float currBubbleBounceTime;
    public float x = Mathf.PI;

	void ScaleText ()
	{
        float _scale = (CurrentEffectTime / EffectDuration) * (CurrentEffectTime / EffectDuration) * (CurrentEffectTime / EffectDuration);
		float currHeight = 1.0f * Screen.height;
		float currWidth = 1.0f * Screen.width;
        float safeArea = ((float)TextTexture.height - ((float)TextTexture.width / 2.0f)) / ((float)TextTexture.width / 2.0f);
        float targetHeight = -(.2f - safeArea) * Screen.height;
		float targetWidth = targetHeight / _aspectRatio;
		float scaleFactor = currHeight < currWidth ? targetHeight / currHeight : targetWidth / currWidth;
		scaleFactor *= _scale;
		float pos = Camera.main.nearClipPlane + Vector3.Magnitude (DJ_CameraManager.camFinalOffsets);
		//LabelTransform.position = Camera.main.transform.position + Camera.main.transform.forward * pos;
		float h = Mathf.Tan (Camera.main.fieldOfView * Mathf.Deg2Rad * .5f) * pos * 2f;
        
        float angle = x % (Mathf.PI * 2.0f);

        float bounceAmount = 3.0f * Mathf.Cos(angle) / ((1.0f + x) * (1.0f + x) * .75f);
        float bounceAmountPhased = 3.0f * Mathf.Cos(angle + Mathf.PI) / ((1.0f + x) * (1.0f + x) * .75f);

        if (bounceAmount < .001f)
            bounceAmount = 0.0f;
        if (bounceAmountPhased < .001f)
            bounceAmountPhased = 0.0f;

        LabelTransform.localScale = new Vector3(h * TextScale * scaleFactor * _aspectRatio + bounceAmount,
                                                1.0f,
                                                h * TextScale * scaleFactor + bounceAmountPhased);
        BGTransform.localScale = new Vector3(h * TextScale * scaleFactor * _aspectRatio + bounceAmount,
                                             1.0f,
                                             h * TextScale * scaleFactor + bounceAmountPhased);

        currBubbleBounceTime += Time.deltaTime;


        x += Time.deltaTime * bubbleBounceSpeed;
        //DJ_Util.PrintOnce("Scale = " + LabelTransform.localScale + "\n" +
        //                  "TextureWidth = " + TextTexture.width + "\n" +
        //                  "TextureHeight = " + TextTexture.height + "\n" +
        //                  "ScreenWidth = " + Screen.width + "\n" +
        //                  "ScreenHeight = " + Screen.height + "\n" +
        //                  "AspectRatio = " + _aspectRatio);
        //DJ_Util.PrintOnce("SafeArea = " + safeArea);
	}

    //void UpdateHologram()
    //{
    //    //update the scale of the "fake" volumetric light
    //    Vector3 scale = SpotlightTransform.localScale;

    //    scale.x = MinLightScaleX + ((MaxLightScaleX - MinLightScaleX) / EffectDuration) * CurrentEffectTime;
    //    scale.y = MinLightScaleY + ((MaxLightScaleY - MinLightScaleY) / EffectDuration) * CurrentEffectTime;
    //    scale.z = MinLightScaleZ + ((MaxLightScaleZ - MinLightScaleZ) / EffectDuration) * CurrentEffectTime;

    //    //set the scale of the "fake" volumetric light
    //    SpotlightTransform.transform.localScale = scale;

    //    //update the material properties
    //    SpotlightTransform.gameObject.renderer.material.SetFloat("_GlowStrength", (MaxLightGlow / EffectDuration) * CurrentEffectTime);
    //    SpotlightTransform.gameObject.renderer.material.SetFloat("_Alpha", (MaxLightAlpha / EffectDuration) * CurrentEffectTime);
    //}

}










