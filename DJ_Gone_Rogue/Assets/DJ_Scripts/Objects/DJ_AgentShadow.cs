using UnityEngine;
using System.Collections;

public class DJ_AgentShadow : MonoBehaviour {
	/// <summary>
	/// DJ_AgentShadow script. Makes a cool little circular shadow that follows the agent's 
	/// height when it hops. 
	/// 
	/// @author Peter Kong 2/12/2014
	/// </summary>
	public Sprite shadow_sprite;

	private SpriteRenderer _renderer;
	private GameObject agent;
	private GameObject shadow;
	private float scale;

	// Use this for initialization
	void Start () {
		agent = this.gameObject;
		shadow = new GameObject ("shadow_cast");
		_renderer = shadow.AddComponent<SpriteRenderer> ();
		_renderer.sprite = shadow_sprite;
		shadow.transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(0.0f, 1.0f, 0.0f));
		// WOW. So alpha, much channel
		_renderer.color = new Color (0f, 0f, 0f, 0.4f);
	}
	
	// Update is called once per frame
	void Update () {
		// Make the shadow dissappear when the player drops off.
		if (DJ_PlayerManager.player.GetComponent<DJ_Movement>().isFalling == true)
			_renderer.enabled = false;
		else
			_renderer.enabled = true;
		// Draw shadow @ Player's x,z
		shadow.transform.position = new Vector3 (agent.transform.position.x, 0.1f, agent.transform.position.z+0.2f);
		// Scale shadow @ Player's y
		scaleShadow ();
	}

	private void scaleShadow()
	{
		if (agent.transform.position.y > 0) 
			scale = 1-(agent.transform.position.y)/2;
		else if(agent.transform.position.y == 0)
			scale = 1;
		shadow.transform.localScale = new Vector3(scale, scale);
	}
}
