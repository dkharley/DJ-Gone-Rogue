/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NAudio;
using NAudio.Midi;

public class DJ_MidiParser {

	// Public Parameters
	public bool debug; 

	// Access stuff
	private long [][] noteOnArray;
	private long [][] noteOffArray;
	private Queue<long>[] noteOn;
	private Queue<long>[] noteOff;

	// Track info
	private int num_of_tracks;
	private long track_end = 0;
	private int ppq = 0;
	private double bpm = 0;
	private string FILENAME;
	private string filepath;

	private MidiFile file;
	private MidiEventCollection collection;
	// Use this for initialization
	void Start () {
		loadFile ();
		initNoteArrays ();
		initNoteQueues ();
		// test
		// Queue<long>[] temp = GetMoveNoteQueue ();
		// PrintLongQueueArray (temp);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Analyze(string FILE_NAME)
	{
		FILENAME = FILE_NAME;
		loadFile ();
		initNoteArrays ();
		initNoteQueues ();
	}
	private void loadFile()
	{
        //string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        string dir = Application.dataPath;
		filepath = System.IO.Path.Combine(dir, "Resources/Songs/" + FILENAME + "/" + (FILENAME)+ "_layers.mid");
	 	file = new MidiFile((Resources.Load ((FILENAME+"_layers")) as MidiFile));
		file = new MidiFile(filepath, true);
		collection = file.Events;
		num_of_tracks = (collection.Tracks);

		if ( debug == true)
			Debug.Log (filepath + "\n Tracks: " + num_of_tracks);

	}
	private void initNoteArrays()
	{
		// Initialize long jagged Array
		noteOnArray = new long[num_of_tracks][];
		noteOffArray = new long[num_of_tracks][];

		// Finding times of NoteOn events, and the greatest NoteOff to find track end.
		for (int j = 0; j<collection.Tracks; j++) 
		{
			if ( debug == true)
			{
				string temp = collection.GetTrackEvents (j)[0].ToString();
				Debug.Log(temp);
			}
			
			// initialize List to collect all the AbsoluteTimes of NoteOn events.
			List<long> ontimes = new List<long>();
			List<long> offtimes = new List<long>();
			
			for (int i = 0; i<collection.GetTrackEvents (j).Count; i++) 
			{
				// TempoEvent
				if (collection.GetTrackEvents (j) [i].GetType() == typeof(TempoEvent))
				{
					TempoEvent te = (TempoEvent) collection.GetTrackEvents (j) [i];
					bpm = te.Tempo;
				}
				// times of NoteOn
				if (collection.GetTrackEvents (j) [i].CommandCode == MidiCommandCode.NoteOn)
				{
					NoteEvent ne = (NoteEvent) collection.GetTrackEvents (j) [i];
					ontimes.Add(ne.AbsoluteTime);
				}
				// greatest NoteOff
				if (collection.GetTrackEvents (j) [i].CommandCode == MidiCommandCode.NoteOff)
				{
					NoteEvent ne = (NoteEvent) collection.GetTrackEvents (j) [i];
					offtimes.Add (ne.AbsoluteTime);

					if (track_end < ne.AbsoluteTime)
					{
						track_end = ne.AbsoluteTime;
					}
				}
			}
			// Insert in Array to jagged array, converted via list of NoteOns.
			noteOnArray[j]= ontimes.ToArray();
			noteOffArray[j] = offtimes.ToArray();
		}
		if ( debug == true)
		{
			Debug.Log("Track End Time: " + track_end);
		}
	}

	private void initNoteQueues()
	{
		// Initialize the Queue we gonna use
		noteOn = new Queue<long>[(num_of_tracks - 1)];
		for (int j = 1; j < (num_of_tracks); j++) 
		{
			noteOn[j-1] = new Queue<long>();
			for (int i = 0; i < (noteOnArray[j].Length); i++)
			{
				noteOn[j-1].Enqueue(noteOnArray[j][i]);
			}
		}

		noteOff = new Queue<long>[(num_of_tracks - 1)];
		for (int j = 1; j < (num_of_tracks); j++) 
		{
			noteOff[j-1] = new Queue<long>();
			for (int i = 0; i < (noteOffArray[j].Length); i++)
			{
				noteOff[j-1].Enqueue(noteOffArray[j][i]);
			}
		}

	}

	// Returns an array of Queues that contain when each note of a musical layer is going to hit.
	public Queue<long>[] GetNoteOnQueue()
	{
		Queue<long>[] q = new Queue<long>[noteOn.Length];

		for (int i = 0; i < noteOn.Length; i++) 
		{
			q[i] = new Queue<long>(noteOn[i]);
		}
		return q;
	}

	public Queue<long>[] GetNoteOffQueue()
	{
		Queue<long>[] q = new Queue<long>[noteOff.Length];

		for (int i = 0; i < noteOff.Length; i++) {
				q [i] = new Queue<long> (noteOff [i]);
		}
		return q;
	}

	public void PrintNoteOnQueueArray()
	{
		Queue<long>[] temp = GetNoteOnQueue ();
		for (int j = 0; j<temp.Length; j++) 
		{
			for (int i = 0; i<temp[j].Count; i++) 
			{
				while (temp[i].Count>0) {
					Debug.Log (temp [i].Dequeue ());
				}
			}
		}
	}

	public void PrintNoteOffQueueArray()
	{
		Queue<long>[] temp = GetNoteOffQueue ();
		for (int j = 0; j<temp.Length; j++) 
		{
			for (int i = 0; i<temp[j].Count; i++) 
			{
				while (temp[i].Count>0) {
					Debug.Log (temp [i].Dequeue ());
				}
			}
		}
	}

	public void PrintLayerInfo()
	{
		for (int i = 0; i < collection.Tracks; i++) 
		{
			string temp = collection.GetTrackEvents(i)[0].ToString();
			Debug.Log (temp);
		}
	}

	public long GetTrackEnd()
	{
		return track_end;
	}

	public int GetPpq()
	{
		return file.DeltaTicksPerQuarterNote;
	}

	public double GetBpm()
	{
		return bpm;
	}
}
*/