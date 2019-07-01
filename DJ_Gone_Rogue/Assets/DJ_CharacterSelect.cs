using UnityEngine;
using System.Collections;

public class DJ_CharacterSelect : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		visable = false;
		female = (GUITexture.Instantiate(femalePrefab) as GUITexture);
		male = (GUITexture.Instantiate(malePrefab) as GUITexture);
		femaleColor = female.color;
		maleColor = male.color;

		femaleColor.a = 0;
		maleColor.a = 0;
		female.color = femaleColor;
		male.color = maleColor;

		female.GetComponent<Collider>().enabled = false;
		male.GetComponent<Collider>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnMouseDown()
	{
		if(visable == false)
		{
			Debug.Log("Visable = false");
			femaleColor.a = 1;
			female.color = femaleColor;
			female.GetComponent<Collider>().enabled = true;
			maleColor.a = 1;
			male.color = maleColor;
			male.GetComponent<Collider>().enabled = true;
			visable = true;
		}
		else if(visable == true)
		{
			Debug.Log("Visable = true");
			femaleColor.a = 0;
			female.color = femaleColor;
			female.GetComponent<Collider>().enabled = false;
			maleColor.a = 0;
			male.color = maleColor;
			male.GetComponent<Collider>().enabled = false;
			visable = false;
		}
	}

	Color femaleColor;
	Color maleColor;

	public GUITexture femalePrefab;
	public GUITexture malePrefab;
	
	
	GUITexture female;
	GUITexture male;

	public bool visable;

}
