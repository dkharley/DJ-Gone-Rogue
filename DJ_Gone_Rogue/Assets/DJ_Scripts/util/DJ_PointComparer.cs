using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DJ_PointComparer : IEqualityComparer<DJ_Point>
{
	private static DJ_PointComparer _default;
	public static DJ_PointComparer Default
	{
		get
		{
			if (_default == null)
				_default = new DJ_PointComparer();
			return _default;
		}
	}
	
	public bool Equals(DJ_Point x, DJ_Point y)
	{
		return x.X == y.X && x.Y == y.Y;
	}
	
	public int GetHashCode(DJ_Point x)
	{
		return x.GetHashCode();
	}
}
