using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour
{
	public enum EffectType
	{
		EXPLOSION = 0,
		DEBRIS,
		BLOOD,
		LIGHTNING,
		SMOKE,
		SOUL,
		DEATH,
	}


	public List<string> effectsNames;
	public List<GameObject> effectsList;

	public static Dictionary<string, GameObject> effectsMap;

	// Use this for initialization
	void Start ()
	{
	
	}

	void Awake()
	{
		effectsMap = new Dictionary<string, GameObject>();

		if(effectsNames.Count != effectsList.Count)
		{
			Debug.LogError("Lengths of effects maps do not match. Effects disabled");
			return;
		}

		for(int i = 0; i < effectsNames.Count; ++i)
		{
			effectsMap.Add(effectsNames[i], effectsList[i]);
		}

		effectsNames.Clear();
		effectsList.Clear();

		effectsNames = null;
		effectsList = null;
	}

	//play the specified effect at the specified location
	//Wyatt
	public static void PlayEffect(string name, Vector3 pos)
	{
//		if(effectsMap.ContainsKey(name))
//		{
//			var e = GameObject.Instantiate(effectsMap[name]) as GameObject;
//			e.transform.position = pos;
//		}
	}

	public static void PlayEffect(string name, Vector3 pos, float scale)
	{
//		if(effectsMap.ContainsKey(name))
//		{
//			var e = GameObject.Instantiate(effectsMap[name]) as GameObject;
//			e.transform.position = pos;
//			e.transform.localScale = new Vector3(scale, scale, scale);
//		}
	}

	public static void PlayEffect(string name, Vector3 pos, Quaternion rotation)
	{

	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
