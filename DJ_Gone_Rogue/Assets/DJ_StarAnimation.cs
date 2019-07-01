using UnityEngine;
using System.Collections;

public class DJ_StarAnimation : MonoBehaviour
{
    public bool StarAcquired = false;

	public Transform[] _rotationReferences;

    private Vector3 _startPos;

    public float _angle;
    private float _dAngle;

    public float CurrentTime;
    [Range(0.01f, 10.0f)]
    public float AnimationDuration;

    public float WobbleRadius;
    public float WobbleSpeed;

    private Vector3 _offset;

	private float _heightOffset;
	public float MaxRadius;
	private float _currRadius;
	public float MaxHeight;

	Quaternion _startRot;

	private DJ_Point _tilePos;

	// Use this for initialization
	void Start ()
    {
        _startPos = this.transform.position;
        StarAcquired = false;
        _angle = 0.0f;
		_tilePos = DJ_Util.GetTilePos(this.transform.position);
		_startRot = this.transform.rotation;
	}

	private bool _prevAcquired = false;

	// Update is called once per frame
	void Update ()
    {
		if(_tilePos.X == DJ_PlayerManager.playerTilePos.X &&
		   _tilePos.Y == DJ_PlayerManager.playerTilePos.Y)
		{
			StarAcquired = true;
		}

		if(!_prevAcquired && StarAcquired)
		{
			CurrentTime = 0.0f;
			_angle = 0.0f;
		}

	    if(StarAcquired)
        {
			WinAnimation();
        }
        else
        {
			CurrentTime = 0.0f;
            IdleAnimation();
        }

		_prevAcquired = StarAcquired;
	}

	private Vector3 _targetWinPos;

	private void WinAnimation()
	{
		_heightOffset = CurrentTime / AnimationDuration * MaxHeight;

		Vector3 _tempPos;
		_tempPos.y = DJ_PlayerManager.player.transform.position.y + _heightOffset;

		_tempPos.x = DJ_PlayerManager.player.transform.position.x + MaxRadius * (1.0f - CurrentTime / AnimationDuration) * Mathf.Cos (_angle);
		_tempPos.z = DJ_PlayerManager.player.transform.position.z + MaxRadius * (1.0f - CurrentTime / AnimationDuration) * Mathf.Sin (_angle);
		
		this.transform.position = Vector3.Lerp (this.transform.position, _tempPos, Time.deltaTime);

		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _startRot, Time.deltaTime);

		CurrentTime += Time.deltaTime;
		_angle += Mathf.PI * 2.0f * Time.deltaTime;

		if(CurrentTime > AnimationDuration)
			CurrentTime = AnimationDuration;

		if(_angle > Mathf.PI * 2.0f)
			_angle = 0.0f;
	}

    /// <summary>
    /// this animation makes the star wobble
    /// </summary>
    private void IdleAnimation()
    {
        Vector3 up = Vector3.up;
        Vector3 right = this.transform.right;

        _offset = (Mathf.Cos(_angle) * WobbleRadius * 3 / 2) * right +  Mathf.Abs(Mathf.Sin(_angle) * WobbleRadius) * up;

		this.transform.position = _startPos + _offset;

        _angle += Time.deltaTime * Mathf.Deg2Rad * 10.0f * WobbleSpeed;

		if(_angle < Mathf.PI)
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _rotationReferences[0].rotation, Time.deltaTime);
		else
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _rotationReferences[1].rotation, Time.deltaTime);

        if (_angle > 2.0f * Mathf.PI)
            _angle = 0.0f;
    }

    private Vector3 AvgPos(Vector3 one, Vector3 two, float weight)
    {
        return (one * weight + two * (1.0f - weight) ) / 2.0f;
    }
}
