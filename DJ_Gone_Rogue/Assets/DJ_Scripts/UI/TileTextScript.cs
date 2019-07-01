using UnityEngine;
using System.Collections;

public class TileTextScript : MonoBehaviour
{
	#region fields  
	
	public bool Active = false;

	public bool WasActive = false;

	public Texture2D TextTexture;
	private float _aspectRatio;
	public float TextScale = 1.0f;

	public int FontSize = 100;

	private bool _selected = false;

	public bool Selected
	{
		get { return _selected; }
		set
		{
			_selected = value;
		}
	}

    public Transform StarTransform;

    public int NumberOfStars;

    public int NumberOfStarsAcquired;

    public GameObject StarGO;

    public GameObject[] Stars;

    public float StarAnimationDuration;

    public float[] StarTimers;

    public float StarLabelWidth;

    public float StarOffsetY;

	public Transform PlayTransform;

	public float MaxPlayAlpha;
	public float MaxPlayGlow;

	[Range(0.0f, 10.0f)]
	public float PlayAnimationDuration;

	public float CurrentPlayAnimationTime;

	[Range(0.0f, 2.0f)]
	public float MaxPlayDX;

	[Range(0.0f, 2.0f)]
	public float MaxPlayScaleXY;

	[Range(0.0f, 2.0f)]
	public float MinPlayScaleXY;

	public Transform SpotlightTransform;

	[Range(0.0f, 1.0f)]
	public float MaxLightAlpha;

	[Range(0.0f, 1.0f)]
	public float MaxLightGlow;

	[Range(0.0f, 10.0f)]
	public float MaxLightScaleX;

	[Range(0.0f, 10.0f)]
	public float MaxLightScaleY;

	[Range(0.0f, 10.0f)]
	public float MaxLightScaleZ;

	[Range(0.0f, 10.0f)]
	public float MinLightScaleX;
	
	[Range(0.0f, 10.0f)]
	public float MinLightScaleY;
	
	[Range(0.0f, 10.0f)]
	public float MinLightScaleZ;

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

    public bool textureOnly;

	private Vector3 _labelPos;

	private DJ_Point _tilePos;

	#endregion

    public Transform BGTransform;
    public Texture2D BGTexture;

	// Use this for initialization
	void Start ()
	{
		_labelPos = LabelTransform.position;

		_labelPos.y = 0.0f;

		LabelTransform.position = _labelPos;

		CurrentEffectTime = 0.0f;

		_tilePos = new DJ_Point(0,0);

		DJ_Util.GetTilePos( this.transform.position, _tilePos);

		Vector3 _pos = PlayTransform.position;
		_pos += -PlayTransform.forward * .002f;
		PlayTransform.position = _pos;
		PlayTransform.tag = "level_select_button";

        Stars = new GameObject[NumberOfStars];

        StarTimers = new float[NumberOfStars];

		PlayTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
		PlayTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);

		SpotlightTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
		SpotlightTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);

		LabelTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
		LabelTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);

        BGTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
        BGTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);

        for (int i = 0; i < Stars.Length; ++i)
        {
            Stars[i] = GameObject.Instantiate(StarGO) as GameObject;
            Stars[i].transform.parent = StarTransform;

			Stars[i].gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
			Stars[i].gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);
        }

		SetupTextTexture();
	}

	private float _conversion;

	private void SetupTextTexture()
	{
		if(null != TextTexture)
		{
			LabelTransform.GetComponent<Renderer>().material.SetTexture("_MainTex", TextTexture);
			LabelTransform.GetComponent<Renderer>().material.SetTexture("_GlowTex", TextTexture);
			LabelTransform.GetComponent<Renderer>().material.SetTexture("_Illum", TextTexture);

            BGTransform.GetComponent<Renderer>().material.SetTexture("_MainTex", BGTexture);
            BGTransform.GetComponent<Renderer>().material.SetTexture("_GlowTex", BGTexture);
            BGTransform.GetComponent<Renderer>().material.SetTexture("_Illum", BGTexture);

			_aspectRatio = TextTexture.width / TextTexture.height;
		}
	}

	private void Reset()
	{
		for(int i = 0; i < StarTimers.Length; ++i)
		{
			StarTimers[i] = 0.0f;
			Stars[i].gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
			Stars[i].gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);
		}
	}

	public static bool OnHover = false;

	// Update is called once per frame
	void Update ()
	{
		//if the player is close enough to the ground (after a jump for example), and on this level text tile,
		//then activate this effect
		if(DJ_PlayerManager.playerTilePos.X == _tilePos.X && DJ_PlayerManager.playerTilePos.Y == _tilePos.Y
		   && DJ_PlayerManager.player.transform.position.y < .01f)
		{
			Active = true;
		}
		else { //otherwise, don't
			if(Active)
				WasActive = true;
			Active = false;
		}

		if(Active || (!Active && WasActive))
		{
			
			NumberOfStarsAcquired = LevelSelectScreenScript.NumberOfStars;

			if(OnHover)
			{
				PlayTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 2.5f);
				PlayTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", MaxPlayAlpha + .05f);
				if(_selected)
				{
					_selected = false;
					LevelSelectScreenScript.PlayLevel();
				}
			}
			else
			{
				PlayTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 1.0f);
			}

			UpdateEffectTime ();

			UpdateTextLabel ();
			
			UpdateHologram  ();

			UpdatePlayButton();

	        UpdateStars();

			UpdateText();
		}
	}

	void UpdateText()
	{
//		//update the position
//		TextMesh.transform.position = LabelTransform.position;
//
//		//update the scale
//		scale.x = (CurrentEffectTime / EffectDuration) / FontSize;
//		scale.y = (CurrentEffectTime / EffectDuration) / FontSize;
//		TextMesh.transform.localScale = scale;
//
//		Color _col = TextMesh.renderer.material.color;
//		_col.a = (CurrentEffectTime * CurrentEffectTime * CurrentEffectTime * CurrentEffectTime) / (EffectDuration * EffectDuration * EffectDuration * EffectDuration) * .9f;
//
//		TextMesh.renderer.material.color = _col;
//
////		TextTransform.localRotation = LabelTransform.localRotation;
////		TextTransform.rotation = LabelTransform.rotation;
	}

    public float StarForwardOffset;
    private static Vector3 _starScale = new Vector3(.01617273f, .4173487f, 0.03172573f);

    void UpdateStars()
    {
		if(StarTransform == null)
			return;

		//update position  of star container
        Vector3 _pos = LabelTransform.localPosition;
        _pos.y = LabelTransform.localPosition.y - StarOffsetY * (CurrentEffectTime / EffectDuration) * (CurrentPlayAnimationTime / PlayAnimationDuration);
        _pos += Vector3.Cross(LabelTransform.right, LabelTransform.forward) * StarForwardOffset;
        StarTransform.localPosition = _pos;

        float _startX = (StarLabelWidth / 2.0f) * (CurrentEffectTime / EffectDuration);
        float _dy = -(StarLabelWidth / (Stars.Length - 1)) * (CurrentEffectTime / EffectDuration);

		//update positions of stars
        for (int i = 0; i < Stars.Length; ++i)
        {
            Vector3 _p = Vector3.zero;
            _p.x = _startX + _dy * (i);
            Stars[i].transform.localPosition = _p;
            Stars[i].transform.rotation = StarTransform.rotation;
            Stars[i].transform.localScale = _starScale;
//            Stars[i].gameObject.renderer.material.SetFloat("_Alpha", 0.0f);
//			Stars[i].gameObject.renderer.material.SetFloat("_GlowStrength", 0.0f);
        }

		//set the values for any unacquired stars to 0
        for (int i = Stars.Length - 1; i >= NumberOfStarsAcquired && i < Stars.Length; --i )
        {
            Stars[i].gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
			Stars[i].gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);
            StarTimers[i] = 0.0f;
        }


		//only if the stars should be visible should we adjust the alpha
		//and glow values of the material
		//if(CurrentPlayAnimationTime >= PlayAnimationDuration)
		{
	        if (NumberOfStarsAcquired >= 1)
	        {
	            Stars[0].gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 1.0f * (StarTimers[0] / StarAnimationDuration) * (CurrentPlayAnimationTime / PlayAnimationDuration));
				Stars[0].gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", .5f * (StarTimers[0] / StarAnimationDuration) * (CurrentPlayAnimationTime / PlayAnimationDuration));
	            
				if(CurrentPlayAnimationTime >= PlayAnimationDuration)
					StarTimers[0] += Time.deltaTime;
				else
					StarTimers[0] -= Time.deltaTime;

	            if (StarTimers[0] > StarAnimationDuration)
	                StarTimers[0] = StarAnimationDuration;
				if(StarTimers[0] < 0)
					StarTimers[0] = 0;
	        }

	        for (int i = 1; i < NumberOfStarsAcquired && i < Stars.Length; ++i)
	        {
				Stars[i].gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 1.0f * (StarTimers[i] / StarAnimationDuration) * (CurrentPlayAnimationTime / PlayAnimationDuration));
				Stars[i].gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", .5f * (StarTimers[i] / StarAnimationDuration) * (CurrentPlayAnimationTime / PlayAnimationDuration));

	            if (CurrentEffectTime >= EffectDuration && StarTimers[i - 1] >= StarAnimationDuration)
	                StarTimers[i] += Time.deltaTime;
				else
					StarTimers[i] -= Time.deltaTime;

	            if (StarTimers[i] > StarAnimationDuration)
	                StarTimers[i] = StarAnimationDuration;

				if(StarTimers[i] < 0)
					StarTimers[i] = 0.0f;
	        }
		}
    }

	void UpdateEffectTime()
	{

		//if the effect is active, increment the current animation time
		//and cap it at the max effect duration
		if(Active) {
			if(!EaseInPlay) {
				CurrentEffectTime += Time.deltaTime;

				if(CurrentEffectTime > EffectDuration) {
					CurrentEffectTime = EffectDuration;

					EaseInPlay = true;
				}
			}
			else {

				CurrentPlayAnimationTime += Time.deltaTime;

				if(CurrentPlayAnimationTime > PlayAnimationDuration) {
					CurrentPlayAnimationTime = PlayAnimationDuration;
				}
			}
		}
		//if it is not active, decrement the current animation time
		//and clamp it at 0 if it goes below 0
		else if(WasActive) {
			if(!EaseInPlay) {
				CurrentEffectTime -= Time.deltaTime;

				if(CurrentEffectTime < 0.0f) {
					CurrentEffectTime = 0.0f;
					WasActive = false;
					Reset ();
				}
			}
			else {
				CurrentPlayAnimationTime -= Time.deltaTime;

				if(CurrentPlayAnimationTime < 0.0f) {
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
		//update the scale of the label
//		Vector3 _scale = LabelTransform.localScale;
//		_scale.x = (MinLabelScaleX + ( (MaxLabelScaleX - MinLabelScaleX) / EffectDuration) * CurrentEffectTime);
//		_scale.z = (MinLabelScaleZ + ( (MaxLabelScaleZ - MinLabelScaleZ) / EffectDuration) * CurrentEffectTime);
//		//set the scale of the label
//		LabelTransform.localScale = _scale;

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

	void ScaleText ()
	{
		float _scale = CurrentEffectTime / EffectDuration;
		float currHeight = 1.0f * Screen.height;
		float currWidth = 1.0f * Screen.width;
		float targetHeight = textureOnly ? .5f * Screen.height : .2f * Screen.height;
		float targetWidth = targetHeight * currWidth / currHeight;
		float scaleFactor = currHeight < currWidth ? targetHeight / currHeight : targetWidth / currWidth;
		scaleFactor *= _scale;
		float pos = Camera.main.nearClipPlane + Vector3.Magnitude (DJ_CameraManager.camFinalOffsets);
		//LabelTransform.position = Camera.main.transform.position + Camera.main.transform.forward * pos;
		float h = Mathf.Tan (Camera.main.fieldOfView * Mathf.Deg2Rad * .5f) * pos * 2f;
        LabelTransform.localScale = new Vector3(h * _aspectRatio * TextScale * scaleFactor, 1.0f, h * TextScale * scaleFactor);
        BGTransform.localScale = new Vector3(h * _aspectRatio * TextScale * scaleFactor, 1.0f, h * TextScale * scaleFactor);
	}

	void UpdateHologram()
	{
		//update the scale of the "fake" volumetric light
		Vector3 scale = SpotlightTransform.localScale;
		
		scale.x = MinLightScaleX + ((MaxLightScaleX - MinLightScaleX) / EffectDuration) * CurrentEffectTime;
		scale.y = MinLightScaleY + ((MaxLightScaleY - MinLightScaleY) / EffectDuration) * CurrentEffectTime;
		scale.z = MinLightScaleZ + ((MaxLightScaleZ - MinLightScaleZ) / EffectDuration) * CurrentEffectTime;
		
		//set the scale of the "fake" volumetric light
		SpotlightTransform.transform.localScale = scale;

		//update the material properties
		SpotlightTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", (MaxLightGlow / EffectDuration) * CurrentEffectTime);
		SpotlightTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", (MaxLightAlpha / EffectDuration) * CurrentEffectTime);
	}

	float angle = 0.0f;

	void UpdatePlayButton()
	{

		Vector3 _pos = PlayTransform.position;
		
		Vector3 _scale = PlayTransform.localScale;

		_scale.x = MaxPlayScaleXY;
		_scale.z = MaxPlayScaleXY;

		_pos = LabelTransform.position;

		//if(EaseInPlay)
		{
			//_pos += _PlayLabelOffset * (1.0f / EffectDuration) * CurrentEffectTime;

			//_pos = PlayTransform.localRotation * Vector3.left * (MaxPlayDX / PlayAnimationDuration) * CurrentPlayAnimationTime;
			_pos -= PlayTransform.right * ((1.0f + LabelTransform.localScale.x + MaxPlayDX) / (PlayAnimationDuration * PlayAnimationDuration * PlayAnimationDuration)) * CurrentPlayAnimationTime * CurrentPlayAnimationTime * CurrentPlayAnimationTime;
		}
//		else {
//			
//		}

		_pos.y = MinHeight + (MaxHeight / EffectDuration) * CurrentEffectTime;

		_pos += -PlayTransform.forward * .002f;

		PlayTransform.localScale = _scale;
		PlayTransform.position = _pos;
		
		if( CurrentPlayAnimationTime > 0.0f )
		{
			if(!OnHover)
				PlayTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", MaxPlayAlpha);
		}
		else
		{
			PlayTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
		}
		
		//PlayTransform.gameObject.renderer.material.SetFloat("_Alpha", (1.0f / (EffectDuration * EffectDuration * EffectDuration)) * CurrentEffectTime * CurrentEffectTime * CurrentEffectTime);
		if(!OnHover)
		{
			PlayTransform.gameObject.GetComponent<Renderer>().material.SetFloat("_GlowStrength", Mathf.Sin(angle) * MaxPlayGlow * (MaxLabelGlow / (EffectDuration * EffectDuration * EffectDuration)) * CurrentEffectTime * CurrentEffectTime * CurrentEffectTime);
			angle = (angle + Mathf.PI / 50) % (Mathf.PI * 2);
		}
		else
		{
			angle = Mathf.PI / 2;
		}
	}
}










