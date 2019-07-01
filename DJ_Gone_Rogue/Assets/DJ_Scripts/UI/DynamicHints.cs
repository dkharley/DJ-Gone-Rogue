using UnityEngine;
using System.Collections;

public class DynamicHints : MonoBehaviour {
	public string[] hints = new string[5]{
		"Balls only push you when they turn red.", 
		"Try to land on the beat, rather than moving with the beat", 
		"Have you tried using both thumbs?", 
		"Filler text", 
		"Finishing the level fast enough gives you another star"
	};
	// Use this for initialization
	void Start () {
		GetComponent<UILabel>().text = hints[Random.Range(0, hints.Length)];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
