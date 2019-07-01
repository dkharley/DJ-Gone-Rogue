using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// D j_ entity script. This is a script that represents the
/// most basic of game entities that will exist in the game
/// world. Player, Tiles, Walls, Items, etc. extend this script.
/// 
/// @author Wyatt Sanders 1/9/2014
/// </summary>

public class DJ_EntityScript : MonoBehaviour
{

	public DJ_EntityScript()
	{

	}

	// Use this for initialization
	public virtual void Start ()
	{

	}

	public void Awake()
	{
		m_parent = null;
		m_children = new List<Transform>();
		
		tilePos = new DJ_Point(0,0);
		size = new DJ_Point(0,0);
		direction = DJ_Dir.NONE;
		
		//assign the sprite (the one assigned via the  inspector) to that of the Sprite Renderer component
		//that is attached to this entity
		//Debug.Log(m_texture);
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		tilePos.X = (int)(transform.position.x + .5f);
		tilePos.Y = (int)(transform.position.z + .5f);
	}

	/// <summary>
	/// Resets this instance and preps it for reuse/storage within a pool.
	/// </summary>
	public virtual void Reset()
	{

	}

	/// <summary>
	/// Releases all resource used by the <see cref="DJ_EntityScript"/> object.
	/// </summary>
	/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="DJ_EntityScript"/>. The <see cref="Dispose"/>
	/// method leaves the <see cref="DJ_EntityScript"/> in an unusable state. After calling <see cref="Dispose"/>, you must
	/// release all references to the <see cref="DJ_EntityScript"/> so the garbage collector can reclaim the memory that
	/// the <see cref="DJ_EntityScript"/> was occupying.</remarks>
	public virtual void Dispose()
	{

	}

	
	/// <summary>
	/// This is a reference to the texture that is
	/// used by this classe/script and thus
	/// entities of this type that exist in the game world
	/// Example: If I have a Tile script that extends
	/// this Entity script, the Tile script will contain
	/// a reference to this texture. Assign the reference
	/// in the prefab via the inspector.
	/// </summary>
	public Texture m_texture;
	
	public List<Transform> m_children;
	
	public Transform m_parent;

	/// <summary>
	/// Component variables used for all objects
	/// E.G. Player, Enemies, Tiles, Items.
	/// </summary>

	// Size of the object
	public DJ_Point size;
	// Position of the object based on the grid system
	public DJ_Point tilePos;
	// Position of the object based on the world coordinates
	public Vector3 truePos;
	// Direction of the object for movement
	public DJ_Dir direction;
}






