using UnityEngine;
using System.Collections;

public class DJ_LoadingScreen : MonoBehaviour
{
	public Texture2D texture;
	static DJ_LoadingScreen instance;
	
	void Awake()
	{
		if (instance)
		{
			Destroy(gameObject);
			hide();
			return;
		}
		instance = this;    
		gameObject.AddComponent<GUITexture>().enabled = false;
		GetComponent<GUITexture>().texture = texture;
		transform.position = new Vector3(0.5f, 0.5f, 1f);
		DontDestroyOnLoad(this);
	}

	//Makes the loading Image appear. 
	public static void show()
	{
		if (!InstanceExists()) 
		{
            Debug.Log("SHOW: Existance doesn't exist");
			return;
		}
		instance.GetComponent<GUITexture>().enabled = true;
	}

	//Makes the loading image dissappear.
	public static void hide()
	{
		if (!InstanceExists()) 
		{
            Debug.Log("HIDE: Existance doesn't exist");
			return;
		}
		instance.GetComponent<GUITexture>().enabled = false;
	}
	
	static bool InstanceExists()
	{
		if (!instance)
		{
			return false;
		}
		return true;
		
	}

	public static void LoadLevel(string level)
	{
        NGUITools.SetActive(DJ_UIManager.pauseOverlay, false);
        NGUITools.SetActive(DJ_UIManager.pauseButton, false);
		show();
        //Debug.Log("Level name = " + level);
		instance.StartCoroutine( Load(level) );
	}

	private static IEnumerator Load(string level){
		AsyncOperation async = Application.LoadLevelAsync(level);
		yield return async;
		//Debug.Log ("Loading of level" + level + " complete");
		hide();
	}

}