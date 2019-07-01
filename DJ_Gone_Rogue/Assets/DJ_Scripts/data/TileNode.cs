using UnityEngine;
using System.Collections;

public class TileNode
{
	public DJ_Point pos;
	public GameObject tile;
	public TileNode[] neighbors;
	
	public TileNode(DJ_Point tilePos, ref GameObject tile)
	{
		neighbors = new TileNode[4];
		pos = new DJ_Point(0,0);
		pos.Set (tilePos);
		this.tile = tile;
	}
	
	public void AddNeighbor(TileNode neighbor)
	{
		if(DJ_Util.GetDistance(pos, neighbor.pos, DJ_Distance.PATH) <= 1)
		{
			float dx = neighbor.pos.X - pos.X;
			float dy = neighbor.pos.Y - pos.Y;
			
			if(dx != dy)
			{
				if(dx > 0)
				{
					neighbors[(int)DJ_Dir.RIGHT] = neighbor;
				}
				else if(dx < 0)
				{
					neighbors[(int)DJ_Dir.LEFT] = neighbor;
				}
				else if(dy > 0)
				{
					neighbors[(int)DJ_Dir.UP] = neighbor;
				}
				else if(dy < 0)
				{
					neighbors[(int)DJ_Dir.DOWN] = neighbor;
				}
			}
		}
	}

	public void Render()
	{
		for(int i = 0; i < neighbors.Length; ++i)
		{
			if(neighbors[i] != null)
				Debug.DrawLine(new Vector3(pos.X, tile.transform.position.y + .25f, pos.Y),
				               new Vector3(neighbors[i].pos.X,
				            		neighbors[i].tile.transform.position.y + .25f,
				            		neighbors[i].pos.Y)
				               );
		}
	}

	public void Deactivate()
	{
		tile.SetActive(false);

		//free links
		neighbors[0] = null;
		neighbors[1] = null;
		neighbors[2] = null;
		neighbors[3] = null;
	}
}
