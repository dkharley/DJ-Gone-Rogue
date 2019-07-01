#pragma strict

import System.IO;

@script ExecuteInEditMode
@script RequireComponent (Camera)
@script AddComponentMenu ("DJ/Image Effects/BGEffectScript1")

class BGEffectScript1 extends PostEffectsBase
{	
	protected var screenWidth : int;
	protected var screenHeight : int;
	
	public var effectShader : Shader;
	protected var effectMaterial : Material = null;
	
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
		
		var pass : int = 0;
		
		//assign screen dims
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		//assign shader values
		effectMaterial.SetInt("_ScreenWidth", screenWidth);
		effectMaterial.SetInt("_ScreenHeight", screenHeight);
		
		//blit the textures
		Graphics.Blit(source, destination, effectMaterial);
	}
}
