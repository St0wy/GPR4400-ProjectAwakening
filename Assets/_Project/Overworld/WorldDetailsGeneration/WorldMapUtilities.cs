using System.Collections.Generic;
using System.Linq;
using ProjectAwakening.Overworld.WaveFunctionCollapse;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ProjectAwakening.Overworld.WorldDetailsGeneration
{
	public static class WorldMapUtilities
	{
		/// <summary>
		/// Convert a tilemap to an array of dimension 2 where true corresponds to a wall
		/// </summary>
		/// <returns>array dim2 where true means there is a wall there</returns>
		public static bool[,] SuperPositionMapToArray(SuperpositionsMap oldMap, TileSetScriptable tileSet)
		{
			var size = new Vector2Int(oldMap.SuperpositionMap.GetLength(0), oldMap.SuperpositionMap.GetLength(1));
			var newMap = new bool[size.x, size.y];

			// Populate the map
			for (var x = 0; x < size.x; x++)
			{
				for (var y = 0; y < size.y; y++)
				{
					// Check that the tile has no collider
					RuleTile rule = tileSet.Tiles[oldMap[x, y][0].Id].RuleTile;
					if (rule.m_DefaultColliderType != Tile.ColliderType.None)
					{
						newMap[x, y] = true;
					}
					else
					{
						newMap[x, y] = false;
					}
				}
			}

			return newMap;
		}

		/// <summary>
		/// Finds via flood fill the largest area in a map
		/// </summary>
		/// <param name="map">the map to find in</param>
		/// <param name="matchValue">wether we match against true or false</param>
		/// <returns>all positions in the map that belong to the largest area</returns>
		public static List<Vector2Int> GetLargestArea(bool[,] map, bool matchValue = true)
		{
			// Storage for the amount of tiles in each area
			var amountsPerId = new Dictionary<int, int>();

			var size = new Vector2Int(map.GetLength(0), map.GetLength(1));

			// Map of ids to know what tiles belong to what area
			var ids = new int[size.x, size.y];

			var curId = 0;

			//Fill ids with 0s
			for (var x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					ids[x, y] = 0;
				}
			}

			//Call flood fill on each valid tile
			for (var x = 0; x < size.x; x++)
			{
				for (var y = 0; y < size.y; y++)
				{
					//Check tile is valid
					if (map[x, y] != matchValue || ids[x, y] != 0) continue;

					curId++;
					amountsPerId.Add(curId, RecursiveFloodFill(new Vector2Int(x, y), curId, ids, map));
				}
			}

			//Find largest of the found areas
			var largest = 0;
			var biggestId = 0;
			foreach (KeyValuePair<int, int> keyPair in amountsPerId.Where(keyPair => keyPair.Value > largest))
			{
				largest = keyPair.Value;
				biggestId = keyPair.Key;
			}

			//Return the tiles belonging to that area
			var tilesPos = new List<Vector2Int>();
			for (var x = 0; x < size.x; x++)
			{
				for (var y = 0; y < size.y; y++)
				{
					if (ids[x, y] == biggestId)
					{
						tilesPos.Add(new Vector2Int(x, y));
					}
				}
			}

			return tilesPos;
		}

		/// <summary>
		/// Recursively flood fills a tile and its neighbours to find an area
		/// </summary>
		/// <param name="pos">The position of the current tile</param>
		/// <param name="id">The id of the area</param>
		/// <param name="idMap">Map of ids where we set the id</param>
		/// <param name="map">the map where we check if something can belong to our area</param>
		/// <returns>the number of tiles in the area</returns>
		private static int RecursiveFloodFill(Vector2Int pos, int id, int[,] idMap, bool[,] map)
		{
			//Set our id
			idMap[pos.x, pos.y] = id;

			var amountOfTilesFilled = 1;

			Vector2Int direction = Vector2Int.down;

			for (var i = 0; i < 4; i++)
			{
				//Switch direction
				direction = i switch
				{
					0 => Vector2Int.down,
					1 => Vector2Int.left,
					2 => Vector2Int.right,
					3 => Vector2Int.up,
					_ => direction,
				};

				Vector2Int neighbourPos = pos + direction;

				//Check that neighbourPos is valid
				if (neighbourPos.x < 0 || neighbourPos.x >= map.GetLength(0) || neighbourPos.y < 0 ||
				    neighbourPos.y >= map.GetLength(1))
					continue;

				//Check that neighbour can be added
				if (idMap[neighbourPos.x, neighbourPos.y] != 0 ||
				    map[neighbourPos.x, neighbourPos.y] != map[pos.x, pos.y])
					continue;

				//Call recursive floodFill on that tile and the result
				amountOfTilesFilled += RecursiveFloodFill(neighbourPos, id, idMap, map);
			}

			return amountOfTilesFilled;
		}

		public static IEnumerable<Vector3> ConvertTilemapToWorld(
			IEnumerable<Vector2Int> tilemapPositions,
			Tilemap conversionTilemap)
		{
			return tilemapPositions.Select(oldPos => conversionTilemap.CellToWorld((Vector3Int) oldPos)).ToList();
		}
	}
}