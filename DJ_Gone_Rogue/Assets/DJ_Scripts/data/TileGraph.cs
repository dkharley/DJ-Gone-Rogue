using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGraph
{

	public Dictionary<DJ_Point, TileNode> tileMap;
	public List<TileNode> tilePool;

	public TileGraph(GameObject parent, GameObject m_tilePrefab)
	{
		tileMap = new Dictionary<DJ_Point, TileNode>(DJ_PointComparer.Default);
		tilePool = new List<TileNode>();
		FillTileMap();
	}

	// Fills tile map with the current tiles in the scene
	void FillTileMap(){
		GameObject[] tileList;
		tileList = GameObject.FindGameObjectsWithTag("DJ_Tile");

		if (tileList.Length == 0){
			Debug.Log("No game objects are tagged 'DJ_Tile'");
		}
		else{
			//Debug.Log("There are " + tileList.Length + " Objects tagged 'DJ_Tile'");
		}

		GameObject tempTile;	          
		//iterates though all the tiles with the tag DJ_Tile
		for ( int i = 0; i < tileList.Length; i++)
        {
		//foreach( GameObject tile in tileList){
			tempTile = tileList[i];
			//ToDo: set parent to something
				
			Vector3 pos = tempTile.transform.position;
				
			//insert this DJ_Point and object into the map
            if (pos.y == 0)
            {

                DJ_Point newPoint = new DJ_Point((int)(pos.x), (int)(pos.z));


                //set the level dim values
                if (pos.x < DJ_TileManagerScript.MinX)
                    DJ_TileManagerScript.MinX = tempTile.transform.position.x;
                if (pos.x > DJ_TileManagerScript.MaxX)
                    DJ_TileManagerScript.MaxX = tempTile.transform.position.x;

                if (pos.z < DJ_TileManagerScript.MinZ)
                    DJ_TileManagerScript.MinZ = tempTile.transform.position.z;
                if (pos.z > DJ_TileManagerScript.MaxZ)
                    DJ_TileManagerScript.MaxZ = tempTile.transform.position.z;


                //checks to see two tiles are trying to occupy the same point
                if (tileMap.ContainsKey(newPoint))
                {
                    Debug.Log("Tile Exists already: " + newPoint.X + "," + newPoint.Y);
                }
                else
                {
                    //adds to the map that helps player movement

                    tileMap.Add(newPoint, new TileNode(newPoint, ref tempTile));

                    //create the links in the graph
                    LinkPossibleNeighbors(newPoint);
                    if (tempTile.GetComponent<DJ_TileScript>().checkpointTile)
                    {
                        DJ_TileManagerScript.checkpointList.Add(newPoint);
                        // checkpoint
                    }
                    else if (tempTile.GetComponent<DJ_TileScript>().spawnTile)
                    {
                        // spawn
                        DJ_TileManagerScript.checkpointList.Add(newPoint);
                        DJ_PlayerManager.spawnPoint = newPoint;
                    }
                    else if (tempTile.GetComponent<DJ_TileScript>().tutorialTile)
                    {
                        DJ_TileManagerScript.checkpointList.Add(newPoint);
                        DJ_PlayerManager.tutorialPoint = newPoint;
                    }
                    else if (tempTile.GetComponent<DJ_TileScript>().exitTile)
                    {
                        DJ_TileManagerScript.exitPoint = newPoint;
                    }
                    else if (tempTile.GetComponent<DJ_TileScript>().levelselectTile)
                    {
                        DJ_TileManagerScript.levelselectList.Add(newPoint);
                    }
                    else if (tempTile.GetComponent<DJ_TileScript>().teleportTile)
                    {
                        if (DJ_TileManagerScript.teleporterTiles.ContainsKey(tempTile.GetComponent<DJ_TeleporterTile>().teleporterPad))
                        {
                            DJ_TileManagerScript.teleporterTiles[tempTile.GetComponent<DJ_TeleporterTile>().teleporterPad].Add(tempTile);
                        }
                        else
                        {
                            List<GameObject> tempList = new List<GameObject>();
                            tempList.Add(tempTile);
                            DJ_TileManagerScript.teleporterTiles.Add(tempTile.GetComponent<DJ_TeleporterTile>().teleporterPad, tempList);
                        }
                    }
                    else if (tempTile.GetComponent<DJ_TileScript>().teleportPad)
                    {
                        DJ_TileManagerScript.teleporterPads.Add(tempTile.GetComponent<DJ_TeleporterPad>().teleportPadNumber, tempTile);
                    }
                    else
                    {
                        // normal
                    }
                    if (tempTile.GetComponent<DJ_TileScript>().fadingTile)
                    {
                        if (!tempTile.GetComponent<DJ_TileScript>().startVisible)
                        {
                            DJ_TileManagerScript.tilePool.Push(tileMap[new DJ_Point((int)tempTile.transform.position.x, (int)tempTile.transform.position.z)]);
                            tileMap.Remove(new DJ_Point((int)tempTile.transform.position.x, (int)tempTile.transform.position.z));
                            tempTile.gameObject.SetActive(false);
                        }
                    }
                    if (tempTile.GetComponent<DJ_TileScript>().recieverPad)
                    {
                        DJ_TileManagerScript.receiverTile.Add(tempTile.GetComponent<DJ_TileScript>().recieverNumber, tempTile);
                    }

                    if (tempTile.GetComponent<DJ_TileScript>().levelSelectTeleporterTile)
                    {
                        if (DJ_TileManagerScript.levelSelectTeleporterTile.ContainsKey(tempTile.GetComponent<DJ_LevelSelectTeleporter>().recieverNumber))
                        {
                            DJ_TileManagerScript.levelSelectTeleporterTile[tempTile.GetComponent<DJ_LevelSelectTeleporter>().recieverNumber].Add(tempTile);
                            //Debug.Log("adding levelSelect teleport tile to a created list");
                        }
                        else
                        {
                            //Debug.Log("adding levelSelect teleport tile by adding a list");
                            //Debug.Log("adding reciever number " + " = " + tempTile.GetComponent<DJ_LevelSelectTeleporter>().recieverNumber);
                            List<GameObject> tempList = new List<GameObject>();
                            tempList.Add(tempTile);
                            DJ_TileManagerScript.levelSelectTeleporterTile.Add(tempTile.GetComponent<DJ_LevelSelectTeleporter>().recieverNumber, tempList);
                            
                        }
                    }
                }
            }
		}
	}

	void LinkPossibleNeighbors (DJ_Point newPoint)
	{
		DJ_Point tempLeft = new DJ_Point(newPoint.X - 1, newPoint.Y);
		DJ_Point tempRight = new DJ_Point(newPoint.X + 1, newPoint.Y);
		DJ_Point tempUp = new DJ_Point(newPoint.X, newPoint.Y - 1);
		DJ_Point tempDown = new DJ_Point(newPoint.X, newPoint.Y + 1);

		//make neighbor links within the graph
		if (tileMap.ContainsKey (tempLeft))
		{
			tileMap [tempLeft].neighbors [(int)DJ_Dir.RIGHT] = tileMap [newPoint];
			tileMap [newPoint].neighbors [(int)DJ_Dir.LEFT] = tileMap [tempLeft];
		}
		if (tileMap.ContainsKey (tempRight))
		{
			tileMap [tempRight].neighbors [(int)DJ_Dir.LEFT] = tileMap [newPoint];
			tileMap [newPoint].neighbors [(int)DJ_Dir.RIGHT] = tileMap [tempRight];
		}
		if (tileMap.ContainsKey (tempUp))
		{
			tileMap [tempUp].neighbors [(int)DJ_Dir.DOWN] = tileMap [newPoint];
			tileMap [newPoint].neighbors [(int)DJ_Dir.UP] = tileMap [tempUp];
		}
		if (tileMap.ContainsKey (tempDown))
		{
			tileMap [tempDown].neighbors [(int)DJ_Dir.UP] = tileMap [newPoint];
			tileMap [newPoint].neighbors [(int)DJ_Dir.DOWN] = tileMap [tempDown];
		}
	}

	public void Render()
	{
		foreach(TileNode t in tileMap.Values)
		{
			t.Render();
		}

		//draw the paths. only works with one enemy
		if(tilesVisited.Count > 1)
		{
			for(int i = 0; i < tilesVisited.Count - 1; ++i)
			{
				if(tileMap.ContainsKey(tilesVisited[i + 1]) && tileMap.ContainsKey(tilesVisited[i]))
				{
					Vector3 from = new Vector3(tilesVisited[i].X, tileMap[tilesVisited[i]].tile.transform.position.y + .25f, tilesVisited[i].Y);
					Vector3 to = new Vector3(tilesVisited[i + 1].X, tileMap[tilesVisited[i + 1]].tile.transform.position.y + .25f, tilesVisited[i + 1].Y);
					Debug.DrawLine(from, to, Color.green);
				}
			}
		}
	}

	public void DeactivateTile(DJ_Point tilePos)
	{
		if(tileMap.ContainsKey(tilePos))
		{
			//remove from neighbors to tile
			for(int i = 0; i < tileMap[tilePos].neighbors.Length; ++i)
			{
				if(tileMap[tilePos].neighbors[i] != null)
				{
					for(int t = 0; t < tileMap[tilePos].neighbors[i].neighbors.Length; ++t)
					{
						if(tileMap[tilePos].neighbors[i].neighbors[t] != null)
						{
							if(tileMap[tilePos].neighbors[i].neighbors[t].pos.Equals(tilePos))
							{
								tileMap[tilePos].neighbors[i].neighbors[t] = null;
							}
						}
					}
				}
			}

			tileMap[tilePos].Deactivate();

			tilePool.Add (tileMap[tilePos]);
			tileMap.Remove (tilePos);
		}
	}

	private List<DJ_Point> tilesVisited = new List<DJ_Point>();
	private List<float> currentCosts = new List<float>();

	public DJ_Dir GetNextMove(DJ_Point from, DJ_Point to, float maxDist, DJ_Distance distanceType)
	{
		tilesVisited.Clear();
		currentCosts.Clear();

		tilesVisited.Add (from);

		currentCosts.Add (0.0f);

		if(tileMap.ContainsKey(from))
		{
			DJ_Dir prefDir = DJ_Util.GetPreferredDirection(from, to);

			float currentCost = 0.0f;

			//immediately return none if the preferred direction
			//happens to be none. This either means the positions, from
			//and to, are the same or that there is no path from "from" to "to"
			if( prefDir == DJ_Dir.NONE )
				return prefDir;

			//Debug.Log("the preferred direction is " + prefDir.ToString());

			int index;

			if(tileMap[from].neighbors[(int)prefDir] != null && DJ_LevelManager.isTileAvailable(tileMap[from].neighbors[(int)prefDir].pos))
			{
				//push info  onto queue
				tilesVisited.Add(tileMap[from].neighbors[(int)prefDir].pos);
				currentCosts.Add(currentCost);

				if(GetNextMove(tileMap[from].neighbors[(int)prefDir].pos, to, 0.0f, maxDist, distanceType, currentCost + 1.0f))
				{

					return prefDir;
				}

				//remove info from queue
				index = tilesVisited.IndexOf(tileMap[from].neighbors[(int)prefDir].pos);
				currentCosts.RemoveAt(index);
				tilesVisited.Remove(tileMap[from].neighbors[(int)prefDir].pos);
			}

			for(int i = 0; i < tileMap[from].neighbors.Length; ++i)
			{
				if(tileMap[from].neighbors[i] != null && i != (int)prefDir && DJ_LevelManager.isTileAvailable(tileMap[from].neighbors[i].pos))
				{
					tilesVisited.Add(tileMap[from].neighbors[i].pos);
					currentCosts.Add(currentCost);

					if(GetNextMove(tileMap[from].neighbors[i].pos, to, 0.0f, maxDist, distanceType, currentCost + 1.0f))
					{

						return DJ_Util.GetPreferredDirection(from, tileMap[from].neighbors[i].pos);
					}

					index = tilesVisited.IndexOf(tileMap[from].neighbors[i].pos);
					currentCosts.RemoveAt(index);
					tilesVisited.Remove(tileMap[from].neighbors[i].pos);
				}
			}
		}

		return DJ_Dir.NONE;
	}


	private bool GetNextMove(DJ_Point from, DJ_Point to, float currDist, float maxDist, DJ_Distance distanceType, float currentCost)
	{
		if(from.Equals(to))
		{
			return true;
		}

		if(!tileMap.ContainsKey(to) && DJ_Util.GetDistance(from, to, DJ_Distance.EUCLIDEAN) <= 1)
			return false;
		
		if(currDist > maxDist)
			return true;

		if(tileMap.ContainsKey(from))
		{
			DJ_Dir prefDir = DJ_Util.GetPreferredDirection(from, to);

			int index;

			//immediately return none if the preferred direction
			//happens to be none. This either means the positions, from
			//and to, are the same or that there is no path from "from" to "to"
			if( prefDir == DJ_Dir.NONE )
				return true;
			
			if(tileMap[from].neighbors[(int)prefDir] != null && DJ_LevelManager.isTileAvailable(tileMap[from].neighbors[(int)prefDir].pos))
			{
				if(!tilesVisited.Contains(tileMap[from].neighbors[(int)prefDir].pos))
				{
					tilesVisited.Add(tileMap[from].neighbors[(int)prefDir].pos);
					currentCosts.Add(currentCost);
				}
				else
				{
					index = tilesVisited.IndexOf(tileMap[from].neighbors[(int)prefDir].pos);

					float currCost = currentCosts[index];

					if(currentCost < currCost)
					{
						tilesVisited.RemoveAt(index);
						currentCosts.RemoveAt(index);

						tilesVisited.Add(tileMap[from].neighbors[(int)prefDir].pos);
						currentCosts.Add(currentCost);
					}
					else
						return false;
				}
				
				if(GetNextMove(tileMap[from].neighbors[(int)prefDir].pos, to, currDist + 1.0f, maxDist, distanceType, currentCost + 1.0f))
				{

					return true;
				}

				index = tilesVisited.IndexOf(tileMap[from].neighbors[(int)prefDir].pos);

				currentCosts.RemoveAt(index);
				tilesVisited.Remove(tileMap[from].neighbors[(int)prefDir].pos);
			}
			
			for(int i = 0; i < tileMap[from].neighbors.Length; ++i)
			{
				if(tileMap[from].neighbors[i] != null && i != (int)prefDir && DJ_LevelManager.isTileAvailable(tileMap[from].neighbors[i].pos))
				{
					tilesVisited.Add(tileMap[from].neighbors[i].pos);
					currentCosts.Add(currentCost);
					
					if(GetNextMove(tileMap[from].neighbors[i].pos, to, currDist + 1.0f, maxDist, distanceType, currentCost + 1.0f))
					{

						return true;
					}

					index = tilesVisited.IndexOf(tileMap[from].neighbors[i].pos);
					currentCosts.RemoveAt(index);
					tilesVisited.Remove(tileMap[from].neighbors[i].pos);
				}
			}
		}

		return false;
	}

	public void Update ()
	{
	
	}
}

