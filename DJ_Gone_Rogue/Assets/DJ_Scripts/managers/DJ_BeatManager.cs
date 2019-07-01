using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

// <summary>
// DJ_BeatManager script. 
// @author Peter Kong 5/7/14
//
// TABLE OF CONTENTS: (Search the titles to skip to their respective positions.)
// 1) HELLA VARIABLES
// 2) START FUNCTIONS
// 3) UPDATE FUNCTIONS
// 4) SONG CONTROL
// 5) LAYER ACTIVATION
// 6) GET FUNCTIONS
//
// </summary>
//
//


public class DJ_BeatManager : MonoBehaviour {



    //<summary>
    // ** 1) HELLA VARIABLES **
    //</summary>
	
    public bool debug = false; // are debug printouts wanted?
    // Are Layers Active?
    public static bool paceActive, layerOneActive, layerTwoActive, layerThreeActive, layerFourActive;
    // Report instrument MIDI triggers
    public static bool pace, layerTwo, layerThree, layerFour, layerOne;

	// TODO: DELETION. Donny will delete this.
    // public static bool tempOnBeat { get; set; }	// These flash a signal when a tile is supposed to break (whether measure or interval)

	// Bass/Snare/synth1/synth2
	// public static bool bassThreshold {get;set;}
	public static bool bass {get;set;}
	// public static bool snareThreshold {get;set;}
	public static bool snare {get; set;}
	// public static bool synth1Threshold {get; set;}
	public static bool synth1 {get;set;}
	// public static bool synth2Threshold {get;set;}
	public static bool synth2 {get;set;}

    // SONGSCRIPTSTUFF

    //INPUT FROM UNITY
    public string NAME; // This will load the song from Resources via its name
    
    // We need these because not all MID files contain the BPM. 
    public float bpm; // input via Unity
    public static float hardcode_bpm;

    //** PUBLIC ACCESS 
    // Audio Sources!
    public static AudioSource[] source;
    // Music Clips!
    public static AudioClip[] clips;

    // Associating musical layers to AudioSources
    public static readonly int l_pace = 0;
    public static readonly int l_layerTwo = 1;      // originally snare
    public static readonly int l_layerThree = 2;    // originally synth1
    public static readonly int l_layerFour = 3;     // originally synth2
    public static readonly int l_layerOne = 4;      // originally bass

    // PRIVATE VARIABLES FOR MIDI
    private static long[][] noteOnArray;
    private static long[][] noteOffArray;
    private static int[] noteIndex;
    private static Layer[] layerInfo;
    // NoteOn Absolute Times
    private static float paceOn, layerTwoOn, layerThreeOn, layerFourOn, layerOneOn;
    // NoteOff Absolute Times
    private static float paceOff, layerTwoOff, layerThreeOff, layerFourOff, layerOneOff;

    // PRIVATE VARIABLES FOR PLAYING SONG INFO
    private static bool updateTime = false; // keeps track of how many seconds in a song's been played
    private static float timeSinceStart = 0; 
    private static float endLength;

    public float starttime; // This is the time during the game in which our level has started.
    private bool started; // Holds the pause for above.

    private float BEGIN_TIME = 1.0f; // Assures that the audio starts with the MIDI if there is latency
    private const float VOLUME_RATE = 0.01f; // The rate volume increases
    
    // Use this for initialization
    void Start()
    {
        started = false;
        starttime = Time.time;
        bpm = transform.parent.GetComponent<DJ_DJGoneRogueGame>().bpm;
        hardcode_bpm = bpm;
        bpm = transform.parent.GetComponent<DJ_DJGoneRogueGame>().bpm;
        NAME = transform.parent.GetComponent<DJ_DJGoneRogueGame>().songname;
        loadXML();
        loadAudio();
        noteIndex = new int[layerInfo.Length];
        Reset(); // We need this here b/c otherwise song will start w/ all layers activated
        //Resume();

    }
    
    //<summary>
    // ** 2) START FUNCTIONS **
    // loadXML() - Loads the XML of NAME.
    // LoadAudio() - Loads the AudioSource and it's Clips based off of NAME's wav/ogg files.
    // 
    //</summary>

