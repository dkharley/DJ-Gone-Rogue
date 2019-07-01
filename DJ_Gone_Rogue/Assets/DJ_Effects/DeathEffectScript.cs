using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/DeathEffectCS")]
class DeathEffectScript : MonoBehaviour
{
	protected int screenWidth;
	protected int screenHeight;
	
	public Shader effectShader;
	protected Material effectMaterial = null;

	public float CurrentEffectTime = 0.0f;
	public float EffectDuration = 5.0f;
	
	public bool Active = false;
	
	//the range for the number  of bars
	public int NumberOfBars = 50;

	void Start()
	{
		effectMaterial = new Material(effectShader);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//if the effect is  not active, meaning the player didnt die,
		//then don't use up  the gpu and cpu resources to use this effect
		//so just return
		if(!Active)
		{
			CurrentEffectTime = 0.0f;
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
		
		if(CurrentEffectTime > EffectDuration * 2.0f)
		{
			CurrentEffectTime = 0.0f;
			Active = false;
		}
	}
}
