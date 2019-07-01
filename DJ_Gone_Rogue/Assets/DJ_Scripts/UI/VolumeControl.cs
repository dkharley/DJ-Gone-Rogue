using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class VolumeControl : MonoBehaviour {

	private static UISlider _Volumeslider;
	
	// Use this for initialization
	void Start ()
	{
		_Volumeslider = gameObject.GetComponent<UISlider>();
        _Volumeslider.value = PlayerPrefs.GetFloat("GameVolume");
        //Debug.Log("Loaded Volume Value: " + _Volumeslider.value);

	}
	
	void Update()
	{
		AudioListener.volume = _Volumeslider.value;
	}

    // This method exists so pauseDropDown.cs can call this function.
    public static void saveData()
    {
        PlayerPrefs.SetFloat("GameVolume", AudioListener.volume);
        //Debug.Log("Saved Volume Value: " + AudioListener.volume);
    }
    public static void setVolume(float val)
    {
        AudioListener.volume = val;
    }
}
