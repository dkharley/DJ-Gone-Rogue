/// <summary>
/// D j_ point. Class that represents a point.
/// @author - Wyatt Sanders 1/15/2014
/// </summary>
/// 
public class DJ_Point
{
	public DJ_Point(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}

	public int X;
	public int Y;

	public override string ToString ()
	{
		return string.Format ("Tile:({0}, {1})", X, Y);
	}

	public override bool Equals (object point)
	{
		return X == ((DJ_Point)point).X && Y == ((DJ_Point)point).Y;
	}

	public override int GetHashCode ()
	{
		return ToString().GetHashCode();
	}

	public void Set(DJ_Point p)
	{
		X = p.X;
		Y = p.Y;
	}
}
