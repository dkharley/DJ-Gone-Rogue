using System;

/// Serializable Layer
/// Version of Layer with all public fields, used for XML serialization.
/// Constructor SerializableLayer(Layer layer) takes in a layer and allocates 
/// the fields, Method ToLayer() will return the information in Layer form.
/// @Peter Kong 4/3/14
/// 

public class SerializableLayer
{
    // Summary
    public String SEQ_INSTRUMENT;
    public String TRACK_INSTRUMENT;

    public long[] NoteOnArray;
    public long[] NoteOffArray;

    // We might not have these values
    public long TRACK_END = -1;
    public int PPQ = -1;
    public double BPM = -1;

    public SerializableLayer()
    {
    }

    public SerializableLayer(Layer layer)
    {
        SEQ_INSTRUMENT = layer.GetSeqInstrument();
        TRACK_INSTRUMENT = layer.GetTrackInstrument();
        NoteOnArray = layer.GetNoteOn();
        NoteOffArray = layer.GetNoteOff();
        TRACK_END = layer.GetEndTime();
        PPQ = layer.GetPpq();
        BPM = layer.GetBpm();
    }

    public Layer ToLayer()
    {
        return new Layer(SEQ_INSTRUMENT, TRACK_INSTRUMENT, NoteOnArray, NoteOffArray, TRACK_END, PPQ, BPM);
    }
}

