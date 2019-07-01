using System;

/// Layer:
/// Custom class used to carry the following MIDI information of a single track: 
/// 1) Name of the instrument
/// 2) NoteOn signals
/// 3) NoteOff signals
/// 4) Track End time
/// 5) PPQ
/// 6) BPM
/// Also has Getters for all fields.
/// @Peter Kong 4/3/14
/// 

public class Layer
{
    private String SEQ_INSTRUMENT = "N/A";
    private String TRACK_INSTRUMENT = "N/A";
    
    private long[] NoteOnArray;
    private long[] NoteOffArray;
    
    // We might not have these values
    private long TRACK_END = -1;
    private int PPQ = -1;
    private double BPM = -1;

    public Layer()
    {
    }

    // Constructor
    public Layer(String sequencename, String trackinstrumentname, long[] noteon, long[] noteoff, long trackend, int ppq, double bpm)
    {
        SEQ_INSTRUMENT = sequencename;
        TRACK_INSTRUMENT = trackinstrumentname;

        NoteOnArray = new long[noteon.Length];
        noteon.CopyTo(NoteOnArray, 0);

        NoteOffArray = new long [noteoff.Length];
        noteoff.CopyTo(NoteOffArray, 0);

        TRACK_END = trackend;

        PPQ = ppq;

        BPM = bpm;
    }
    
    // Access Classes
    public String GetTrackInstrument()
    {
        return TRACK_INSTRUMENT;
    }
    public String GetSeqInstrument()
    {
        return SEQ_INSTRUMENT;
    }
    public long[] GetNoteOn()
    {
        long[] temp = new long[NoteOnArray.Length];
        NoteOnArray.CopyTo(temp, 0);
        return temp;
    }
    public long[] GetNoteOff()
    {
        long[] temp = new long[NoteOffArray.Length];
        NoteOffArray.CopyTo(temp, 0);
        return temp;
    }
    public int NoteOnLength()
    {
        return NoteOnArray.Length;
    }
    public int NoteOffLength()
    {
        return NoteOffArray.Length;
    }
    public double GetBpm()
    {
        return BPM;
    }
    public int GetPpq()
	{
		return PPQ;
	}
    public long GetEndTime()
    {
        return TRACK_END;
    }

    public override String ToString()
    {
        if (TRACK_INSTRUMENT != "N/A")
            return TRACK_INSTRUMENT;
        else if (SEQ_INSTRUMENT != "N/A")
            return SEQ_INSTRUMENT;
        else
            return "N/A";
    }
    public string Info()
    {
        string info =   "\r\nName:\t\t" + (this.ToString()) +
                        "\r\nBPM:\t\t" + (BPM) +
                        "\r\nTrackEnd:\t" + (TRACK_END) +
                        "\r\nPPQ:\t\t" + (PPQ) +
                        "\r\nNoteOn Count:\t" + (NoteOnArray.Length) +
                        "\r\nNoteOff Count:\t" + (NoteOffArray.Length);
        return info;
    }
}