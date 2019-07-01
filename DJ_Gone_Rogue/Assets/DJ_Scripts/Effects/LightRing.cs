using UnityEngine;
using System.Collections;

public class LightRing : MonoBehaviour {

    public enum AnimationState
    {
        Hover = 0,
        Spin = 1,
    }

    public int RingNumber;

    public AnimationState state;

	public Transform Beam;
	public Transform Source;

	public float CurrentTime;

	[Range(0.001f, 10.0f)]
	public float AnimationTime;

	public float MaxBeamAlpha;
    public float MaxBeamGlow;
    public float MinHeight;
	public float MaxHeight;
	public float NumRotations;
    public float hoverSpeed;
    public float hoverDistance;

    private float currRotY;
    private Vector3 startPos;
    private float angle;
    private DJ_Point _tilePos;

    private bool _active = false;

    private Quaternion _startRotation;

    public bool BlastOff = false;

	// Use this for initialization
	void Start ()
	{
        state = AnimationState.Hover;
        startPos = this.transform.localPosition;
        _tilePos = DJ_Util.GetTilePos(this.transform.parent.position);
        Beam.GetComponent<Renderer>().material.SetFloat("_Alpha", 0.0f);
        Beam.GetComponent<Renderer>().material.SetFloat("_GlowStrength", 0.0f);
        _active = false;
        _startRotation = this.transform.rotation;
        BlastOff = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateState();

        HoverRings();

        RotateRing();
	}

    private void UpdateState()
    {
        BlastOff = false;

        if (_tilePos.X == DJ_PlayerManager.playerTilePos.X && _tilePos.Y == DJ_PlayerManager.playerTilePos.Y)
        {
            CurrentTime += Time.deltaTime;
            _active = true;

            if (CurrentTime > AnimationTime)
            {
                CurrentTime = AnimationTime;
                BlastOff = true;
            }
        }
        else
        {
            CurrentTime -= Time.deltaTime;

            if (CurrentTime < 0.0f)
            {
                CurrentTime = 0.0f;
                _active = false;
            }
        }


        Beam.GetComponent<Renderer>().material.SetFloat("_Alpha", MaxBeamAlpha * (CurrentTime / AnimationTime) * (CurrentTime / AnimationTime));
        Beam.GetComponent<Renderer>().material.SetFloat("_GlowStrength", MaxBeamGlow * (CurrentTime / AnimationTime) * (CurrentTime / AnimationTime));
    }

    private void HoverRings()
    {
        float angleOffset = 45.0f * Mathf.Deg2Rad;
        _pos.y = startPos.y + hoverDistance * Mathf.Cos(angle + RingNumber * angleOffset);
        _pos.z = startPos.z + hoverDistance * Mathf.Sin(angle + 90.0f * Mathf.Deg2Rad + RingNumber * angleOffset);
        _pos.z = startPos.x + hoverDistance * Mathf.Cos(angle + 120.0f * Mathf.Deg2Rad + RingNumber * angleOffset);
        this.transform.localPosition += (_pos - this.transform.localPosition) / AnimationTime;

        angle += Time.deltaTime * hoverSpeed;

        if (angle > Mathf.PI * 2.0f)
            angle = 0.0f;
    }

    private void RotateRing()
    {
        _rot = this.transform.rotation;
        //_rot.y = (NumRotations * Mathf.PI * 2.0f) * CurrentTime / AnimationTime;

        if (!_active)
            currRotY -= ((NumRotations * Mathf.PI * 1.935f))
                        * (CurrentTime / AnimationTime) * (CurrentTime / AnimationTime);
        else
            currRotY += ((NumRotations * Mathf.PI * 1.935f))
                        * (CurrentTime / AnimationTime) * (CurrentTime / AnimationTime);


        _rot = Quaternion.AngleAxis(currRotY, Vector3.up) * _rot;

        //this.transform.rotation = _rot;

        Quaternion _tempRot2 = Quaternion.Slerp(this.transform.rotation, _startRotation,
                                                Time.deltaTime * (1.0f - CurrentTime / AnimationTime) *
                                                (1.0f - CurrentTime / AnimationTime) *
                                                (1.0f - CurrentTime / AnimationTime));

        float t = Mathf.Clamp(CurrentTime, 0.0f, AnimationTime) / AnimationTime;

        this.transform.rotation = AvgRotation(_rot, _tempRot2, t * t);
    }

    private Quaternion AvgRotation(Quaternion one, Quaternion two, float weight)
    {
        float x1 = one.x * weight;
        float x2 = two.x * (1.0f - weight);
        float y1 = one.y * weight;
        float y2 = two.y * (1.0f - weight);
        float z1 = one.z * weight;
        float z2 = two.z * (1.0f - weight);
        float w1 = one.w * weight;
        float w2 = two.w * (1.0f - weight);
        return new Quaternion((x1 + x2) / 2.0f, (y1 + y2) / 2.0f,
                              (z1 + z2) / 2.0f, (w1 + w2) / 2.0f);
    }

	private Vector3 _pos;
	private Quaternion _rot;
}
