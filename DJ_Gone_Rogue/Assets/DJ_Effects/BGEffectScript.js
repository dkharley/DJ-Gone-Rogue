#pragma strict

import System.IO;

@script ExecuteInEditMode
@script RequireComponent (Camera)
@script AddComponentMenu ("DJ/Image Effects/BGEffectScript")

class BGEffectScript extends PostEffectsBase
{
	public var effectShader : Shader;
	protected var effectMaterial : Material = null;

	protected var screenWidth : int;
	protected var screenHeight : int;


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
			if(!CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			
			//currEffectTime += Time.deltaTime;
			
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			
			effectMaterial.SetFloat("_ScreenWidth", screenWidth);
			effectMaterial.SetFloat("_ScreenHeight", screenHeight);
			
	}
}