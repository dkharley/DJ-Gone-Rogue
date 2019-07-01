using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Gets the tile position based on the actual position.
/// </summary>
/// 
/// @modified - Jason Wang 1/27/2014
///     - Added additional parameter to LeapTrajectory named float currHeight that currPos.Y will be set to.
/// <param name="position">Position.</param>
/// <param name="tilePos">Tile position.</param>

public static class DJ_Util
{
	private static string cmp = "";
	private static int printCount = 0;

	public static void PrintOnce(string msg)
	{
		if(cmp.CompareTo(msg) != 0)
		{
			cmp = msg;
			Debug.Log(cmp);
			printCount++;
		}

		if(printCount > 10)
		{
			printCount = 0;
			cmp = "";
		}
	}

	public static void GetTilePos(Vector3 position, DJ_Point tilePos)
	{
		tilePos.X = (int)(position.x + .5f);
		tilePos.Y = (int)(position.z + .5f);
	}

	public static DJ_Dir GetOppositeDirection(DJ_Dir dir)
	{
		if(dir == DJ_Dir.LEFT)
			return DJ_Dir.RIGHT;
		
		if(dir == DJ_Dir.RIGHT)
			return DJ_Dir.LEFT;
		
		if(dir == DJ_Dir.UP)
			return DJ_Dir.DOWN;
		
		if(dir == DJ_Dir.DOWN)
			return DJ_Dir.UP;

		return DJ_Dir.NONE;
	}