    // Reads custom XML files and loads them
    private void loadXML()
    {
        TextAsset ta = new TextAsset();
        ta = (TextAsset)Resources.Load(NAME, typeof(TextAsset));
        SerializableLayer[] getit;
        XmlSerializer serializer = new XmlSerializer(typeof(SerializableLayer[]));
        using (System.IO.StringReader reader = new System.IO.StringReader(ta.text))
        {
            getit = serializer.Deserialize(reader) as SerializableLayer[];
        }
        // We now know how many layers we're getting
        layerInfo = new Layer[getit.Length];
        noteOnArray = new long[getit.Length][];
        noteOffArray = new long[getit.Length][];

        int i = 0;
        foreach (SerializableLayer sl in getit)
        {
            layerInfo[i] = sl.ToLayer();
            noteOnArray[i] = layerInfo[i].GetNoteOn();
            noteOffArray[i] = layerInfo[i].GetNoteOff();
            i++;
        }
    }

    // Loads audio assets
    public void loadAudio()
    {
        clips = new AudioClip[layerInfo.Length];
        source = gameObject.GetComponents<AudioSource>();

        // Load music looping from Resources
        for (int i = 0; i < layerInfo.Length; i++)
        {
			endLength = 0;
            // We're gonna stick with "0" as the first number in an array.
            string tempname = NAME + "_" + (i);
            AudioClip temp = new AudioClip();
            temp = Resources.Load(tempname) as AudioClip;
            source[i].clip = temp;
            if (temp.length > endLength)
            {
                endLength = temp.length;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // This exists so that the song plays when the game starts,
        // since the song objects are not recognized until AFTER
        // Start() is resolved.
        if (started == false && (Time.time-starttime) > (BEGIN_TIME))
        {
            started = true;
            Pause();
            Restart();
            Resume();
        }
        
        
        // Bool Flash signals
        Falsify(); // Set all signals to false
        // tempOnBeat = false; // So that the next tick will assure it's false 
        checkDebugKeys();

        // Previously Songscript functions
        checkSong();
        checkActivation();
    }

    //<summary>
    // ** 3) UPDATE FUNCTIONS **
    // Falsify() - sets everything that needs to flash from true to false in one tick... to false. 
    // checkDebugKeys() - I wonder what this is for.
    // checkSong() - Keeps the song loop going, automatically starts it from the end to start when necessary.
    //      * checkMidi() - Checks MIDI On and Off. Used by checkSong().
    // checkActivation() - Checks to see which layers are activated, and turns up their volume accordingly.
    //      * toMs() - converts each of the NoteOn/NoteOff floats into milliseconds via special equation.
    // 
    //</summary>

    // These are signals set to become false after each "flash". 
    private void Falsify()
    {
        bass = false;
        snare = false;
        synth1 = false;
        synth2 = false;

        if (pace == true)
        {
            if (debug)
                Debug.Log("pace");
            pace = false;
        }
        if (layerOne == true)
        {
            if (debug)
                Debug.Log("ONE");
            bass = true;
            layerOne = false;
        }
        if (layerTwo == true)
        {
            if (debug)
                Debug.Log("\tTWO");
            snare = true;
            layerTwo = false;
        }
        if (layerThree == true)
        {
            if (debug)
                Debug.Log("\t\tTHREE");
            synth1 = true;
            layerThree = false;
        }
        if (layerFour == true)
        {
            if (debug)
                Debug.Log("\t\t\tFOUR");
            synth2 = true;
            layerFour = false;
        }
    }

    // Debug related user input
    private void checkDebugKeys()
    {
        if (Input.GetKeyDown("p")) //switchsong TEST
        {
            Pause();
        }
        if (Input.GetKeyDown("0")) //switchsong TEST
        {
            Resume();
        }
        if (Input.GetKeyDown("y"))
        {
            ActivateLayerOne();
        }
        if (Input.GetKeyDown("u"))
        {
            ActivateLayerTwo();
        }
        if (Input.GetKeyDown("i"))
        {
            ActivateLayerThree();
        }
        if (Input.GetKeyDown("o"))
        {
            ActivateLayerFour();
        }
    }
    // This puppy keeps the loop goin'
    private void checkSong() 
    {
        if (updateTime) // How long has this iteration of the loop been playing?
        {
            timeSinceStart += Time.deltaTime;
            checkMidi();
        }
        if (timeSinceStart >= endLength) // When this iteration of the loop is finished, restart. 
        {
            Restart();
            Resume();
        }
    }
    // checks to see if layers are activated and if so, keep volume at 1. 
    private void checkActivation() 
    {
        if (paceActive)  // Check for layer activation
        {
            source[l_pace].volume = 1f;
        }
        if (layerOneActive)
        {
            source[l_layerOne].volume = 1f;
        }
        if (layerTwoActive)
        {
            source[l_layerTwo].volume = 1f;
        }
        if (layerThreeActive)
        {
            source[l_layerThree].volume = 1f;
        }
        if (layerFourActive)
        {
            source[l_layerFour].volume = 1f;
        }
    }
    // checks times of MIDI hits, adjusts trigger bools as necessary.
    private void checkMidi()
    {
        if ((paceOn) <= source[l_pace].time && paceOn >= 0f)
        {
            pace = true;
            paceOn = -1f;
            noteIndex[l_pace]++;
            if (noteIndex[l_pace] < noteOnArray[l_pace].Length)
                paceOn = toMs(l_pace, true);
            if (noteIndex[l_pace] < noteOffArray[l_pace].Length)
                paceOff = toMs(l_pace, false);
        }

        if ((layerTwoOn) <= source[l_layerTwo].time && layerTwoOn >= 0f)
        {
            layerTwo = true;
            layerTwoOn = -1f;
            noteIndex[l_layerTwo]++;
            if (noteIndex[l_layerTwo] < noteOnArray[l_layerTwo].Length)
                layerTwoOn = toMs(l_layerTwo, true);
            if (noteIndex[l_layerTwo] < noteOffArray[l_layerTwo].Length)
                layerTwoOff = toMs(l_layerTwo, false);
        }
        if ((layerThreeOn) <= source[l_layerThree].time && layerThreeOn >= 0f)
        {
            layerThree = true;
            layerThreeOn = -1f;
            noteIndex[l_layerThree]++;
            if (noteIndex[l_layerThree] < noteOnArray[l_layerThree].Length)
                layerThreeOn = toMs(l_layerThree, true);
            if (noteIndex[l_layerThree] < noteOffArray[l_layerThree].Length)
                layerThreeOff = toMs(l_layerThree, false);
        }
        if ((layerFourOn) <= source[l_layerFour].time && layerFourOn >= 0f)
        {
            layerFour = true;
            layerFourOn = -1f;
            noteIndex[l_layerFour]++;
            if (noteIndex[l_layerFour] < noteOnArray[l_layerFour].Length)
                layerFourOn = toMs(l_layerFour, true);
            if (noteIndex[l_layerFour] < noteOffArray[l_layerFour].Length)
                layerFourOff = toMs(l_layerFour, false);
        }
        // Since we won't always have 4 layers. 
        if (layerInfo.Length == 5)
        {
            if ((layerOneOn) <= source[l_layerOne].time && layerOneOn >= 0f)
            {
                layerOne = true;
                layerOneOn = -1f;
                noteIndex[l_layerOne]++;
                if (noteIndex[l_layerOne] < noteOnArray[l_layerOne].Length)
                    layerOneOn = toMs(l_layerOne, true);
                if (noteIndex[l_layerOne] < noteOffArray[l_layerOne].Length)
                    layerOneOff = toMs(l_layerOne, false);
            }
        }
    }

    // toMs()
    // Converts ticks to milliseconds: 60f * ticks/ ppq * bpm
    // Overload functions to make toMS calls easier in BeatManager.
    private static float toMs(int layer, bool onArray)
    {
        long ticks;
        if (onArray)
            ticks = noteOnArray[layer][noteIndex[layer]];
        else
            ticks = noteOffArray[layer][noteIndex[layer]];
        double bpm = layerInfo[layer].GetBpm();
        int ppq = layerInfo[layer].GetPpq();

        return toMs(ticks, bpm, ppq);
    }
    private static float toMs(long ticks, double bpm, int ppq)
    {
        if (ticks == 0)
        {
            return 0f;
        }
        else if (bpm == 0)
        {
            float t = (float)ticks;
            return (60f * t / (((float)ppq) * ((float)hardcode_bpm)));
        }
        else
        {
            float t = (float)ticks;
            return (60f * t / (((float)ppq) * ((float)bpm)));
        }
    }
    

    //<summary>
    // ** 4) SONG CONTROL **
    // Reset() - Resets the song and all layers.
    // Restart() - Starts the song from the first measure.
    // Pause() - 
    // Resume() - 
    // LayerVolume() - changes specified layer's volume to given amount.
    //
    //</summary>

    // Resets the song as though the level just loaded.
    public void Reset()
    {
        paceActive = true;
        layerTwoActive = false;
        layerThreeActive = false;
        layerFourActive = false;
        layerOneActive = false;
        source[0].volume = 0f;
        source[1].volume = 0f;
        source[2].volume = 0f;
        source[3].volume = 0f;
        source[4].volume = 0f;
        source[5].volume = 0f;
        source[6].volume = 0f;
        Restart();
    }

    // Restarts the song loop to play from the beginning
    public void Restart()
    {
        Pause();
        // Set time start to zero
        timeSinceStart = 0f;
        source[0].Stop();
        source[1].Stop();
        source[2].Stop();
        source[3].Stop();
        source[4].Stop();
        source[5].Stop();
        source[6].Stop();

        for (int i = 0; i < noteIndex.Length; i++)
        {
            noteIndex[i] = 0;
        }

        paceOn = toMs(l_pace, true);
        paceOff = toMs(l_pace, false);

        layerTwoOn = toMs(l_layerTwo, true);
        layerTwoOff = toMs(l_layerTwo, false);
        layerThreeOn = toMs(l_layerThree, true);
        layerThreeOff = toMs(l_layerThree, false);
        layerFourOn = toMs(l_layerFour, true);
        layerFourOff = toMs(l_layerFour, false);
        // Since we won't always have 4 layers. 
        if (layerInfo.Length == 5)
        {
            layerOneOn = toMs(l_layerOne, true);
            layerOneOff = toMs(l_layerOne, false);
        }
    }

    public void Pause()
    {
        updateTime = false;
        source[0].Pause();
        source[1].Pause();
        source[2].Pause();
        source[3].Pause();
        source[4].Pause();
        source[5].Pause();
        source[6].Pause();
    }

    public void Resume()
    {
        updateTime = true;
        source[0].Play();
        source[1].Play();
        source[2].Play();
        source[3].Play();
        source[4].Play();
        source[5].Play();
        source[6].Play();
    }
    public static void LayerVolume(int layer, float volume)
    {
        source[layer].volume = volume;
    }

    //<summary>
    // ** 5) LAYER ACTIVATION **
    // ActivateLayerPace() - Activates the pace layer.
    // ActivateLayer[One..Four]() - Activates the instrument [one...four] layer.
    //
    //</summary>

    // Relaying Methods
    public static void ActivateLayerPace()
    {
        paceActive = true;
    }
    public static void ActivateLayerTwo()
    {
        layerTwoActive = true;
        //DJ_BeatManager.layerTwoOn = true;
    }
    public static void ActivateLayerThree()
    {
        layerThreeActive = true;
        //DJ_BeatManager.layerThreeOn = true;
    }
    public static void ActivateLayerFour()
    {
        layerFourActive = true;
        //DJ_BeatManager.layerFourOn = true;
    }
    public static void ActivateLayerOne()
    {
		if(layerInfo.Length >= 5) {
			layerOneActive = true;
			// DJ_BeatManager.layerOneOn = true;
		}
		//else throw new System.ArgumentException("LayerOneActive(): No 4th Obstacle Layer given!");
		else Debug.Log("LayerOneActive(): No 4th Obstacle Layer given!");
    }

    //<summary> 
    // ** 6) GET FUNCTIONS **
    // GetNextLayer___On() - returns float of the next "On" midi signal of a layer
    // GetNextLayer___Off() - returns float of the next "Off" midi signal of a layer
    // GetEndTime() - returns float of the song's end time in milliseconds 
    //
    //</summary>
    public static float GetNextLayerPaceOn()
    {
        if (paceOn == -1f)
        {
            return (endLength - timeSinceStart +
                    toMs(noteOnArray[l_pace][0], layerInfo[l_pace].GetBpm(), layerInfo[l_pace].GetPpq()));
        }
        else return (paceOn - timeSinceStart);
    }
    public static float GetNextLayerTwoOn()
    {
        if (layerTwoOn == -1f)
        {
            return (endLength - timeSinceStart +
                    toMs(noteOnArray[l_layerTwo][0], layerInfo[l_layerTwo].GetBpm(), layerInfo[l_layerTwo].GetPpq()));
        }
        else return (layerTwoOn - timeSinceStart);
    }
    public static float GetNextLayerThreeOn()
    {
        if (layerThreeOn == -1f)
        {
            return (endLength - timeSinceStart +
                    toMs(noteOnArray[l_layerThree][0], layerInfo[l_layerThree].GetBpm(), layerInfo[l_layerThree].GetPpq()));
        }
        else return (layerThreeOn - timeSinceStart);
    }
    public static float GetNextLayerFourOn()
    {
        if (layerFourOn == -1f)
        {
            return (endLength - timeSinceStart +
                    toMs(noteOnArray[l_layerFour][0], layerInfo[l_layerFour].GetBpm(), layerInfo[l_layerFour].GetPpq()));
        }
        else return (layerFourOn - timeSinceStart);
    }
    public static float GetNextLayerOneOn()
    {
        if (layerInfo.Length == 5)
        {
            if (layerOneOn == -1f)
            {
                return (endLength - timeSinceStart +
                        toMs(noteOnArray[l_layerOne][0], layerInfo[l_layerOne].GetBpm(), layerInfo[l_layerOne].GetPpq()));
            }
            else return (layerOneOn - timeSinceStart);
        }
        else throw new System.ArgumentException("GetNextBassOn(): No 4th Obstacle Layer given!");
    }
    public static float GetNextPaceOff()
    {
        if (paceOff == -1f)
        {
            return (endLength - timeSinceStart +
                    toMs(noteOffArray[l_pace][0], layerInfo[l_pace].GetBpm(), layerInfo[l_pace].GetPpq()));
        }
        else return (paceOff - timeSinceStart);
    }

    public static float GetNextLayerTwoOff()
    {
        if (layerTwoOff == -1f)
        {
            return (endLength - timeSinceStart +
                    toMs(noteOffArray[l_layerTwo][0], layerInfo[l_layerTwo].GetBpm(), layerInfo[l_layerTwo].GetPpq()));
        }
        else return (layerTwoOff - timeSinceStart);
    }
    public static float GetNextLayerThreeOff()
    {
        if (layerThreeOff == -1f)
        {
            return (endLength - timeSinceStart +
                    toMs(noteOffArray[l_layerThree][0], layerInfo[l_layerThree].GetBpm(), layerInfo[l_layerThree].GetPpq()));
        }
        else return (layerThreeOff - timeSinceStart);
    }
    public static float GetNextLayeFourOff()
    {
        if (layerFourOff == -1f)
        {
            return (endLength - timeSinceStart +
                    toMs(noteOffArray[l_layerFour][0], layerInfo[l_layerFour].GetBpm(), layerInfo[l_layerFour].GetPpq()));
        }
        else return (layerFourOff - timeSinceStart);
    }
    public static float GetNextLayerOneOff()
    {
        if (layerInfo.Length == 5)
        {
            if (layerOneOff == -1f)
            {
                return (endLength - timeSinceStart +
                        toMs(noteOffArray[l_layerOne][0], layerInfo[l_layerOne].GetBpm(), layerInfo[l_layerOne].GetPpq()));
            }
            else return (layerOneOff - timeSinceStart);
        }
        else throw new System.ArgumentException("GetNextBassOff(): No 4th Obstacle Layer given!");
    }

    public static float GetEndTime()
    {
        return endLength;
    }
}
