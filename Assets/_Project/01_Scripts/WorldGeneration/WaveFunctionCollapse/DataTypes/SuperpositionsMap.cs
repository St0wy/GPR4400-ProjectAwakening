using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectAwakening.WorldGeneration
{
    public class SuperpositionsMap
	{
		private Vector2Int size;
		private readonly TileSetScriptable tileSet;
		private readonly bool wrapAround = false;

		public SuperpositionsMap(Vector2Int size, TileSetScriptable tileSet)
		{
			this.size = size;
			this.tileSet = tileSet;
		}

		public SuperpositionsMap(Vector2Int size, TileSetScriptable tileSet, bool wrapAround)
		{
			this.size = size;
			this.tileSet = tileSet;
			this.wrapAround = wrapAround;
		}

		/// <summary>
		/// Map where each tile is in a superposition of possibilities, we store the index and the rotation
		/// </summary>
		public List<TileWFC>[,] SuperpositionMap { get; private set; }

		public List<TileWFC> this[int x, int y] => SuperpositionMap[x, y];

		public List<TileWFC> PossibilitiesAt(Vector2Int pos)
		{
			return SuperpositionMap[pos.x, pos.y];
		}

		/// <summary>
		/// Populate possibilities assuming every tile is possible
		/// </summary>
		public void PopulateMap()
		{
			SuperpositionMap = new List<TileWFC>[size.x, size.y];
			for (int x = 0; x < SuperpositionMap.GetLength(0); x++)
			{
				for (int y = 0; y < SuperpositionMap.GetLength(1); y++)
				{
					FillMapAt(x, y);
				}
			}
		}

		private void FillMapAt(int x, int y)
		{
			List<TileWFC> possibleTiles = new List<TileWFC>();

			//Put each tile
			for (int index = 0; index < tileSet.Tiles.Count; index++)
			{
				//in each possible rotation
				if (tileSet.Tiles[index].CanRotate)
				{
					for (int rotation = 0; rotation < 4; rotation++)
					{
						possibleTiles.Add(new TileWFC(index, rotation));
					}
				}
				else
				{
					possibleTiles.Add(new TileWFC(index, 0));
				}
			}

			SuperpositionMap[x, y] = possibleTiles;
		}

		/// <summary>
		/// Find the lowest point of entropy (lesser amount of possibilities) within the map
		/// </summary>
		/// <returns>Returns the position of the lowest entropy point or (0, -1) if all points have no entropy</returns>
		public Vector2Int FindLowestEntropy()
		{
			int lowestEntropy = int.MaxValue;
			Vector2Int lowestEntropyPoint = Vector2Int.down;

			//Go over the whole map
			for (int x = 0; x < SuperpositionMap.GetLength(0); x++)
			{
				for (int y = 0; y < SuperpositionMap.GetLength(1); y++)
				{
					//Check amount of entropy
					//Check that the tile has entropy
					if (SuperpositionMap[x, y].Count > 1)
					{
						//Check if the tile has less entropy than current lowest
						if (SuperpositionMap[x, y].Count < lowestEntropy)
						{
							lowestEntropy = SuperpositionMap[x, y].Count;
							lowestEntropyPoint = new Vector2Int(x, y);
						}
					}
				}
			}

			return lowestEntropyPoint;
		}

		/// <summary>
		/// Recalculate the possible tiles in neighbourings slots. Only call this on changed slots
		/// </summary>
		/// <param name="pos"></param>
		private void RecalculatePossibilities(Vector2Int pos)
		{
			Vector2Int direction = Vector2Int.up;

			//Recalculate the possibilities allowed by the possibles modules on this tile
			do
			{
				//Rotate the direction we look at
				direction = TileScriptable.NumDirectionToVector(
					(TileScriptable.VectorToNumDirection(direction) + 1) % 4);
				Vector2Int pos2 = pos + direction;

				//Check if the second tile would be outside the map
				if (pos2.x < 0 || pos2.x >= SuperpositionMap.GetLength(0) || pos2.y < 0 ||
				    pos2.y >= SuperpositionMap.GetLength(1))
				{
					if (!wrapAround) continue;

					pos2 = new Vector2Int((pos2.x + SuperpositionMap.GetLength(0)) % SuperpositionMap.GetLength(0),
						(pos2.y + SuperpositionMap.GetLength(1)) % SuperpositionMap.GetLength(1));
				}

				if (SuperpositionMap[pos2.x, pos2.y].Count == 1)
					continue;

				List<TileWFC> allowedPossibilities = new List<TileWFC>();

				//Recalculate allowed possibilities on this neighvours based on our current modules
				foreach (TileWFC tile in SuperpositionMap[pos.x, pos.y])
				{
					foreach (TileWFC possibility in SuperpositionMap[pos2.x, pos2.y])
					{
						if (!allowedPossibilities.Contains(possibility))
						{
							if (tileSet.Tiles[tile.Id].CheckCompatibility(
								    tileSet.Tiles[possibility.Id], TileScriptable.VectorToNumDirection(direction),
								    tile.Rotation, possibility.Rotation))
							{
								allowedPossibilities.Add(possibility);
							}
						}
					}
				}

				//Recalculate the possibilities of our neighbour's neighbours if we changed their possibilities
				if (allowedPossibilities.Count != SuperpositionMap[pos2.x, pos2.y].Count)
				{
					SuperpositionMap[pos2.x, pos2.y] = allowedPossibilities;
					RecalculatePossibilities(pos2);
				}
			} while (direction != Vector2Int.up);
		}

		public int ChooseRandomByWeight(Vector2Int pos)
		{
			float totalWeight = SuperpositionMap[pos.x, pos.y].Sum(tile => tileSet.Tiles[tile.Id].Weight);

			float randNum = Random.Range(0.0f, totalWeight);

			int choice = 0;
			int i = 0;
			foreach (TileWFC tile in SuperpositionMap[pos.x, pos.y])
			{
				randNum -= tileSet.Tiles[tile.Id].Weight;

				if (randNum < 0)
				{
					choice = i;
					break;
				}

				i++;
			}

			return choice;
		}

		public void CollapsePossibilities(Vector2Int pos, int chosenPos)
		{
			List<TileWFC> tiles = new List<TileWFC>();
			tiles.Add(SuperpositionMap[pos.x, pos.y][chosenPos]);

			SuperpositionMap[pos.x, pos.y] = tiles;

			RecalculatePossibilities(pos);
		}
	}
}