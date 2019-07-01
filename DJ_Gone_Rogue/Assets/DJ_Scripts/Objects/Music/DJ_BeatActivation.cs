using UnityEngine;
using System.Collections;


/// <summary>
/// DJ_BeatActivation. Generalized variables attached to each obstacle 
/// in order to know what part of the music they are synced to, where, and
/// what direction they start in.
/// 
/// @author - Donnell Lu
/// </summary>
public class DJ_BeatActivation : MonoBehaviour {

	// Use this for initialization

    // For now and this is a reference to the beat.
    // Instrument 1 = base
    // Instrument 2 = snare
    // Instrument 3 = synth1
    // Instrument 4 = synth2;

    public enum instru {in1 = 0, in2, in3, in4 }

    public bool instrument1, instrument2, instrument3, instrument4;
    public bool instrumentThreshold1, instrumentThreshold2, instrumentThreshold3, instrumentThreshold4;
    public bool thresholdStartNote;

	void Start () {
        //instrument1 = instrument2 = instrument3 = instrument4 = false;
        //instrumentThreshold1 = instrumentThreshold2 = instrumentThreshold3 = instrumentThreshold4 = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
