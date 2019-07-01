using UnityEngine;
using System.Collections;

public class menuManager : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		InstantiateManager(menuItems);
	}

	void InstantiateManager (GameObject managerPrefab)
	{
		GameObject manager = (GameObject.Instantiate (managerPrefab)) as GameObject;
		manager.transform.parent = transform;
		manager.transform.position = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject menuItems;
}
