/// <summary>
/// D j_ dir. Enum used to signify direction on the xz plane.
/// 
/// @author - Wyatt Sanders
/// </summary>


public enum DJ_Dir
{
	//[OppositeAttribute(DOWN)]
	UP = 0,
	//[OppositeAttribute(RIGHT)]
	LEFT,
	//[OppositeAttribute(UP)]
	DOWN,
	//[OppositeAttribute(LEFT)]
	RIGHT,
	//[OppositeAttribute(NONE)]
	NONE,
    TP
}

//public class OppositeAttribute : System.Attribute
//{
//	public OppositeAttribute(DJ_Dir opp)
//	{
//		opposite = opp;
//	}
//
//	public DJ_Dir opposite
//	{
//		get;
//		private set;
//	}
//}
