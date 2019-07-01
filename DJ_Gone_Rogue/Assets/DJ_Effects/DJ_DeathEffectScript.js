#pragma strict

import System.IO;

@script ExecuteInEditMode
@script RequireComponent (Camera)
@script AddComponentMenu ("DJ/Image Effects/DJDeathEffect")

class DJ_DeathEffectScript extends PostEffectsBase
{	
	protected var screenWidth : int;
	protected var screenHeight : int;
	
	public var effectShader : Shader;
	protected var effectMaterial : Material = null;
	
	
	@Range(0.0, 10.0)
	public var CurrentEffectTime : float = 0.0f;
	public var EffectDuration : float = 5.0f;
	
	public var Active : boolean = false;
	
	//the range for the number  of bars
	@Range(0,1000)
	public var NumberOfBars : int = 50.0;
	
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
	
	function OnRenderImage(source : RenderTexture, destination : RenderTexture)
	{
		//if the effect is  not active, meaning the player didnt die,
		//then don't use up  the gpu and cpu resources to use this effect
		//so just return
		if(!CheckResources() || !Active)
		{
			CurrentEffectTime = 0.0;
			Graphics.Blit(source, destination);
			return;
		}
		
		CurrentEffectTime += Time.deltaTime;
		
		//assign screen dims
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		//assign shader values
		effectMaterial.SetInt("_ScreenWidth", screenWidth);
		effectMaterial.SetInt("_ScreenHeight", screenHeight);
		
		effectMaterial.SetFloat("_Duration", EffectDuration);
		effectMaterial.SetFloat("_CurrTime", CurrentEffectTime);
		
		effectMaterial.SetFloat("_NumBars", NumberOfBars);
		
		//blit the textures
		Graphics.Blit(source, destination, effectMaterial);
		
		if(CurrentEffectTime > EffectDuration * 2.0)
		{
			CurrentEffectTime = 0.0;
			Active = false;
		}
	}
}
