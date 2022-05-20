using System.Collections.Generic;
using System.Linq;
using StowyTools.Logger;
using UnityEngine;

namespace ProjectAwakening.Overworld.WaveFunctionCollapse
{
	[CreateAssetMenu(fileName = "TileSet", menuName = "ScriptableObjects/TileSets", order = 1)]
	public class TileSetScriptable : ScriptableObject
	{
		[SerializeField] private List<TileScriptable> tiles = new();
		public List<TileScriptable> Tiles => tiles;

		public void CalculateNeighbours()
		{
			//Reset all neighbours
			foreach (TileScriptable tile in tiles)
				tile.ResetNeighbours();

			//Check each tile
			foreach (TileScriptable t in tiles)
			{
				//Check each direction once (0 is up and turning clockwise)
				for (int direction = 0; direction < 4; direction++)
				{
					//Check each possible neighbour
					for (int neighbourIndex = 0; neighbourIndex < tiles.Count; neighbourIndex++)
					{
						for (int rotation1 = 0; rotation1 < 4; rotation1++)
						{
							for (int rotation2 = 0; rotation2 < 4; rotation2++)
							{
								//Check compatibility
								if (t.CheckCompatibility(tiles[neighbourIndex], direction, 0, rotation2))
								{
									//add the neighbour
									t.AddNeighbour(new TileWfc(neighbourIndex, rotation2), direction);
								}

								//Stop checking rotations if we can't rotate
								if (!tiles[neighbourIndex].CanRotate)
									break;
							}

							if (!t.CanRotate)
								break;
						}
					}
				}
			}
		}

		public void PrintTiles()
		{
			foreach (TileScriptable tile in tiles)
			{
				string up = tile.UpNeighbours.Aggregate("", (current, n) => current + (n.Id + "," + n.Rotation + "|"));
				string down =
					tile.DownNeighbours.Aggregate("", (current, n) => current + (n.Id + "," + n.Rotation + "|"));
				string left =
					tile.LeftNeighbours.Aggregate("", (current, n) => current + (n.Id + "," + n.Rotation + "|"));
				string right =
					tile.RightNeighbours.Aggregate("", (current, n) => current + (n.Id + "," + n.Rotation + "|"));

				this.Log(tile.name + " : \n" +
					"Up : " + up + "\n" +
					"Down : " + down + "\n" +
					"Left : " + left + "\n" +
					"Right : " + right);
			}
		}
	}
}