	//created by Fernando
	public static DJ_Point GetTilePos(Vector3 position)
	{
		DJ_Point tilePos = new DJ_Point(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
		return tilePos;
	}

	public static DJ_Dir GetPreferredDirection(DJ_Point from, DJ_Point to)
	{
		DJ_Dir direction = DJ_Dir.NONE;

		float dx = to.X - from.X;
		float dy = to.Y - from.Y;

		if(Mathf.Abs(dx) > Mathf.Abs(dy))
		{
			if(dx < 0)
				direction = DJ_Dir.LEFT;
			if(dx > 0)
				direction = DJ_Dir.RIGHT;
		}
		else
		{
			if(dy < 0)
			{
				direction = DJ_Dir.UP;
			}
			if(dy > 0)
			{
				direction = DJ_Dir.DOWN;
			}
		}

		return direction;
	}

	public static void GetNearbyTiles(DJ_Point tilePos, float depth, ref List<DJ_Point> nearbyTiles)
	{
		/*
		for(int i = 0; i < nearbyTiles.Count; ++i)
		{
			DJ_Point p = nearbyTiles[i];

			if(DJ_TileManagerScript.tileMap.ContainsKey(p))
				DJ_TileManagerScript.tileMap[p].tile.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
		}
		*/

		nearbyTiles.Clear();
		
		int numTilesPerDim = (int)(1 + 2 * depth);
		
		if(depth > 1)
		{
			numTilesPerDim = 3 * (int)Mathf.Pow(2.0f, depth - 1.0f) - 1;
		}
		
		DJ_Point startPoint = new DJ_Point(tilePos.X - Mathf.FloorToInt(numTilesPerDim / 2.0f), tilePos.Y - Mathf.FloorToInt(numTilesPerDim / 2.0f));
		
		DJ_Point currPoint = startPoint;
		
		for(int x = 0; x < numTilesPerDim; ++x)
		{
			for(int z = 0; z < numTilesPerDim; ++z)
			{
				DJ_Point newPoint = new DJ_Point(currPoint.X + x, currPoint.Y + z);
				
				//print ("Next tile pos = " + newPoint);
				nearbyTiles.Add(newPoint);
			}
		}

		//debug coloring of tiles
		///*
//		for(int i = 0; i < nearbyTiles.Count; ++i)
//		{
//			DJ_Point p = nearbyTiles[i];
//			if(DJ_TileManagerScript.tileMap.ContainsKey(p))
//				DJ_TileManagerScript.tileMap[nearbyTiles[i]].tile.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
//		}
		//*/
	}

	public static void GetNeighboringTilePos(DJ_Point currTilePos, DJ_Dir direction, DJ_Point targetTilePos)
	{
		GetNeighboringTilePos(currTilePos, direction, 1, targetTilePos);
	}

	public static void GetNeighboringTilePos(DJ_Point currTilePos, DJ_Dir direction, int dist, DJ_Point targetTilePos)
	{ 
		targetTilePos.Set (currTilePos);
		if(direction == DJ_Dir.DOWN)
		{
				targetTilePos.Y += dist;
		}
		else if(direction == DJ_Dir.UP)
		{
				targetTilePos.Y += -dist;
		}
		else if(direction == DJ_Dir.LEFT)
		{
				targetTilePos.X += -dist;
		}
		else if(direction == DJ_Dir.RIGHT)
		{
				targetTilePos.X += dist;
		}
	}

	private static Rect _rect = new Rect();

	public static Rect GetScreenSpaceRectangle(Transform go)
	{
		_rect.width = Screen.height * 10 * go.localScale.z;
		_rect.height = Screen.height * 10 * go.localScale.x;
		_rect.x = .5f * Screen.width + go.localPosition.x * Screen.height - _rect.width * .5f;
		_rect.y = Screen.height - Screen.height * (.5f + go.localPosition.x) - _rect.height * .5f;

		return _rect;
	}

	public static void ScaleRectToScreen(Transform go, Vector3 targetSize)
	{
		float _scale = 2f / Screen.height *
						(go.position.z - Camera.main.transform.position.z) *
				Mathf.Tan (Camera.main.fieldOfView / 2f * Mathf.PI / 180);

		go.localScale = .1f * targetSize * _scale;
	}

	/// <summary>
	/// Smooth lerp along trajectory. Used primarily for hopping between tiles.
	/// </summary>
	/// <param name="currPos">Curr position.</param>
	/// <param name="targetTilePos">Target tile position.</param>
	/// <param name="targetHeight">Target height.</param>
	/// <param name="animationTime">Animation time.</param>
	public static void LerpTrajectory(ref Vector3 currPos, DJ_Point prevTilePos, DJ_Point targetTilePos,
	                                  float targetHeight, float currAnimationTime, float animationLength, float currHeight)
	{

		float dx = (targetTilePos.X - prevTilePos.X);
		float dz = (targetTilePos.Y - prevTilePos.Y);

		//if dx is 0, snap x to 0 to avoid errors
		if(dx == 0)
		{
			currPos.x = targetTilePos.X;
		}

		//if dz is 0, snap z to 0 to avoid errors
		if(dz == 0)
		{
			currPos.z = targetTilePos.Y;
		}


		float dxdt = (dx) / animationLength;
		float dzdt = (dz) / animationLength;
        
        /*
        if (animationLength == 0)
        {
            dxdt = dx;
            dzdt = dz;
        }
        */
        
        
		if(dx == 0.0f && dz == 0.0f)
		{
			//clamp y
			currPos.y = currHeight;
		}
		else //if( (Mathf.Abs(targetTilePos.X - currPos.x) < .5f && dz == 0.0f) || (dx == 0.0f && Mathf.Abs(targetTilePos.Y - currPos.z) < .5) )
		{
//			yDir = -1.0f;
//			height = targetHeight;
			currPos.y = targetHeight * Mathf.Sin(Mathf.PI * (currAnimationTime / animationLength));
		}

		currPos.x = prevTilePos.X + dxdt * currAnimationTime;
		currPos.z = prevTilePos.Y + dzdt * currAnimationTime;
	}

	public static float GetDistance(DJ_Point start, DJ_Point end, DJ_Distance distanceType)
	{
		float dist = 0.0f;

		float distSquared = Mathf.Abs(end.X - start.X) + Mathf.Abs(end.Y - start.Y);

		switch(distanceType)
		{
		case DJ_Distance.EUCLIDEAN:
			dist = Mathf.Sqrt(distSquared);
			break;
		case DJ_Distance.PATH:
			dist = distSquared;
			break;
		case DJ_Distance.MANHATTAN:
			dist = distSquared;
			break;
		case DJ_Distance.TILE:
			dist = Mathf.Floor(distSquared);
			break;
		default:
			break;
		}

		return dist;
	}

    public static bool isLayerOn(DJ_BeatActivation go)
    {
        if (go.instrument1 && DJ_BeatManager.layerOneActive)
        {
            return true;
        }
        else if (go.instrument2 && DJ_BeatManager.layerTwoActive)
        {
            return true;
        }
        else if (go.instrument3 && DJ_BeatManager.layerThreeActive)
        {
            return true;
        }
        else if (go.instrument4 && DJ_BeatManager.layerFourActive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool activateWithSound(DJ_BeatActivation go)
	{
        if (DJ_BeatManager.source[DJ_BeatManager.l_layerOne] != null && DJ_BeatManager.source[DJ_BeatManager.l_layerTwo] != null &&
            DJ_BeatManager.source[DJ_BeatManager.l_layerThree] != null && DJ_BeatManager.source[DJ_BeatManager.l_layerFour])
        {
            if (go.instrument1 && DJ_BeatManager.bass && DJ_BeatManager.source[DJ_BeatManager.l_layerOne].volume == 1.0f)
            {
                return true;
            }
            else if (go.instrumentThreshold1  && DJ_BeatManager.source[DJ_BeatManager.l_layerOne].volume == 1.0f)
            {
                return true;
            }
            else if (go.instrument2 && DJ_BeatManager.snare && DJ_BeatManager.source[DJ_BeatManager.l_layerTwo].volume == 1.0f)
            {
                return true;
            }
            else if (go.instrumentThreshold2 && DJ_BeatManager.source[DJ_BeatManager.l_layerTwo].volume == 1.0f)
            {
                return true;
            }
            else if (go.instrument3 && DJ_BeatManager.synth1 && DJ_BeatManager.source[DJ_BeatManager.l_layerThree].volume == 1.0f)
            {
                return true;
            }
            else if (go.instrumentThreshold3 && DJ_BeatManager.source[DJ_BeatManager.l_layerThree].volume == 1.0f)
            {
                return true;
            }
            else if (go.instrument4 && DJ_BeatManager.synth2 && DJ_BeatManager.source[DJ_BeatManager.l_layerFour].volume == 1.0f)
            {
                return true;
            }
            else if (go.instrumentThreshold4 && DJ_BeatManager.synth2 && DJ_BeatManager.source[DJ_BeatManager.l_layerFour].volume == 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public static bool activateWithNoSound(DJ_BeatActivation go)
    {
        if (go.instrument1 && DJ_BeatManager.bass)
        {
            return true;
        }
        else if (go.instrumentThreshold1 /*&& DJ_BeatManager.bassThreshold*/)
        {
            return true;
        }
        else if (go.instrument2 && DJ_BeatManager.snare)
        {
            return true;
        }
        else if (go.instrumentThreshold2 /*&& DJ_BeatManager.snareThreshold*/)
        {
            return true;
        }
        else if (go.instrument3 && DJ_BeatManager.synth1)
        {
            return true;
        }
        else if (go.instrumentThreshold3 /*&& DJ_BeatManager.synth1Threshold*/)
        {
            return true;
        }
        else if (go.instrument4 && DJ_BeatManager.synth2)
        {
            return true;
        }
        else if (go.instrumentThreshold4 /*&& DJ_BeatManager.synth2Threshold*/)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static DJ_Instrument activateWithSoundWithInstrument (DJ_BeatActivation go)
    {
        if (go.instrument1 && DJ_BeatManager.bass && DJ_BeatManager.synth2 && DJ_BeatManager.source[DJ_BeatManager.l_layerOne].volume == 1.0f)
        {
            return DJ_Instrument.ONE;
        }
        else if (go.instrumentThreshold1 /*&& DJ_BeatManager.bassThreshold*/ && DJ_BeatManager.source[DJ_BeatManager.l_layerOne].volume == 1.0f)
        {
            return DJ_Instrument.ONE;
        }
        else if (go.instrument2 && DJ_BeatManager.snare && DJ_BeatManager.source[DJ_BeatManager.l_layerTwo].volume == 1.0f)
        {
            return DJ_Instrument.TWO;
        }
        else if (go.instrumentThreshold2 /*&& DJ_BeatManager.snareThreshold*/ && DJ_BeatManager.source[DJ_BeatManager.l_layerTwo].volume == 1.0f)
        {
            return DJ_Instrument.TWO;
        }
        else if (go.instrument3 && DJ_BeatManager.synth1 && DJ_BeatManager.source[DJ_BeatManager.l_layerThree].volume == 1.0f)
        {
            return DJ_Instrument.THREE;
        }
        else if (go.instrumentThreshold3 /*&& DJ_BeatManager.synth1Threshold*/ && DJ_BeatManager.source[DJ_BeatManager.l_layerThree].volume == 1.0f)
        {
            return DJ_Instrument.THREE;
        }
        else if (go.instrument4 && DJ_BeatManager.synth2 && DJ_BeatManager.source[DJ_BeatManager.l_layerFour].volume == 1.0f)
        {
            return DJ_Instrument.FOUR;
        }
        else if (go.instrumentThreshold4 /*&& DJ_BeatManager.synth2Threshold*/ && DJ_BeatManager.source[DJ_BeatManager.l_layerFour].volume == 1.0f)
        {
            return DJ_Instrument.FOUR;
        }
        else
        {
            return DJ_Instrument.NONE;
        }
    }

}











