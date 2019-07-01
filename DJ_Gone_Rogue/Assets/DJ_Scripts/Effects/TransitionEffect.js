#pragma strict

import System.IO;

@script ExecuteInEditMode
@script RequireComponent (Camera)
@script AddComponentMenu ("DJ/Image Effects/TransitionEffect")

class TransitionEffect extends PostEffectsBase
{
	@Range(0,1)
	public var R : float = 1.0f;
	@Range(0,1)
	public var G : float = 1.0f;
	@Range(0,1)
	public var B : float = 1.0f;

	public var lengthOfEffect : float = 2.0f;
	
	public var widthOfLine : float = 6.0f;
	
	protected var screenWidth : int;
	protected var screenHeight : int;
	
	public var effectShader : Shader;
	protected var effectMaterial : Material = null;
	
	public var currEffectTime : float = 0.0f;
	
	function CheckResources() : boolean
	{
		CheckSupport(false);
		
		effectMaterial = CheckShaderAndCreateMaterial (effectShader, effectMaterial);
		
		if(!isSupported)
			ReportAutoDisable();
			
		return isSupported;
	}
	
	function OnDisable()
	{
		if(effectMaterial)
			DestroyImmediate(effectMaterial);
	}
	
	function ActivateEffect()
	{
		currEffectTime = 0.0f;
	}
	
	function DeactivateEffect()
	{
		currEffectTime = 0.0f;
	}
	
	var shotNum = 0;
	
	function OnRenderImage(source : RenderTexture, destination : RenderTexture)
	{
		if(!CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		
		//currEffectTime += Time.deltaTime;
		
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		Mathf.Clamp(widthOfLine, 0.0f, screenWidth);
		
		if(lengthOfEffect <= 0)
			lengthOfEffect = .001;
			
		if(currEffectTime <= 0)
			currEffectTime = .001;
		
		//Debug.Log("Width of screen = " + screenWidth);
		
		effectMaterial.SetFloat("_R", R);
		effectMaterial.SetFloat("_G", G);
		effectMaterial.SetFloat("_B", B);
		
		effectMaterial.SetFloat("_CurrTime", lengthOfEffect);
		effectMaterial.SetFloat("_EffectDuration", currEffectTime);
		effectMaterial.SetInt("_ScreenWidth", screenWidth);
		effectMaterial.SetInt("_ScreenHeight", screenHeight);
		effectMaterial.SetFloat("_WidthOfLine", widthOfLine);
		
		Graphics.Blit(source, destination, effectMaterial);
		 
		 
		 RenderTexture.active = null;
		 Camera.main.targetTexture = null;
	}
}
