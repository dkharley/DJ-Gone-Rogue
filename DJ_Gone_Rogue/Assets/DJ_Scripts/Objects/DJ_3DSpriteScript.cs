using UnityEngine;
using System.Collections;

public class DJ_3DSpriteScript : DJ_EntityScript {

	public DJ_3DSpriteScript()
		: base()
	{

	}

	// Use this for initialization
	public override void Start ()
	{
		base.Start();

		m_spriteTransform = gameObject.transform.GetChild(0);
		
		//rotate the entity so that is facing the correct direction
		//Note*** Change this so it uses the  opposite of the main cameras forward direction
		//instead of Vector3.back
		m_spriteTransform.rotation = Quaternion.FromToRotation(m_spriteTransform.up, Vector3.up);
		m_spriteTransform.rotation = Quaternion.FromToRotation(m_spriteTransform.forward, Vector3.back);
		m_spriteTransform.rotation.SetLookRotation(-Camera.main.transform.forward);

		//change the position of the attached sprite plane
		//so the origin located at the middle center of the plane
		m_spriteTransform.transform.position =
			new Vector3(m_spriteTransform.transform.position.x,
			            m_spriteTransform.transform.position.y + (m_spriteTransform.GetComponent(typeof(MeshRenderer)) as MeshRenderer).bounds.size.y / 2.0f,
			            m_spriteTransform.transform.position.z);

		(m_spriteTransform.GetComponent(typeof(MeshRenderer)) as MeshRenderer).material.mainTexture = m_texture;

	}
	
	// Update is called once per frame
	public override void Update ()
	{
		base.Update();

		m_spriteTransform.LookAt(m_spriteTransform.position + Camera.main.transform.forward);

		if(gameObject.GetComponent<DJ_Movement>())
		{
			switch (gameObject.GetComponent<DJ_Movement> ().direction) 
			{
			case DJ_Dir.UP: //(m_spriteTransform.GetComponent(typeof(MeshRenderer)) as MeshRenderer).material.mainTexture
				m_spriteTransform.localScale = new Vector3( Mathf.Abs(m_spriteTransform.localScale.x), m_spriteTransform.localScale.y, m_spriteTransform.localScale.z);
				break;
	        case DJ_Dir.RIGHT: //(m_spriteTransform.GetComponent(typeof(MeshRenderer)) as MeshRenderer).material.mainTexture
	            m_spriteTransform.localScale = new Vector3(Mathf.Abs(m_spriteTransform.localScale.x), m_spriteTransform.localScale.y, m_spriteTransform.localScale.z);
	            break;
	        case DJ_Dir.LEFT: //(m_spriteTransform.GetComponent(typeof(MeshRenderer)) as MeshRenderer).material.mainTexture
	            m_spriteTransform.localScale = new Vector3(Mathf.Abs(m_spriteTransform.localScale.x) * -1, m_spriteTransform.localScale.y, m_spriteTransform.localScale.z);
	            break;
			case DJ_Dir.DOWN:
				m_spriteTransform.localScale = new Vector3(Mathf.Abs (m_spriteTransform.localScale.x) * -1, m_spriteTransform.localScale.y, m_spriteTransform.localScale.z);
				break;
			default:
				break;
			}
		}
	}

	//this is the visual representation of this "billboarded" entity.
	//it should only be used to reflect to visual state of this entity
	private Transform m_spriteTransform;
}

